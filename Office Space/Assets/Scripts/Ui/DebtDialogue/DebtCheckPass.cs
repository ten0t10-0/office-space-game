﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebtCheckPass : MonoBehaviour 
{
	int counter = 0;
	bool disableSpace = true;

	DialogueManager manager;
	Dialogue dialogue;
	DialogueTrigger trigger;
	public GameObject canvas,tutorial,invoice;
	UIController controller;

	// Use this for initialization
	void Awake () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		dialogue = gameObject.GetComponent<DialogueTrigger>().dialogue; 
		trigger = gameObject.GetComponent<DialogueTrigger>(); 
		controller = FindObjectOfType<UIController> ();
	}

	public void StartUp()
	{
		canvas.SetActive (true);
		trigger.TriggerDialogue ();
		disableSpace = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Space) && disableSpace == false && manager.done == true)
		{
			if (counter == 1) 
			{
				invoice.SetActive (false);
				controller.NewDayAfterDebt ();
				tutorial.SetActive (false);
				canvas.SetActive (false);
			}
			counter++;
			manager.DisplayNextSentence();
		}
	}
		
}
