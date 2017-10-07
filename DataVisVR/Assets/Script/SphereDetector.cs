using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : MonoBehaviour {

	GameObject HandModel;
	GameObject LPalm;
	HandController handController;
	bool isBind = false;

	// Use this for initialization
	void Start () {
		HandModel = GameObject.Find ("HandModel");
//		LPalm = HandModel.transform.Find()
//		LPalm = GameObject.FindGameObjectWithTag ("LPalm");
		//handController = LPalm.GetComponent<HandController> ();
		handController = HandModel.transform.FindChild("RigidRoundHand_L/palm").GetComponent<HandController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "sphere") {
			handController.Velocity = 0;
		}
	}
}
