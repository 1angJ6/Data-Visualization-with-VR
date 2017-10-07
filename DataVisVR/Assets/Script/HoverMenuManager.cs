using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMenuManager : MonoBehaviour
{

    public void exit()
    {
        Debug.Log("Called..");
        Application.Quit();
    }

	public void changeSliderValue(float newValue)
	{
		HandController ht = gameObject.GetComponent<HandController> ();
		ht.Velocity = newValue;
	}

	public void reset()
	{
		HandController ht = gameObject.GetComponent<HandController> ();
		ht.resetCamera ();
	}
}
