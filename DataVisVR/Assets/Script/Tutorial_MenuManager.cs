using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_MenuManager : MonoBehaviour {

	public void next() {
		this.GetComponent<Tutorial_Manager> ().nextState ();
	}

	public void reset() {
		GameObject.Find ("LMHeadMountedRig").transform.position = new Vector3(0,1,0);
	}
}
