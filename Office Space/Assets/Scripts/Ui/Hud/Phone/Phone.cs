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

		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			tablet.SetActive (true);

            GameMaster.Instance.BuildMode = false;
		}
		if (Input.GetKeyDown(KeyCode.X)) 
		{
			tablet.SetActive (false);

            GameMaster.Instance.BuildMode = false;
        }
	}
}

