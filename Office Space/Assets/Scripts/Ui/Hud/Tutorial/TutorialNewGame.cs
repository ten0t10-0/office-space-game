using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNewGame : MonoBehaviour 
{

	DialogueManager manager;
	DialogueTrigger trigger;
	Dialogue dialogue;
	public GameObject pctrigger;
	public GameObject canvas;
	public GameObject NewOrder;
	public Button appbtn;

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
			Debug.Log ("Bloooop" + counter.ToString ());
			NextDialogue (counter);
			counter++;
			manager.DisplayNextSentence();

		}

		if (Input.GetKeyUp (KeyCode.P))
			startTrigger ();

	}

	public void startTrigger()
	{
		canvas.SetActive (true);
		trigger.TriggerDialogue ();
		disableSpace = false;
		manager.charaA.Greet();
	}

	public void NextDialogue(int i)
	{
		switch (i)
		{
		case 3:
			{
				NewOrder.SetActive (true);
				break;
			}
		case 8:
			{
                    //GameMaster.Instance.NewGame_PostTutorial();
					pctrigger.SetActive(true);
                    gameObject.SetActive(false);
				break;
			}
		}
	}
}
