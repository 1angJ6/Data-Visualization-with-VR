using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineTest : MonoBehaviour {

    private LineRenderer lineRenderer;

    private Vector3 position;

    private int index = 0;

    private int lengthOfLineRenderer = 10;

    private Vector3[] ps = new Vector3[3]; 

	// Use this for initialization
	void Start () {

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.GetComponent<Renderer>().material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;

        ps[0] = new Vector3(0, 0, 0);
        ps[1] = new Vector3(0, 1, 0);
        ps[2] = new Vector3(0, 0, 1);

    }
	
	// Update is called once per frame
	void Update () {

        //if(Input.GetMouseButtonDown(0))
        //{
        //    position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
        //    lengthOfLineRenderer++;
        //    lineRenderer.SetVertexCount(lengthOfLineRenderer);
        //}

        //while (index < lengthOfLineRenderer)
        //{
        //    //两点确定一条直线，所以我们依次绘制点就可以形成线段了
        //    lineRenderer.SetPosition(index, position);
        //    index++;
        //}

        var points = new Vector3[10];
        var t = Time.time;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f);
        }
        lineRenderer.SetPositions(points);

    }

    //void OnGUI()
    //{
    //    GUILayout.Label("当前鼠标X轴位置：" + Input.mousePosition.x);
    //    GUILayout.Label("当前鼠标Y轴位置：" + Input.mousePosition.y);
    //}
}
