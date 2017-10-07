using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Leap;
using Leap.Unity;

public class GuestureTest : MonoBehaviour
{
    private GameObject VRCamera;
    private Vector3 cameraPos;
    private GameObject cube1;
    private GameObject cube2;

    private LeapProvider provider;
    private Vector3 lastHandPos;
    private Vector3 currHandPos;

    private const int FRONT_BACK = 1;
    private const int LEFT_RIGHT = 2;
    private const int UP_DOWN = 3;

    private float x, y, z;
    private float velocity = 1f;
	private float accuracy = 0.003f;
    private float count = 0;
    private bool isLeftFist = false;
    private bool isRightFist = false;

    // Use this for initialization
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;

        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                currHandPos = lastHandPos = hand.PalmPosition.ToVector3();
            }
        }

        VRCamera = GameObject.Find("LMHeadMountedRig");
        cameraPos = VRCamera.transform.position;

        cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.tag = "cube";
        cube1.transform.position = new Vector3(0.1f, 0f, 0.4f);
        cube1.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        cube1.GetComponent<Collider>().isTrigger = true;
        cube1.AddComponent<sleepTest>();
        cube1.name = "cube1";

        cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.tag = "cube";
        cube2.transform.position = new Vector3(-0.1f, 0f, 0.4f);
        cube2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        cube2.GetComponent<Collider>().isTrigger = true;
        cube2.AddComponent<sleepTest>();
        cube2.name = "cube2";

		for (int i = 0; i < 10000; i++) {
			GameObject a = GameObject.CreatePrimitive (PrimitiveType.Cube);
			a.transform.position = new Vector3(0f, 0f, 5 * i);
			a.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		}
    }

    // Update is called once per frame
    void Update()
    {
//		Debug.Log (lastHandPos);

        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                currHandPos = hand.PalmPosition.ToVector3();
                isRightFist = checkIsFist(hand);
            }
            if (hand.IsLeft)
            {
                isLeftFist = checkIsFist(hand);
            }
        }

		if (isLeftFist && !isRightFist && frame.Hands.Count == 2) {
			cameraPos.x += (currHandPos.x - lastHandPos.x) * velocity;
			cameraPos.y += (currHandPos.y - lastHandPos.y) * velocity;
			cameraPos.z += (currHandPos.z - lastHandPos.z) * velocity;
			VRCamera.transform.position = cameraPos;

		}

		lastHandPos = currHandPos;

    }

	public float Velocity
	{
		get { return velocity; }
		set { velocity = value; }
	}

    bool checkIsFist(Hand hand)
    {

        if (hand.GrabStrength > 0.9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int checkGestureType(float x, float y, float z)
    {
        if (x > y && x > z)
        {
			return LEFT_RIGHT;
        }

        if (y > x && y > z)
        {
			return UP_DOWN;
		}

        if (z > x && z > y)
        {
			return FRONT_BACK;
        }

        return 0;
    }
}
