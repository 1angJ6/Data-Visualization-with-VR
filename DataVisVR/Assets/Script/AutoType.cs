using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoType : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";

	// Use this for initialization
	void Start () {
//		StartCoroutine (ShowText());
	}

	public void autoPrint(string text) {

		StartCoroutine (ShowText(text));
	}


	IEnumerator ShowText(string text) {
		for (int i = 0; i < text.Length; i++) {
			currentText = text.Substring (0, i);
			this.GetComponent<Text> ().text = currentText;
			yield return new WaitForSeconds (delay);
		}
	}

	public string FullText
	{
		get { return fullText; }
		set { fullText = value; }
	}
}
