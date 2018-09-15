using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorScript : MonoBehaviour {

	private Animator animator;

	private bool isInsideTrigger = false;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			isInsideTrigger = true;
			FindObjectOfType<SoundManager>().Play("SlidingDoor");
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "NPC")
		{
			isInsideTrigger = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "NPC")
		{
			isInsideTrigger = false;
			animator.SetBool("SOpen", false);
			FindObjectOfType<SoundManager>().Play("SlidingDoor");
		}
	}
		

	// Update is called once per frame
	void Update () 
	{

		if(isInsideTrigger)
		{
			animator.SetBool("SOpen", true); 

		}
	}
}

