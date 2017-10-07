using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ButtonControl : MonoBehaviour {

	private Vector3 originalPos;
	private Vector3 under = new Vector3(0, -10, 0);
	private Tutorial_Manager manager;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
		manager = GameObject.Find ("Manager").GetComponent<Tutorial_Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (manager.IsTyping()) {
			transform.position = under;
		} else {
			transform.position = originalPos;
		}
	}
}
