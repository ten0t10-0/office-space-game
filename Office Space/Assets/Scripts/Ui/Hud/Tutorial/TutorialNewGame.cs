using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNewGame : MonoBehaviour 
{

	DialogueManager manager;
	Dialogue dialogue;


	void Start () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		dialogue = gameObject.GetComponent<DialogueTrigger>().dialogue; 
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			manager.DisplayNextSentence ();
		}

	}
}
