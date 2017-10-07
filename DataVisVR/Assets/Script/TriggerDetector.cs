using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MongoDB.Bson;
using Leap;
using Leap.Unity;
using System.Threading;

public class TriggerDetector : MonoBehaviour {

	BsonDocument tweetInfo = null;
	GameObject popCanvas;
	DataRequest data;
	GameObject temp;
	IList<GameObject> tempNodes;
	int[][] offsets;

	private float timeCount = 0f;
	private bool toRender = false;
	private IDictionary<string, bool> reform_table = new Dictionary<string, bool> ();
	private int pivot = 1;

	private AudioSource audio;

	private LeapProvider provider;
	private Frame frame;
	private bool rightPinch = false;
	private Vector3 indexPos;
	private Vector3 originalPos;
	private GameObject selectedNode;
	private string selectedNodeName;
	private BsonDocument info;

	private Thread thread;
	private GameObject window;
	private Text detail;
	private bool isSet = false;
	private bool isSelected = false;
	private DatabaseConn databaseConnector;

	void Start() {
		data = GameObject.Find ("LMHeadMountedRig").GetComponent<DataRequest>();
		window = GameObject.Find ("PopCanvas");
		detail = GameObject.Find ("Detail").GetComponent<Text> ();
		databaseConnector = GameObject.Find ("DatabaseConn").GetComponent<DatabaseConn> ();
		audio = GetComponent<AudioSource> ();

		provider = FindObjectOfType<LeapProvider>() as LeapProvider;
		indexPos = gameObject.transform.position;

		int[] o1 = new int[3] { 1, 1, 1 };
		int[] o2 = new int[3] { -1, 1, 1 };
		int[] o3 = new int[3] { -1, 1, -1 };
		int[] o4 = new int[3] { 1, 1, -1 };
		int[] o5 = new int[3] { 1, -1, 1 };
		int[] o6 = new int[3] { -1, -1, 1 };
		int[] o7 = new int[3] { -1, -1, -1 };
		int[] o8 = new int[3] { 1, -1, -1 };
		offsets = new int[][] { o1, o2, o3, o4, o5, o6, o7, o8 };
	}

	void Update () {

		frame = provider.CurrentFrame;
		foreach (Hand hand in frame.Hands)
		{
			if (hand.IsRight)
			{
				rightPinch = hand.PinchStrength > 0.8 ? true : false;
			}

		}

		if (selectedNode != null && rightPinch) {
			selectedNode.GetComponent<Renderer> ().material.color = new Color (255,128,0);
			selectedNode.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			selectedNode.transform.position = transform.position;
			selectedNode.transform.rotation = transform.rotation;

			Vector3 pos = transform.position;
			pos.z += 0.2f;
			pos.y += 0.2f;
			window.transform.position = pos;

			if (!isSet && info != null) {

				if (info ["id_str"] == selectedNodeName) {
					detail.text = "@" + info ["user"] ["screen_name"] + "\n" + info ["text"];
				} else {
					detail.text = "@" + info ["retweeted_status"]["user"] ["screen_name"] + "\n" + info ["retweeted_status"]["text"];
				}

				isSet = true;
			}

		}

		if (!rightPinch && selectedNode != null) {
			selectedNode.transform.position = originalPos;
			selectedNode.GetComponent<Renderer> ().material.color = Color.blue;
			selectedNode = null;
			selectedNodeName = null;
			info = null;
			isSet = false;

			window.transform.position = new Vector3 (2048, 2048, 2048);
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "sphere") {

			if (tempNodes != null) {
				tempNodes.Clear ();
			}
			tempNodes = data.getSubTreeObject (other.name);


			if (!reform_table.ContainsKey (tempNodes [0].name)) {

				reform_table.Add (tempNodes [0].name, true);

				StartCoroutine (rearrange (tempNodes));
			}

			if (!rightPinch && other.gameObject.tag == "sphere") {
				audio.Play ();
			}
		}
	}

	void OnTriggerStay(Collider other) {

		if (other.gameObject.tag == "sphere") {


			if (rightPinch || selectedNode != null) {
				return;
			}

			Debug.Log ("Trigger....");
			if (reform_table.Count > 0) {
				if (reform_table [tempNodes [0].name] == true) {
					Debug.Log("Selected...");
					selectedNode = data.getTweet (other.name);
					selectedNodeName = selectedNode.name;
					originalPos = selectedNode.transform.position;

					thread = new Thread(getDetailTweet);
					thread.Start();
				}
			}
		}
	}

	// called when trigger finished
	void OnTriggerExit(Collider other)
	{
	}

	IEnumerator rearrange(IList<GameObject> nodes) {

		int length = nodes.Count;
		var root = nodes [0];
		root.transform.localScale = new Vector3(1f,1f,1f);
		root.GetComponent<Renderer> ().material.color = Color.red;

		var nodePos = root.transform.position;

		GameObject camera = GameObject.Find ("LMHeadMountedRig");
		camera.transform.position = new Vector3 (nodePos.x, nodePos.y, nodePos.z - 10);


		for (int i = 1; i < length; i++) {
			nodePos.x = root.transform.position.x + (i / 2f) * (offsets[(i-1)%8][0]) + 1/i;
			nodePos.y = root.transform.position.y + (i / 2f) * (offsets[(i-1)%8][1]) + 1/i;
			nodePos.z = root.transform.position.z + (i / 2f) * (offsets[(i-1)%8][2]) + 1/i;
			nodes [i].transform.position = nodePos;
//			nodes [i].transform.localScale = new Vector3 (1f/(i+1),1f/(i+1),1f/(i+1));
			nodes [i].transform.localScale = new Vector3 (1f/4,1f/4,1f/4);
			nodes[i].GetComponent<Renderer> ().material.color = Color.red;

			nodes [i].AddComponent<LineRenderer> ();
			var lineRenderer = nodes [i].GetComponent<LineRenderer>();

			lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
			lineRenderer.startColor = Color.yellow;
			lineRenderer.endColor = Color.red;
			lineRenderer.startWidth = 1f/length/2;
			lineRenderer.endWidth = 1f/length/2;
			lineRenderer.SetPosition(0, nodePos);
			lineRenderer.SetPosition(1, nodes[i-1].transform.position);

		}

		yield return null;
	}

	IEnumerator pauseInUpdate(float sec) {
		yield return new WaitForSeconds (sec);
	}
		
	void getDetailTweet() {		
		info = databaseConnector.getTweetInfo (selectedNodeName);
	}
}
