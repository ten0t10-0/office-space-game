using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTrigger : MonoBehaviour {

	public GameObject mirror;
	private bool isInsideTrigger = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isInsideTrigger == true) 
		{
			mirror.SetActive (true);
		}
		if (isInsideTrigger == false) 
		{
			mirror.SetActive (false);
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;

		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = false;

		}
	}
}
