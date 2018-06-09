using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Phone : MonoBehaviour {

	public GameObject phone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.C)) 
		{
			OpenPhone ();
		}
	}

	public void OpenPhone()
	{
		phone.SetActive (true);
	}
}
