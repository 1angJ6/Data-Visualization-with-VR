using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHover : MonoBehaviour
{

    private float rotationY;
	private bool isActive = false;
    GameObject leapEventSystem;
    GameObject canvas;
	Vector3 front = new Vector3(-0.25f, -0.05f, 0.35f);
	Vector3 back = new Vector3(0, 0, -0.3f);

    // Use this for initialization
    void Start()
    {
        rotationY = gameObject.transform.rotation.y;
        leapEventSystem = GameObject.Find("LeapEventSystem");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
		if (rotationY < -0.1f && rotationY > -0.5f) {
			if (!isActive) {
				canvas.transform.localPosition = front;
				isActive = true;
			}
		}
		else
		{
			if (isActive)
			{
				canvas.transform.localPosition = back;
				isActive = false;
			}
		}

        rotationY = gameObject.transform.rotation.y;
    }
}
