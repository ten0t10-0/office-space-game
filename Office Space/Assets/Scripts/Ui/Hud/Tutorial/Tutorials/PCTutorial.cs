using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PCTutorial : MonoBehaviour 
{
	public Animator app, customisation, achivment, bank;
	public GameObject questionmark,charaterQuestionMark,pcTutorial2;

	DialogueManager manager;
	DialogueTrigger trigger;

	public Button appbtn;


	private bool isInsideTrigger = false;


	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		trigger = gameObject.GetComponent<DialogueTrigger>(); 

		appbtn.GetComponent<Button>().onClick.AddListener(delegate {NextScreen();});
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				questionmark.SetActive (false);
				trigger.TriggerDialogue ();
				Cursor.visible = false;
			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) 
		{

			if (manager.count == 0) 
			{
				customisation.enabled = true;
			}
			if (manager.count == 1) 
			{
				customisation.enabled = false;
				app.enabled = true;
			}
			if (manager.count == 2) 
			{
				app.enabled = false;
				achivment.enabled = true;
			}
			if (manager.count == 3) 
			{
				achivment.enabled = false;
			}
			if (manager.count == 4) 
			{
				Cursor.visible = true;
				app.enabled = true;
			}

			manager.DisplayNextSentence ();

		}

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
		}
	}

	void NextScreen()
	{
		app.enabled = false;
		pcTutorial2.SetActive (true);
		Cursor.visible = false;
	}
}
