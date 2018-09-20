using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DisplayInventory : MonoBehaviour 
{
	[SerializeField] //items
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	public TextMeshProUGUI category,label;

	public Button btnDecrease,btnIncrease,cancel,confirm,shop,inventory,nothing; 

	public Sprite bronze, silver, gold;

	public GameObject confirmPanel;

	int min = 0, max;

	int increasePerClick = 1;
	int currentAmount = 0;
	int inventoryType = 0;
	string [] cat ;
	CustomerInteractionUI cusInt;

	Item tempItem;

	bool inventoryS = true;

	void Start () 
	{
		cusInt = FindObjectOfType<CustomerInteractionUI>();
		currentAmount = 0;
		cat = Enum.GetNames(typeof(ItemCategory));
		max = cat.Length-1;
		category.SetText(cat [currentAmount].ToString ());
		//butons
		AddShopItems();
		inventoryType = 0;

		shop.GetComponent<Button> ().onClick.AddListener (delegate {SetShopInventory();});
		inventory.GetComponent<Button>().onClick.AddListener(delegate {SetInventory ();});
		nothing.GetComponent<Button>().onClick.AddListener(delegate {SetNothing();});
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ChangeAmount(bool increase)
	{
		// clamp current value between min-max
		currentAmount = Mathf.Clamp(currentAmount + (increase ? increasePerClick : -increasePerClick), min, max);
		category.SetText (cat [currentAmount].ToString ());

		if (inventoryS == true)
			AddShopItems ();
		else
			AddItems();
		
		// disable buttons i
		btnDecrease.interactable = currentAmount > min;
		btnIncrease.interactable = currentAmount < max;
	}

	public void AddItems()
	{
		ClearInventory ();

		foreach (OrderItem item in GameMaster.Instance.Player.Business.WarehouseInventory.Items) 
		{
			if (item.Category.EnumID.ToString() == cat[currentAmount]) 
			{	
				GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
				newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
				newItem.transform.Find ("Border/Image").GetComponent<Image> ().sprite = item.Picture;

				if (item.Quality == ItemQuality.Low)
				{
					newItem.transform.Find ("Border").GetComponent<Image> ().sprite = bronze;;

				}
				else if (item.Quality == ItemQuality.Medium)
				{
					newItem.transform.Find ("Border").GetComponent<Image> ().sprite = silver;;

				}
				else if (item.Quality == ItemQuality.High)
				{
					newItem.transform.Find ("Border").GetComponent<Image> ().sprite = gold;;
				}
				newItem.GetComponent<Button> ().onClick.AddListener(delegate {ConfirmPanel(item);});
			}
		}
	}
	public void AddShopItems()
	{
		ClearInventory ();

		foreach ( Item item  in GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay) 
		{
			if (item != null) 
			{
				if (item.Category.EnumID.ToString () == cat [currentAmount])
				{
					GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
					newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
					newItem.transform.Find ("Border/Image").GetComponent<Image> ().sprite = item.Picture;
					//newItem.transform.Find ("Button/Qty").GetComponent<TMP_Text> ().text = item.Quantity.ToString ();

					if (item.Quality == ItemQuality.Low) 
					{
						newItem.transform.Find ("Border").GetComponent<Image> ().sprite = bronze;
						;

					} 
					else if (item.Quality == ItemQuality.Medium) 
					{
						newItem.transform.Find ("Border").GetComponent<Image> ().sprite = silver;
						;

					} 
					else if (item.Quality == ItemQuality.High)
					{
						newItem.transform.Find ("Border").GetComponent<Image> ().sprite = gold;
						;
					}
					newItem.GetComponent<Button> ().onClick.AddListener (delegate {ConfirmPanel (item);});
				}
			}

		}
	}

	public void ClearInventory()
	{
		if (scrollViewContent == null)
		{
			scrollViewContent = transform.Find("Scroll View/Viewport/Content");
		}

		foreach (Transform child in scrollViewContent)
		{
			Destroy(child.gameObject);
		}
	}

	void SetInventory()
	{
		inventoryS = true;
		inventoryType = 1;
		AddItems ();
	}
	void SetShopInventory()
	{
		inventoryS = false;
		inventoryType = 0;
		AddShopItems ();
	}
	void SetNothing()
	{
		inventoryType = -1;
		confirmPanel.SetActive (true);
	}

	public void ConfirmPanel(Item item)
	{
		tempItem = item;
		confirmPanel.SetActive (true);
	}

	public void ConfirmButton()
	{
		cusInt.SelectSubItem (tempItem,inventoryType);
		confirmPanel.SetActive (false);
	}
}
