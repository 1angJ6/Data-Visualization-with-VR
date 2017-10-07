using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;


public class HandController : MonoBehaviour {

	private GameObject VRCamera;
	private Vector3 cameraPos;

	private LeapProvider provider;
	private Frame frame;
	private Vector3 lastHandPos;
	private Vector3 currHandPos;

	private const int FRONT_BACK = 1;
	private const int LEFT_RIGHT = 2;
	private const int UP_DOWN = 3;

	private float x, y, z;
	private float velocity = 0.1f;
	private float scale = 500f;
	private float accuracy = 0.003f;
	private float count = 0;
	private bool isLeftFist = false;
	private bool isRightFist = false;
	private bool isReSet = false;
	private float timer = 0;

	// Use this for initialization
	void Start()
	{
		provider = FindObjectOfType<LeapProvider>() as LeapProvider;

		VRCamera = GameObject.Find("LMHeadMountedRig");
		cameraPos = VRCamera.transform.position;

		lastHandPos = currHandPos = transform.localPosition;

	}

	// Update is called once per frame
	void Update()
	{
		navigation ();

		checkBoundary();

		if (isLeftFist) {
			if (timer < 2f) {
				timer += Time.deltaTime;
			}
		} else {
			timer = 0;
		}

	}

	private void navigation()
	{
		currHandPos = transform.localPosition;

		if (isReSet) {
			lastHandPos = currHandPos = transform.position;
			isReSet = false;
			VRCamera.transform.position = new Vector3 (100f,100f,100f);
			return;
		}

		frame = provider.CurrentFrame;
		foreach (Hand hand in frame.Hands)
		{
			if (hand.IsLeft)
			{
				isLeftFist = hand.PinchStrength * 1000 > 980 ? true : false;
			}
		}

		if (isLeftFist && frame.Hands.Count == 1) {
			Vector3 offset = Vector3.zero;
			offset.x = currHandPos.x - lastHandPos.x;
			offset.y = -currHandPos.z + lastHandPos.z;
			offset.z = -currHandPos.y + lastHandPos.y;
			cameraPos += offset * velocity *  scale * Mathf.Pow (0.1f, 2f - timer);
			VRCamera.transform.position = cameraPos;

		}
		lastHandPos = currHandPos;
	}

	private void checkBoundary()
	{

		cameraPos = VRCamera.transform.position;

		if (cameraPos.x > 2048) {
			cameraPos.x = 2048;
		}
		if (cameraPos.x < -2048) {
			cameraPos.x = -2048;
		}

		if (cameraPos.y > 2048) {
			cameraPos.y = 2048;
		}
		if (cameraPos.y < -2048) {
			cameraPos.y = -2048;
		}

		if (cameraPos.z > 2048) {
			cameraPos.z = 2048;
		}
		if (cameraPos.z < -2048) {
			cameraPos.z = -2048;
		}

		VRCamera.transform.position = cameraPos;

	}

	public float Velocity
	{
		get { return velocity; }
		set { velocity = value; }
	}

	public void resetCamera()
	{
		isReSet = true;
	}

	private void reset()
	{
		Frame frame = provider.CurrentFrame;
		foreach (Hand hand in frame.Hands)
		{
			if (hand.IsRight)
			{
				currHandPos = lastHandPos = hand.PalmPosition.ToVector3();
			}
		}
	}
}
