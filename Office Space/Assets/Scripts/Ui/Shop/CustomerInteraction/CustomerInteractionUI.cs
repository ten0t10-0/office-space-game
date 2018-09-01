using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerInteractionUI : MonoBehaviour 
{

	public Animator customer,player,bars,item,speech,perc,button;
	public GameObject InteractionPanel,uiCharacter, playerLoc,customerLoc,percentagePanel,hudCanvas, mount,buttonpanel;

	public Button btnDecrease,btnIncrease;

	private GameObject playerGuy,customerGuy;

	CharacterCustomizationData Char;
	CharacterCustomizationScript playerCus;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	bool disableSpace = true;

	public TextMeshProUGUI text,name,itemName; 

	int counter = 1,currentAmount ;
	float canPress = 0;
	int max = 100;
	int min = 1;
	int increasePerClick = 1;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				InteractionPanel.SetActive (true);
				OpenPanel.SetActive (false);
				hudCanvas.SetActive (false);
				GameMaster.Instance.PlayerControl = false;
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().isKinematic = true;
				GameMaster.Instance.CurrentPlayerObject.transform.parent = mount.transform;
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().MovePosition (mount.transform.position); 
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().MoveRotation (mount.transform.rotation);
				startInteraction ();
				StartCoroutine (startO ());
			}
		}
		if (Input.GetKeyUp(KeyCode.Space) && Time.time > canPress && disableSpace == false)
		{
			runInteraction (counter);
			canPress = Time.time + 1.5f;  
			counter++;
			Debug.Log (counter);
		}
	}

	public void startInteraction()
	{
		playerGuy = Instantiate (uiCharacter, playerLoc.transform);
		customerGuy = Instantiate (uiCharacter, customerLoc.transform);
		//playerGuy.GetComponent<CharacterCustomizationScript> ().SetAppearanceByData (GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ().GetCustomizationData ());
		//customer apperance
	}
	public void runInteraction(int i)
	{
		switch (i)
		{
		case 0:
			{
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.Static);
				bars.SetBool ("BarIn", true);
				player.SetBool ("PlayerIn", true);
				name.SetText(GameMaster.Instance.Player.Name);
				//array of random greetings
				text.SetText("Good day bastard");
				speech.SetBool ("SpeechIn", true);
				disableSpace = false;
				break;
			}
		case 1:
			{
				customer.SetBool ("CustomerIn", true);
				//araay of comebacks
				text.SetText("Working hard retard? ill take this ples");
				name.SetText("Bryawando");
				itemName.SetText("Name of item");
				item.SetBool("ItemIn",true);
				break;
			}
		case 2:
			{
				speech.SetBool ("SpeechIn", false);
				percentagePanel.SetActive (true);
				perc.SetBool ("PerIn", true);
				break;
			}
		case 3:
			{
				buttonpanel.SetActive (true);
				button.SetBool ("ButtonIn", true);
				Debug.Log ("do i run?");

				break;
			}
		case 4:
			{
				item.SetBool("ItemIn",false);
				text.SetText("That will do");
				name.SetText("Bryawando");
//				button.SetBool ("ButtonIn", false);
				buttonpanel.SetActive (false);
				speech.SetBool ("SpeechIn", true);
				perc.SetBool ("PerIn", false);
				percentagePanel.SetActive (false);
				break;
			}
		case 5:
			{
				customer.SetBool ("CustomerIn", false);
				text.SetText("yay!");
				name.SetText(GameMaster.Instance.Player.Name);
				// dont forget set stuff false, change animator stuf that dont need to go away disableSpace = true;
				break;
			}
		case 6:
			{
				player.SetBool ("PlayerIn", false);
				speech.SetBool ("SpeechIn", false);
				bars.SetBool ("BarIn", false);
				// dont forget set stuff false, change animator stuf that dont need to go away disableSpace = true;
				break;
			}
		default:
			break;
		}
	}
	public void confirmButton()
	{
		runInteraction (4);
		counter = 5;

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = false;
			OpenPanel.SetActive(false);
		}
	}

	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}
	IEnumerator startO()
	{
		yield return new WaitForSeconds(3);
		runInteraction (0);
	}
	public void ChangeAmount(bool increase)
	{
		// clamp current value between min-max
		currentAmount = Mathf.Clamp(currentAmount + (increase ? increasePerClick : -increasePerClick), min, max);


		// disable buttons i
		btnDecrease.interactable = currentAmount > min;
		btnIncrease.interactable = currentAmount < max;
	}
}
