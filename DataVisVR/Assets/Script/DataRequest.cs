using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Newtonsoft.Json.Linq;

public class DataRequest : MonoBehaviour {

	private GameObject VRCamera;
	private LineRenderer lineRenderer;

	Thread thread;
	public string address = "http://127.0.0.1:5000/show_tree";
	private string jsonText;
	public JObject json = null;
	public IDictionary<string, GameObject> nodes = new Dictionary<string, GameObject>();
	public IList<string> keys = new List<string>();
	public IDictionary<string, string> keys_relation = new Dictionary<string, string> ();

	public int numberOfCluster;
	public int pivot = 0;
	public GameObject obj;
	public int maxNumberOfShow = 5000;

	private bool finishLoad = false;
	private bool converted = false;
	private bool finishRender = false;

	IEnumerator Start () {
		VRCamera = GameObject.Find("LMHeadMountedRig");

		WWW request = new WWW(address);
		yield return request;
		jsonText = request.text;

		thread = new Thread(convert_to_jobject);
		thread.Start();
	}

	void convert_to_jobject()
	{
		this.json = JObject.Parse(jsonText);
		numberOfCluster = json.Count;
		Debug.Log(numberOfCluster);
		foreach (var node in json)
		{
			keys.Add(node.Key.ToString());
		}
		converted = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (converted && !finishRender) {
			int count = 0;
			while (pivot < numberOfCluster) {
				string tid = keys [pivot];
				var node = json [tid];

				obj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				obj.transform.position = new Vector3 ((float)node ["x"], (float)node ["y"], (float)node ["z"]);
				obj.transform.localScale = new Vector3 (4f, 4f, 4f);
				obj.name = tid;
				obj.tag = "sphere";
				obj.GetComponent<MeshRenderer> ().receiveShadows = false;
				obj.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

				nodes.Add (tid, obj);
				keys_relation.Add (tid, null);

				foreach (var subNode in node["subNode"]) {
					obj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
					obj.transform.position = new Vector3 ((float)subNode ["x"], (float)subNode ["y"], (float)subNode ["z"]);
					obj.transform.localScale = new Vector3 (1f, 1f, 1f);
					obj.name = subNode["tid"].ToString ();
					obj.tag = "sphere";
					obj.GetComponent<MeshRenderer> ().receiveShadows = false;
					obj.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

					nodes.Add (obj.name, obj);
					keys_relation.Add (obj.name, tid);
				}

				pivot++;
				count++;

				if (count >= 5) {
					break;
				}
			}

			if (pivot > maxNumberOfShow) {
				finishRender = true;
			}
		}
	}

	public GameObject getTweet(string tid) {
		return nodes [tid];
	}

	public IList<GameObject> getSubTreeObject(string tid) {
		JToken subTreeJson;

		if (json [tid] == null) {
			subTreeJson = json [keys_relation [tid]];
		} else {
			subTreeJson = json [tid];
		}

		IList<GameObject> subTreeObject = new List<GameObject>();
		subTreeObject.Add(nodes[subTreeJson["tid"].ToString()]);

		foreach (var subNode in subTreeJson["subNode"]) {

			subTreeObject.Add(nodes[subNode["tid"].ToString()]);

		}

		return subTreeObject;
	}
}
