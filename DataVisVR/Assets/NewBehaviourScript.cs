using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using System;

public class NewBehaviourScript : MonoBehaviour
{

    private GameObject cAmera;
    Vector3 cameraPos;
	Vector3 r_palm_previous;
	Vector3 r_palm_current;


    bool isLeftFist = false;
    bool isRightFist = false;

    // Use this for initialization
    void Start()
    {
		cAmera = GameObject.Find("LMHeadMountedRig");
        cameraPos = cAmera.transform.position;
		r_palm_previous = r_palm_current = gameObject.transform.position;
    }

	// Update is called once per frame
    void Update()
    {
		r_palm_current = gameObject.transform.position;


		cameraPos.x += (r_palm_current.x - r_palm_previous.x) / Time.deltaTime * Time.deltaTime;
		cameraPos.y += (r_palm_current.y - r_palm_previous.y) / Time.deltaTime * Time.deltaTime;
		cameraPos.z += (r_palm_current.z - r_palm_previous.z) / Time.deltaTime * Time.deltaTime;
		cAmera.transform.position = cameraPos;

		r_palm_previous = r_palm_current;
    }

    bool checkIsFist(Hand left)
    {

        if (left.GrabStrength > 0.8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

