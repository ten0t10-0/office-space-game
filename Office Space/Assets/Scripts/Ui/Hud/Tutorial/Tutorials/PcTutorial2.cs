using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcTutorial2 : MonoBehaviour 
{

	public Animator DashBtn, orderBtn, inventoryBtn, OrdersBtn;

	public GameObject DashP, orderP, inventoryP, OrdersP;

	public GameObject shopDoor,pcTutorial2,customisation;
	int counter = 0;

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
		if (Input.GetKeyDown (KeyCode.Space)) {
			
			if (manager.count == 0) {
				DashBtn.enabled = true;
			}
			if (manager.count == 1) {
				ShowPanel (orderP, DashP,orderBtn,DashBtn);
			}
			if (manager.count == 2) {
				ShowPanel (inventoryP, orderP, inventoryBtn, orderBtn);
			}
			if (manager.count == 3) {
				ShowPanel (OrdersP, inventoryP, orderBtn, inventoryBtn);
				Cursor.visible = true;
			}
			manager.DisplayNextSentence ();

			counter++;
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
