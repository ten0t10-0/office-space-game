using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Phone : MonoBehaviour 
{

	public GameObject phone;
	public GameObject tablet;

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			phone.SetActive (true);
		}
		if (Input.GetKeyDown(KeyCode.N)) 
		{
			phone.SetActive (false);
		}
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			tablet.SetActive (true);
		}
		if (Input.GetKeyDown(KeyCode.X)) 
		{
			tablet.SetActive (false);
		}
	}
}

