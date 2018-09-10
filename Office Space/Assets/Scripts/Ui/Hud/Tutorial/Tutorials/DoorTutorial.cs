using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTutorial : MonoBehaviour 
{
	DialogueManager manager;
	DialogueTrigger trigger;
	public GameObject tutorial;

	void Start () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		trigger = gameObject.GetComponent<DialogueTrigger>(); 
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			//manager.DisplayNextSentence ();
			if (manager.count == 3) 
			{
				tutorial.SetActive (false);
			}
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			trigger.TriggerDialogue ();
		}
	}
}
