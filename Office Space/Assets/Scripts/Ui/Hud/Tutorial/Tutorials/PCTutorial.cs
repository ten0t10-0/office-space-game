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
	int counter = 1;
	float canPress = 0;
	bool disableSpace = true;

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

			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			if (Input.GetKeyUp(KeyCode.Space) && Time.time > canPress && disableSpace == false)
			{
				NextDialogue (counter);
				canPress = Time.time + 2f;  
				counter++;
				Debug.Log (counter);
				//diable courser
				manager.DisplayNextSentence ();
			}

		}

	}

	public void NextDialogue(int i)
	{
		switch (i) 
		{
		case 0:
			{
				customisation.SetBool ("BtnO", true);
				disableSpace = false;
				break;
			}
		case 1:
			{
				customisation.SetBool ("BtnO", false);
				app.SetBool ("BtnO", true);
				break;
			}
		case 2:
			{
				app.SetBool ("BtnO", false);
				achivment.SetBool ("BtnO", true);
				break;
			}
		case 3:
			{
				achivment.SetBool ("BtnO", false);
				break;
			}
		case 4:
			{
				Cursor.visible = true;
				app.SetBool ("BtnO", true);
				break;
			}
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
		app.SetBool ("BtnO", false);
		pcTutorial2.SetActive (true);
		Cursor.visible = false;
	}
}
