using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateNavigationPos : MonoBehaviour {

	private GameObject root;

	// Use this for initialization
	void Start () {
		root = GameObject.Find ("LMHeadMountedRig");
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = root.transform.position / 2048;
	}
}
