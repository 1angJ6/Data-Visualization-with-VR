using UnityEngine;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Newtonsoft.Json.Linq;

public class urlRequest : MonoBehaviour
{

    private GameObject VRCamera;
    private LineRenderer lineRenderer;

    Thread thread;
    public string address = "http://127.0.0.1:5000/list_nodes";
    private string jsonText;

    public JObject json = null;
    public IDictionary<string, GameObject> nodes = new Dictionary<string, GameObject>();
    public IList<string> keys = new List<string>();
    public int numberOfNodes;
    public int pivot;
    public GameObject obj;
    public int maxFollower = 0;
    public int maxNumberOfShow = 50000;

    public bool finishLoad = false;
    public bool converted = false;
    public bool finishRender = false;

    IEnumerator Start()
    {
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
        numberOfNodes = json.Count;
        Debug.Log(numberOfNodes);
        foreach (var node in json)
        {

			if (node.Key.ToString ().Equals("796269755191570434")) {
				Debug.Log (node.Value["x"].ToString());
			}
		

            keys.Add(node.Key.ToString());
            if ((int)node.Value["followers_count"] > maxFollower)
            {
                maxFollower = (int)node.Value["followers_count"];
            }
        }
        Debug.Log(maxFollower);

        converted = true;
    }

    // Update is called once per frame
    void Update()
    {

        //if (converted && !finishRender)
        //{
        //    var node = json["796405440427028480"]

        //    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    obj.transform.position = new Vector3((float)node["x"], (float)node["y"], (float)node["z"]);
        //    float scale = (float)node["followers_count"] / maxFollower * 3;
        //    if (scale < 0.6f)
        //    {
        //        scale = 0.6f;
        //    }
        //    obj.transform.localScale = new Vector3(scale, scale, scale);
        //    obj.AddComponent<Rigidbody>();
        //    obj.GetComponent<Rigidbody>().useGravity = false;
        //    obj.GetComponent<Rigidbody>().Sleep();
        //    obj.name = node["tid"].ToString();

        //    finishRender = true;
        //}


        if (converted && !finishRender)
        {
            int count = 0;
            while (pivot < numberOfNodes)
            {
                string tid = keys[pivot];
                var node = json[tid];

                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3((float)node["x"], (float)node["y"], (float)node["z"]);
				if (node ["linked"].ToString () == "") {
					obj.transform.localScale = new Vector3 (10f, 10f, 10f);
				} else {
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
				}
                obj.name = tid;
				obj.tag = "cube";

//				switch ((int)node ["label"]) {
//				case 1:
//					obj.GetComponent<Renderer>().material.color = Color.red;
//					break;
//				case 2:
//					obj.GetComponent<Renderer>().material.color = Color.yellow;
//					break;
//				case 3:
//					obj.GetComponent<Renderer>().material.color = Color.blue;
//					break;
//				case 4:
//					obj.GetComponent<Renderer>().material.color = Color.black;
//					break;
//				case 5:
//					obj.GetComponent<Renderer>().material.color = Color.white;
//					break;
//				case 6:
//					obj.GetComponent<Renderer>().material.color = Color.gray;
//					break;
//				case 7:
//					obj.GetComponent<Renderer>().material.color = Color.green;
//					break;
//				case 8:
//					obj.GetComponent<Renderer>().material.color = Color.white;
//					break;
//				case 9:
//					obj.GetComponent<Renderer>().material.color = Color.red;
//					break;
//				case 10:
//					obj.GetComponent<Renderer>().material.color = Color.white;
//					break;
//				}

                //if ((int)node["label"] == 2)
                //{
                //    obj.GetComponent<Renderer>().material.color = Color.red;
                //}

                try
                {



                    if (node["linked"].ToString() != "")
                    {

						var parent = json[node["linked"].ToString()];

                        obj.AddComponent<LineRenderer>();
                        lineRenderer = obj.GetComponent<LineRenderer>();
                        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                        lineRenderer.startColor = Color.yellow;
                        lineRenderer.endColor = Color.red;
                        lineRenderer.startWidth = 0.2f;
                        lineRenderer.endWidth = 0.2f;
                        lineRenderer.SetPosition(0, new Vector3((float)node["x"], (float)node["y"], (float)node["z"]));


                        lineRenderer.SetPosition(1, new Vector3((float)parent["x"],
                            (float)parent["y"],
                            (float)parent["z"]));
                    }
                }
                catch (Exception e)
                {
					Destroy (obj.GetComponent<LineRenderer>());
                }


                nodes.Add(tid, obj);

                pivot++;
                count++;

                if (count >= 100)
                {
                    break;
                }
            }

            if (pivot > maxNumberOfShow)
            {
                finishRender = true;
                //VRCamera.transform.position = new Vector3((float)json[keys[0]]["x"] / 10, (float)json[keys[0]]["y"] / 10 + 100f, (float)json[keys[0]]["z"] / 10 - 0.35f);
            }

        }


        if (Input.GetKey(KeyCode.W))
        {
            VRCamera.transform.Translate(new Vector3(0, 0, 5 * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.A))
        {
            VRCamera.transform.Translate(new Vector3(-5 * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            VRCamera.transform.Translate(new Vector3(0, 0, -5 * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.D))
        {
            VRCamera.transform.Translate(new Vector3(5 * Time.deltaTime, 0, 0));
        }
    }

}