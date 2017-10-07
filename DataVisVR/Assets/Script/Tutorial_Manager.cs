using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial_Manager : MonoBehaviour {

	public enum State {
		welcome, control, rearrange, select, empty
	};

	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";
	private bool isTyping = false;
	private bool finishTyping = false;

	private Text textField;
	private State currentState;

	private GameObject cubeA;
	private GameObject cubeB;

	// Use this for initialization
	void Start () {
		textField = GameObject.Find ("InstructionText").GetComponent<Text> ();
		currentState = State.welcome;
	}
	
	// Update is called once per frame
	void Update () {

		switch (currentState) {
		case State.welcome:
			welcomeText ();
			break;
		case State.control:
			controlText ();
			break;
		case State.rearrange:
			rearrangeText ();
			break;
		case State.select:
			selectText ();
			break;
		default:
			break;
		}
	}

	private void welcomeText() {
		if (!finishTyping) {
			string text = "Hello, welcome to the virtual reality data visualization system. " +
			"This system applies new features of VR to extend the traditional 2D data visualization method, " +
			"which will make the visualization more vivid and interactive.\n" +
			"So, show me your hands.";
			StartCoroutine (ShowText (text));
		}
	}

	private void controlText() {
		if (!finishTyping) {
			string text = "This section will introduce the method of navigation around the space. " +
			              "First make sure that both you hand should within in you range of vision. " +
				"Then turn your hand over to make your palm facing you, you can see the menu. The reset button helps you to back to original position." +
				"When navigation in the space, you can image you are grabbing the space with your left hand, try it!";
			StartCoroutine (ShowText (text));
		}
	}

	private void rearrangeText() {
		if (!finishTyping) {
			string text = "The system has collected about 210,000 tweets from twitter. All tweets are mapped to the 3D space with some algorithm. " +
				"Each cube can be seen as one tweet and one cluster of tweets are tweets and retweeted tweets. When you first touch the cube with your right hand, " +
				"these tweets from one cluster will be rearranged according to the time they are created. Try it!";
			StartCoroutine (ShowText (text));
		}
	}

	private void selectText() {
		if (!finishTyping) {
			string text = "After all rearrangement, you can pick one of them to read the detail of tweet (Disable in current stage) when you close to it. Try it!";
			StartCoroutine (ShowText (text));
		}
	}

	IEnumerator ShowText(string text) {
		isTyping = true;
		finishTyping = true;
		for (int i = 0; i < text.Length; i++) {
			currentText = text.Substring (0, i);
			textField.text = currentText;
			yield return new WaitForSeconds (delay);
		}

		if (currentState == State.control) {
			GameObject.Find ("RigidRoundHand_L").transform.GetChild (5).GetComponent<HandController> ().enabled = true;
			GameObject.Find ("RigidRoundHand_L").transform.GetChild (5).GetComponent<MyHover> ().enabled = true;
		}

		isTyping = false;
		currentText = "";
	}

	public bool IsTyping() {
		return isTyping;
	}

	public bool isSelectionState() {
		return currentState == State.select;
	}

	public bool isRearrangeState() {
		return currentState == State.rearrange;
	}

	public void nextState() {
		switch (currentState) {
		case State.welcome:
			currentState = State.control;
			finishTyping = false;
			break;
		case State.control:
			currentState = State.rearrange;
			finishTyping = false;
			createSphere ();
			break;
		case State.rearrange:
			currentState = State.select;
			finishTyping = false;
			break;
		case State.select:

			SceneManager.LoadSceneAsync ("MainScene");

			currentState = State.empty;
			finishTyping = false;
			break;
		default:
			break;
		}
	}

	private void createSphere() {
		cubeA = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		cubeA.transform.position = new Vector3 (0.3f, 1f, 0.5f);
		cubeA.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		cubeA.name = "sphereA";
		cubeA.GetComponent<MeshRenderer> ().receiveShadows = false;

		cubeB = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		cubeB.transform.position = new Vector3 (1f, 1.5f, 1.5f);
		cubeB.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		cubeB.name = "sphereB";
		cubeB.GetComponent<MeshRenderer> ().receiveShadows = false;
	}

	public void rearrange() {
		cubeA.GetComponent<Renderer> ().material.color = Color.red;
		cubeB.GetComponent<Renderer> ().material.color = Color.red;

		cubeA.AddComponent<LineRenderer> ();
		var lineRenderer = cubeA.GetComponent<LineRenderer> ();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.startColor = Color.yellow;
		lineRenderer.endColor = Color.red;
		lineRenderer.startWidth = 0.02f;
		lineRenderer.endWidth = 0.02f;
		lineRenderer.SetPosition(0, cubeB.transform.position);
		lineRenderer.SetPosition(1, cubeA.transform.position);
	}

}
