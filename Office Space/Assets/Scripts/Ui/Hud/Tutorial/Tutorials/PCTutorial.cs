using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PCTutorial : MonoBehaviour 
{
	public Animator app, customisation, achivment, bank;
	public GameObject questionmark,charaterQuestionMark,pcTutorial2,canvas;

	DialogueManager manager;
	DialogueTrigger trigger;
	Dialogue dialogue;

	public Button appbtn,office,achivments,help,close;
	int counter = -1;
	bool disableSpace = true;

	private bool isInsideTrigger = false;
	bool appbtnP = false;

	public Animator DashBtn, orderBtn, inventoryBtn, OrdersBtn,upgradesbtn;

	public GameObject DashP, orderP, inventoryP, OrdersP,upgradesp , tutorial;

	public GameObject shopDoor,customisationt;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
		trigger = gameObject.GetComponent<DialogueTrigger>(); 
		app.GetComponent<Button>().onClick.AddListener(delegate {NextScreen();});
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				canvas.SetActive (true);
				questionmark.SetActive (false);
				trigger.TriggerDialogue ();
				disableSpace = false;

				appbtn.enabled = false;
				office.enabled = false;
				achivments.enabled = false;
				help.enabled = false;
				close.enabled = false;
			}
		}


		if (Input.GetKeyUp(KeyCode.Space) && disableSpace == false && manager.done == true)
			{
			if (appbtnP == false) 
			{
				
				counter++;
				Debug.Log ("Blooooop" + counter);
				NextDialogue (counter);
				manager.DisplayNextSentence ();
			} 
			else 
			{
				counter++;
				NextDialogue2 (counter);
				Debug.Log ("Blooooop" + counter);
				manager.DisplayNextSentence ();
			}
		}
	}

	public void NextDialogue(int i)
	{
		switch (i) 
		{
		case 1:
			{
				manager.charaA.PointSideUp ();
				customisation.SetBool ("BtnO", true);
				disableSpace = false;
				break;
			}
		case 2:
			{
				manager.charaA.PointSideUp ();
				customisation.SetBool ("BtnO", false);
				app.SetBool ("BtnO", true);
				break;
			}
		case 3:
			{
				manager.charaA.PointSide ();
				app.SetBool ("BtnO", false);
				achivment.SetBool ("BtnO", true);
				break;
			}
		case 4:
			{
				manager.charaA.PointSideUp ();
				achivment.SetBool ("BtnO", false);
				app.SetBool ("BtnO", true);
				appbtn.enabled = true;
				disableSpace = true;

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
        if (GameMaster.Instance.TutorialMode)
        {
            app.SetBool("BtnO", false);
            appbtnP = true;
            counter = 0;
            office.enabled = true;
            achivments.enabled = true;
            help.enabled = true;
            close.enabled = true;
            manager.DisplayNextSentence();
            manager.charaA.PointSideUp();
            DashBtn.SetBool("BtnO", true);

            disableSpace = false;
        }
	}
		
	public void NextDialogue2(int i)
	{
		switch (i) 
		{

		case 1:
			{
				manager.charaA.PointSideUp ();
				ShowPanel (orderP, DashP);
				DashBtn.SetBool ("BtnO", false);
				orderBtn.SetBool("BtnO", true);
				break;
			}
		case 2:
			{
				manager.charaA.PointSideUp ();
				ShowPanel (inventoryP, orderP);
				inventoryBtn.SetBool("BtnO", true);
				orderBtn.SetBool("BtnO", false);
				break;
			}
		case 3:
			{
				manager.charaA.PointSide ();
				ShowPanel (OrdersP, inventoryP);
				inventoryBtn.SetBool("BtnO", false);
				OrdersBtn.SetBool("BtnO", true);
				break;
			}
		case 4:
			{
				manager.charaA.PointSide ();
				ShowPanel (upgradesp,OrdersP);
				upgradesbtn.SetBool("BtnO", true);
				OrdersBtn.SetBool("BtnO", false);
				break;
			}
		case 5:
			{
				
				upgradesbtn.SetBool("BtnO", false);
				break;
			}
		case 6:
			{
                    tutorial.SetActive(false);
                    canvas.SetActive(false);
                    orderP.SetActive(false);
                    inventoryP.SetActive(false);
                    OrdersP.SetActive(false);
                    upgradesp.SetActive(false);
                    DashP.SetActive(true);

                    DashBtn.SetBool("BtnO", false);

                    GameMaster.Instance.TutorialMode = false;
                    GameMaster.Instance.CheckDifficulty();

                    this.enabled = false;
                break;
			}
		default:
			break;
		}
	}

	void ShowPanel(GameObject panel,GameObject prevPanel)
	{
		prevPanel.SetActive (false);

		panel.SetActive (true);

	}
	void ExitTutorial()
	{
		shopDoor.SetActive (true);
		customisationt.SetActive(true);

		pcTutorial2.SetActive (false);

	}
}
