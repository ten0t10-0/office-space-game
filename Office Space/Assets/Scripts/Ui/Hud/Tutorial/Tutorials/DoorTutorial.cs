using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTutorial : MonoBehaviour 
{
	DialogueManager manager;
	DialogueTrigger trigger;
	public GameObject tutorial,canvas;
	bool disableSpace = true;
	int counter = 0;

	void Start () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		trigger = gameObject.GetComponent<DialogueTrigger>(); 
	}

	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Space) && disableSpace == false && manager.done == true)
		{
			NextDialogue (counter);
			counter++;
			manager.DisplayNextSentence();
		}

	}
	public void NextDialogue(int i)
	{
		switch (i) 
		{
		case 1:
			{
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.ThirdPerson);
				tutorial.SetActive (false);
				GameMaster.Instance.PlayerControl = true;
				canvas.SetActive (false);
				break;
			}
		}
	}



	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			trigger.TriggerDialogue ();
			canvas.SetActive (true);
			disableSpace = false;
			manager.charaA.Greet ();
			Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.Static);
			GameMaster.Instance.PlayerControl = false;

		}
	}
}
