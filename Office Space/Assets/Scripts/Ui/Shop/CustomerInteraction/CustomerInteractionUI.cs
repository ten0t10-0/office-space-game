using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerInteractionUI : MonoBehaviour 
{

	public Animator customer,player,bars,item,speech,perc,button,inventory;
	public GameObject InteractionPanel,uiCharacter, playerLoc,customerLoc,percentagePanel,hudCanvas, mount,buttonpanel,cube,particle,inventoryP,confirm;

	public Button btnDecrease,btnIncrease;

	private GameObject playerGuy,customerGuy;
	public Animator hudO;

	CharacterCustomizationData Char;
	CharacterCustomizationScript playerCus;
	ServeCustomer serve;
	ShopManagerClass shopMan;
	AiSpawnManager spawner;

	public Image border, pic;
	public Sprite bronze, silver, gold;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	bool disableSpace = true, markUpFail = false,subCate = false,saleSuccessful = false,subNothing = false,endOfDay = false; //End of day True set up!!!

	public TextMeshProUGUI text,name,itemName,labelCat; 
	public TextMeshProUGUI itemCost,calPercentage; 
	Item customerItem;
	ItemSubcategorySO customerSub;

	int counter = 1,failCounter = 0, subFailCounter = 0,AInum =0;
	int inventoryType = 0;
	float currentAmount = 100,percentage,profit ;
	float canPress = 0;
	float max = 10000000,min = 0;
	float increasePerClick = 100;

	[HideInInspector]
	public int customerServed = 0,customerFailed = 0; 

	string[] greet = new string[] {"Hello there!","Welcome","Good Day","Welcome, Im here to help","How can I help you?"};
	string[] buyGreeting = new string[]{"I want this","I'll take this","How much is this?","I finally found this"};
	string[] customerHappy = new string[]{"Thats Perfect","I'll take that","Thats fine","That will do","Not bad"};
	string[] customerSubcate = new string[]{"I'm looking for some ","Do you have ","I want any "};
	string[] customerSubSad = new string[]{"Think I'll try else where ","So you do not have any? ","Thanks for nothing "};
	string[] customerSad = new string[]{"You are crazy","Im not paying that!","Think I'll try else where"};
	string[] customerFind = new string[]{"Thats not it","I dont want that","Not what I'm looking for"};
	string[] playerHappy = new string[]{"Score!","Awesome","Alright","Thank you","Come again"};
	string[] playerSad = new string[]{"Awww","I made them mad","Better luck next time"};
	string[] tooHigh = new string[]{"I can't pay that much","Could you lower it?","Maybe a bit lower?"};

	string customerResponce = "";
	string playerResponce = "";
	string customerSubCat = "";

	// Use this for initialization
	void Awake () 
	{
		serve = FindObjectOfType<ServeCustomer> ();
		shopMan = FindObjectOfType<ShopManagerClass> ();
		spawner = FindObjectOfType<AiSpawnManager> ();
	}
	void Start()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				if (GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay == null) 
				{
					Debug.Log ("No Items on display");
				} 
				else 
				{
					OpenPanel.SetActive (false);
					confirm.SetActive (true);
					Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.Static);
					Cursor.lockState = CursorLockMode.None;
					GameMaster.Instance.PlayerControl = false;
				}
			}

			if (GameMaster.Instance.DayEnd == true) 
			{
				endOfDay = true;
			}
		}
		if (Input.GetKeyUp (KeyCode.Space) && Time.time > canPress && disableSpace == false) 
		{
			if (GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay == null) 
			{
				subCate = true;
			}

			if (subCate == false) 
			{
				if (markUpFail == true) 
				{
					runInteraction (2);
					counter = 3;
					markUpFail = false;
				}
				runInteraction (counter);
				canPress = Time.time + 1.5f;  
				counter++;
				Debug.Log (counter);
			} 
			else 
			{
				runSubInteraction (counter);
				canPress = Time.time + 1.5f;  
				counter++;
				Debug.Log (counter);
			}
		}

	}

	public void OpenShop()
	{
		spawner.SpawnAI1 ();
		Invoke ("spawnAI2", Random.Range (3f, 5f));
		Invoke ("spawnAI3", Random.Range (4f, 7f));
		//InteractionPanel.SetActive (true);
		Cursor.lockState = CursorLockMode.Locked;
		hudCanvas.SetActive (false);
		particle.SetActive (false);
		cube.SetActive (true);
		mount.GetComponent<Collider>().enabled = false;
		Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.FirstPerson);
		GameMaster.Instance.PlayerControl = true;
        GameMaster.Instance.SleepMode = false;
	}
	public void Cancel()
	{
		GameMaster.Instance.PlayerControl = true;
		Cursor.lockState = CursorLockMode.Locked;
		Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.ThirdPerson);
	}

	public void startInteraction(CharacterCustomizationScript customer,int ai)
	{
		playerGuy = Instantiate (uiCharacter, playerLoc.transform);
		customerGuy = Instantiate (uiCharacter, customerLoc.transform);
		AInum = ai;
		InteractionPanel.SetActive (true);
		hudO.SetBool ("UIO", true);
		Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.Static);

		customerItem = RandomItem ();
		customerSub = RandomSubCat ();

		playerGuy.GetComponent<CharacterCustomizationScript>().SetAppearanceByData (customer.GetCustomizationData ());
		customerGuy.GetComponent<CharacterCustomizationScript> ().SetAppearanceByData (GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ().GetCustomizationData ());

			//runSubInteraction(0);
		runInteraction (0);

	}
	public void runInteraction(int i)
	{
		switch (i)
		{
		case 0:
			{
				saleSuccessful = false;
				GameMaster.Instance.ModeSetUI();
				//GameMaster.Instance.CameraLock = true;
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
				pic.sprite = customerItem.Picture;
				SetBorder (customerItem);
				currentAmount = customerItem.UnitCost;
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
				disableSpace = true;
				break;
			}
		case 4:
			{
				disableSpace = false;
				item.SetBool("ItemIn",false);
				text.SetText(customerResponce);
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
				text.SetText(playerResponce);
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
				failCounter = 0;
				GameMaster.Instance.ModeSetPlay();
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.FirstPerson);
				Destroy (playerGuy, 1f);
				Destroy (customerGuy, 1f);
				subCate = true;
				serve.AiExit (AInum);
				Debug.Log ("I r Exit"+AInum.ToString());
				SpawnAfterServed (AInum);
				//GameMaster.Instance.CameraLock = false;

				if (saleSuccessful == true) 
				{
					SaleSuccess (customerItem);
				}
				// dont forget set stuff false, change animator stuf that dont need to go away disableSpace = true;
				break;
			}
		default:
			break;
		}
	}
	public void runSubInteraction(int i)
	{
		switch (i) 
		{
		case 0:
			{
				saleSuccessful = false;
				labelCat.SetText ("Select a " + customerSub.Name.ToString ());
				GameMaster.Instance.ModeSetUI();
				//GameMaster.Instance.CameraLock = true;
				bars.SetBool ("BarIn", true);
				player.SetBool ("PlayerIn", true);
				name.SetText (GameMaster.Instance.Player.Name);
				//array of random greetings
				text.SetText (greet [Random.Range (0, greet.Length - 1)]);
				speech.SetBool ("SpeechIn", true);
				disableSpace = false;
				break;
			}
		case 1:
			{
				customer.SetBool ("CustomerIn", true);
			
				text.SetText(customerSubcate[Random.Range(0 , customerSubcate.Length - 1)]+ customerSub.Name.ToString());
				name.SetText("Bryawando");
				inventoryP.SetActive (true);
				inventory.SetBool ("InvenO", true);
				disableSpace = true;
				break;
			}
		case 2:
			{
				inventoryP.SetActive (false);
				speech.SetBool ("SpeechIn", false);
				percentagePanel.SetActive (true);
				perc.SetBool ("PerIn", true);
				disableSpace = false;
				item.SetBool("ItemIn",true);
				break;
			}
		case 3:
			{
				buttonpanel.SetActive (true);
				button.SetBool ("ButtonIn", true);
				disableSpace = true;
				break;
			}
		case 4:
			{
				disableSpace = false;
				item.SetBool("ItemIn",false);
				text.SetText(customerResponce);
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
				text.SetText(playerResponce);
				name.SetText(GameMaster.Instance.Player.Name);
				// dont forget set stuff false, change animator stuf that dont need to go away disableSpace = true;
				break;
			}
		case 6:
			{
				player.SetBool ("PlayerIn", false);
				speech.SetBool ("SpeechIn", false);
				bars.SetBool ("BarIn", false);
				Destroy (playerGuy, 1f);
				Destroy (customerGuy, 1f);
				counter = 0;
				subFailCounter = 0;
				failCounter = 0;
				if (saleSuccessful == true) 
				{
					if (inventoryType == 0)
						SaleSuccess (customerItem);
					else if (inventoryType == 1)
						SaleSuccessInventory (customerItem);
				}
				GameMaster.Instance.ModeSetPlay();
				//GameMaster.Instance.CameraLock = false;
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.FirstPerson);

				subCate = false;
				subNothing = false;
				serve.AiExit (AInum);
				SpawnAfterServed (AInum);
				// dont forget set stuff false, change animator stuf that dont need to go away disableSpace = true;
				break;
			}
		default:
			break;
		}

	}
	public void NoItems()
	{
		customerResponce = customerSubSad[Random.Range (0, customerSubSad.Length - 1)];
		playerResponce = playerSad[Random.Range (0, playerSad.Length - 1)];
		inventoryP.SetActive (false);
		runSubInteraction (4);
		counter = 5;
	}

	public void SelectSubItem(Item subItem, int type)
	{
		inventoryType = type;

		if (customerSub == subItem.Subcategory && subNothing == false) 
		{
			counter = 3;
			runSubInteraction (2);
			customerItem = subItem;
			itemName.SetText(customerItem.Name.ToString());
			SetBorder (customerItem);
			currentAmount = customerItem.UnitCost;
			itemCost.SetText (currentAmount.ToString ());
		}
		else 
		{
			if (subFailCounter >= 3) 
			{
				customerResponce = customerSubSad[Random.Range (0, customerSubSad.Length - 1)];
				playerResponce = playerSad[Random.Range (0, playerSad.Length - 1)];
				inventoryP.SetActive (false);
				runSubInteraction (4);
				counter = 5;
				customerFailed++;
			} 
			else 
			{
				text.SetText (customerFind[subFailCounter]);
				subFailCounter++;
			}
		}
	}

	void MarkUpTooHigh()
	{
		if (failCounter >= 3) 
		{
			customerResponce = customerSad[Random.Range (0, customerSad.Length - 1)];
			playerResponce = playerSad[Random.Range (0, playerSad.Length - 1)];
			if (subCate == false) 
			{
				runInteraction (4);
			} 
			else 
			{
				runSubInteraction (4);
			}
			counter = 5;
			customerFailed++;
		} 
		else 
		{
			item.SetBool("ItemIn",false);
			name.SetText("Bryawando");
			text.SetText(customerResponce);
			buttonpanel.SetActive (false);
			speech.SetBool ("SpeechIn", true);
			perc.SetBool ("PerIn", false);
			percentagePanel.SetActive (false);
			markUpFail = true;
			disableSpace = false;
			failCounter++;
		}

	}
	public void confirmButton()
	{
		float per = CheckMarkUp ();

		if (percentage > per) 
		{
			Debug.Log ("Customer sad");
			customerResponce = tooHigh[Random.Range (0, tooHigh.Length - 1)];
			MarkUpTooHigh ();
		}
		else
		{
			saleSuccessful = true;
			customerResponce = customerHappy [Random.Range (0, customerHappy.Length - 1)];
			playerResponce = playerHappy [Random.Range (0, playerHappy.Length - 1)];
			Debug.Log ("Customer happy");
			if (subCate == false) 
			{
				runInteraction (4);
			} 
			else
				runSubInteraction (4);
			
			counter = 5;
			customerServed++;
	
		}
	}

	void OnTriggerEnter(Collider other)
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
		percentage = (profit / customerItem.UnitCost) * 100;
		calPercentage.SetText ((percentage + 100).ToString ("f0")+"%");

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
		List<ItemSubcategorySO> tempCats = new List<ItemSubcategorySO> ();

		foreach (ItemSubcategorySO cat in GameMaster.Instance.ItemManager.Database.Subcategories) 
		{
			if (cat.Name.ToString() != ItemSubcategory.Nothing.ToString()) 
			{
				tempCats.Add (cat);
			}
		}
		return tempCats [Random.Range (0, tempCats.Count)];
	}

	float CheckMarkUp()
	{
		if (customerItem.Quality == ItemQuality.Low) 
		{
			return Random.Range (20, 25);
		}
		if (customerItem.Quality == ItemQuality.Medium) 
		{
			return Random.Range (28, 35);
		}
		if (customerItem.Quality == ItemQuality.High) 
		{
			return Random.Range(40,50);
		}
		return 50;
	}

	void SaleSuccess(Item cusitem)
	{
		//normal sale

		GameMaster.Instance.Player.Business.IncreaseMoney(currentAmount);
		Debug.Log ("Doooo i runnss" +	GameMaster.Instance.Player.Business.Money.ToString ());
		int i = 0;
		foreach (Item items in GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay) 
		{
			if (items != null) 
			{
				if (items.ItemID == cusitem.ItemID) 
				{
					GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay [i] = null;
					shopMan.RemoveItem (i);

				}
			}
			i++;
		}
	}
	void SaleSuccessInventory(Item item)
	{
        string outstuff = "";
		//normal sale

		GameMaster.Instance.Player.Business.IncreaseMoney(currentAmount);

		int i = 0;
		for (int iItem = 0; i < GameMaster.Instance.Player.Business.WarehouseInventory.Items.Count; i++)
        {
            OrderItem items = GameMaster.Instance.Player.Business.WarehouseInventory.Items[iItem];

            if (items != null)
            {
                if (items.ItemID == item.ItemID)
                {
                    if (items.Quantity > 1)
                        GameMaster.Instance.Player.Business.WarehouseInventory.Items[iItem].ReduceQuantity(1, false, out outstuff);
                    else
                        GameMaster.Instance.Player.Business.WarehouseInventory.RemoveItem(iItem, out outstuff);
                }
            }
            i++;
        }
	}

	void SetBorder(Item purchasedItem)
	{
		if (purchasedItem.Quality == ItemQuality.Low)
		{
			border.sprite = bronze;;

		}
		else if (purchasedItem.Quality == ItemQuality.Medium)
		{
			border.sprite = silver;;

		}
		else if (purchasedItem.Quality == ItemQuality.High)
		{
			border.sprite = gold;;
		}

	}
	void spawnAI2()
	{
		spawner.SpawnAI2();
	}
	void spawnAI3()
	{
		spawner.SpawnAI3();
	}
	void spawnAI1()
	{
		spawner.SpawnAI1();
	}

	void SpawnAfterServed(int i)
	{
		if (endOfDay == false)
		{
			switch (i) 
			{
			case 1:
				{
					Invoke ("spawnAI1", Random.Range (3f, 6f));
					break;
				}
			case 2:
				{
					Invoke ("spawnAI2", Random.Range (3f, 5f));
					break;
				}
			case 3:
				{
					Invoke ("spawnAI3", Random.Range (3f, 5f));
					break;
				}
			}
		}
	}

	public void NextDayReset()
	{
		particle.SetActive (true);
		cube.SetActive (false);
		customerFailed = 0;
		customerServed = 0;
	}
}
