using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerInteractionUI : MonoBehaviour 
{

	public Animator customer,player,bars,item,speech,perc,button;
	public GameObject InteractionPanel,uiCharacter, playerLoc,customerLoc,percentagePanel,hudCanvas, mount,buttonpanel,cube,particle;

	public Button btnDecrease,btnIncrease;

	private GameObject playerGuy,customerGuy;
	public Animator hudO;

	CharacterCustomizationData Char;
	CharacterCustomizationScript playerCus;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	bool disableSpace = true;

	public TextMeshProUGUI text,name,itemName; 
	public TextMeshProUGUI itemCost,calPercentage; 
	Item customerItem;

	int counter = 1;
	float currentAmount = 100,percentage,profit ;
	float canPress = 0;
	float max = 10000000,min = 0;
	float increasePerClick = 10;

	string[] greet = new string[] {"Hello there!","Welcome","Good Day","Welcome, Im here to help","How can I help you?"};
	string[] buyGreeting = new string[]{"I want this","I'll take this","How much is this?","I finally found this"};
	string[] customerHappy = new string[]{"Thats Perfect","I'll take that","Thats fine","That will do","Not bad"};
	string[] customerSad = new string[]{"You are crazy","Im not paying that!","Think I'll try else where"};
	string[] playerHappy = new string[]{"Score!","Awesome","Alright","Thank you","Come again"};
	string[] playerSad = new string[]{"Awww","I made them mad","Better luck next time"};
	string[] tooHigh = new string[]{"I can't pay that much","Could you lower it?","Maybe a bit lower?"};

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
				
				//InteractionPanel.SetActive (true);
				OpenPanel.SetActive (false);
				hudCanvas.SetActive (false);
				particle.SetActive (false);
				cube.SetActive (true);
				mount.GetComponent<Collider>().enabled = false;
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.FirstPerson);
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

	public void startInteraction(CharacterCustomizationScript customer)
	{
		playerGuy = Instantiate (uiCharacter, playerLoc.transform);
		customerGuy = Instantiate (uiCharacter, customerLoc.transform);

		InteractionPanel.SetActive (true);
		hudO.SetBool ("UIO", true);
		Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.Static);

		customerItem = RandomItem ();

		playerGuy.GetComponent<CharacterCustomizationScript>().SetAppearanceByData (customer.GetCustomizationData ());
		customerGuy.GetComponent<CharacterCustomizationScript> ().SetAppearanceByData (GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ().GetCustomizationData ());
	

		runInteraction (0);

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
				text.SetText(greet[Random.Range(0,greet.Length-1)]);
				speech.SetBool ("SpeechIn", true);
				disableSpace = false;
				break;
			}
		case 1:
			{
				customer.SetBool ("CustomerIn", true);
				text.SetText(buyGreeting[Random.Range(0,buyGreeting.Length-1)]);
				name.SetText("Bryawando");
				itemName.SetText(customerItem.Name.ToString());
				currentAmount = (customerItem.UnitCost *.5f) + customerItem.UnitCost;
				itemCost.SetText (currentAmount.ToString ());
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
				counter = 0;
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.FirstPerson);
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
	public void ChangeAmount(bool increase)
	{
		// clamp current value between min-max
		currentAmount = Mathf.Clamp(currentAmount + (increase ? increasePerClick : -increasePerClick), min, max);
		itemCost.SetText (currentAmount.ToString ());
		profit = currentAmount - customerItem.UnitCost;
		percentage = profit / customerItem.UnitCost;
		calPercentage.SetText ((percentage * 100).ToString ("f0")+"%");

		// disable buttons i
		btnDecrease.interactable = currentAmount > min;
		btnIncrease.interactable = currentAmount < max;
	}
	 Item RandomItem()
	{
		List<Item> tempItems = new List<Item> ();

		foreach (Item item in GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay) 
		{
			if (item != null) 
			{
				tempItems.Add (item);
			}
		}
		return tempItems [Random.Range (0, tempItems.Count)];
	}

	ItemSubcategorySO RandomSubCat()
	{
		return GameMaster.Instance.ItemManager.Database.Subcategories [Random.Range (0, GameMaster.Instance.ItemManager.Database.Subcategories.Count)];
	}

		
}
