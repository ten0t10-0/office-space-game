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
	int counter = 0;
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
				disableSpace = false;

			}
		}


		if (Input.GetKeyUp(KeyCode.Space) && disableSpace == false && manager.done == true)
			{
				NextDialogue (counter);
				counter++;
				Debug.Log ("Blooooop"+counter);
				//Cursor.lockState = CursorLockMode.Locked;
				//manager.DisplayNextSentence ();
				manager.DisplayNextSentence();
			}



	}

	public void NextDialogue(int i)
	{
		switch (i) 
		{
		case 1:
			{
				Debug.Log ("Bloooooprunning????"+counter);
				customisation.SetBool ("BtnO", true);
				disableSpace = false;
				break;
			}
		case 2:
			{
				Debug.Log ("Bloooooprunning????"+counter);
				customisation.SetBool ("BtnO", false);
				app.SetBool ("BtnO", true);
				break;
			}
		case 3:
			{
				app.SetBool ("BtnO", false);
				achivment.SetBool ("BtnO", true);
				break;
			}
		case 4:
			{
				achivment.SetBool ("BtnO", false);
				break;
			}
		case 5:
			{
				//Cursor.lockState = CursorLockMode.None;
				app.SetBool ("BtnO", true);
				break;
			}
		default:
			break;
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
