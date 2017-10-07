using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Tutorial_TriggerDetector : MonoBehaviour {

	private Tutorial_Manager manager;
	private LeapProvider provider;
	private Frame frame;
	private bool rightPinch;
	private GameObject selected;
	private Vector3 originalPos;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Manager").GetComponent<Tutorial_Manager> ();
		provider = FindObjectOfType<LeapProvider>() as LeapProvider;
	}
	
	// Update is called once per frame
	void Update () {
		frame = provider.CurrentFrame;
		foreach (Hand hand in frame.Hands)
		{
			if (hand.IsRight)
			{
				rightPinch = hand.PinchStrength > 0.9 ? true : false;
			}
		}

		if (rightPinch && selected != null) {
			selected.GetComponent<Renderer> ().material.color = new Color (255,128,0);
			selected.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			selected.transform.position = transform.position;
			selected.transform.rotation = transform.rotation;
		}

		if (!rightPinch && selected != null) {
			selected.transform.position = originalPos;
			selected = null;

		}

	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.name == "sphereA" || other.gameObject.name == "sphereB") {
			if(manager.isRearrangeState())
				manager.rearrange ();
		}

	}

	void OnTriggerStay(Collider other) {

		if (manager != null) {
			if (manager.isSelectionState()) {
				if (rightPinch) {
					return;
				}

				selected = other.gameObject;
				originalPos = other.gameObject.transform.position;

			}
		}

	}
}
