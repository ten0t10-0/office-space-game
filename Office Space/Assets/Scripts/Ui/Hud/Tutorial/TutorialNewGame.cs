using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNewGame : MonoBehaviour 
{

	DialogueManager manager;
	DialogueTrigger trigger;
	Dialogue dialogue;
	public GameObject canvas;

	int counter = 0;
	bool disableSpace = true;

	private bool isInsideTrigger = false;


	void Start () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		dialogue = gameObject.GetComponent<DialogueTrigger>().dialogue;
        trigger = gameObject.GetComponent<DialogueTrigger>();
    }
	
	// Update is called once per frame
	void Update () 
	{

			



		if (Input.GetKeyUp(KeyCode.Space) && disableSpace == false && manager.done == true)
		{
			NextDialogue (counter);
			counter++;
		
			manager.DisplayNextSentence();
		}

	}

	public void startTrigger()
	{
		canvas.SetActive (true);
		trigger.TriggerDialogue ();
		disableSpace = false;
	}

	public void NextDialogue(int i)
	{
		switch (i)
		{
		case 1:
			{

				break;
			}
		case 5:
			{
                    //GameMaster.Instance.NewGame_PostTutorial();
                    gameObject.SetActive(false);
				break;
			}
		}
	}
}
