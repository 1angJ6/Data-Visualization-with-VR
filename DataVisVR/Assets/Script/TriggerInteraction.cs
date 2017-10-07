using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class TriggerInteraction : MonoBehaviour {

	private LeapProvider provider;
	private Frame frame;
	private bool rightFist = false;
	private Vector3 indexPos;

	// Use this for initialization
	void Start () {
		provider = FindObjectOfType<LeapProvider>() as LeapProvider;
		indexPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		frame = provider.CurrentFrame;
		foreach (Hand hand in frame.Hands)
		{
			if (hand.IsRight)
			{
				rightFist = hand.GrabStrength > 0.9 ? true : false;
			}

		}

	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "cube") {

		}
	}
}
