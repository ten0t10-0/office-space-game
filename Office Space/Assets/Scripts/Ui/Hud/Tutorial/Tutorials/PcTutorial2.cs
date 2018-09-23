using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcTutorial2 : MonoBehaviour 
{

	public Animator DashBtn, orderBtn, inventoryBtn, OrdersBtn;

	public GameObject DashP, orderP, inventoryP, OrdersP;

	public GameObject shopDoor,pcTutorial2,customisation;
	int counter = 0;
	bool disableSpace = true;

	DialogueManager manager;
	Dialogue dialogue;
	DialogueTrigger trigger;

	// Use this for initialization
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
			Debug.Log ("Blooooop"+counter);
			Cursor.lockState = CursorLockMode.Locked;

			manager.DisplayNextSentence();
		}
	}

	public void NextDialogue(int i)
	{
		switch (i) 
		{
		case 1:
			{
				manager.charaA.PointSideUp ();
				DashBtn.enabled = true;
				break;
			}
		case 2:
			{
				ShowPanel (orderP, DashP,orderBtn,DashBtn);
				break;
			}
		case 3:
			{
				ShowPanel (inventoryP, orderP, inventoryBtn, orderBtn);
				break;
			}
		case 4:
			{
				ShowPanel (OrdersP, inventoryP, orderBtn, inventoryBtn);
				break;
			}
		case 5:
			{
				break;
			}
		default:
			break;
		}
	}

	void ShowPanel(GameObject panel,GameObject prevPanel,Animator button,Animator prevButton)
	{
		prevPanel.SetActive (false);
		prevButton.enabled = false;
		panel.SetActive (true);
		button.enabled = true;
	}
	void ExitTutorial()
	{
		shopDoor.SetActive (true);
		customisation.SetActive(true);

		pcTutorial2.SetActive (false);

	}
}
