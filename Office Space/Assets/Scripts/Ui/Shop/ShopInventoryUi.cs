using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopInventoryUi : MonoBehaviour 
{

	[SerializeField] //items
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	string [] cat ;
	ShopItemTrigger shopitem;

	public Rigidbody gpu, cpu, console, desktop, laptop, game, keyboard, mouse, figure,motherboard;

	Rigidbody prefab;

	public Button btnDecrease, btnIncrease;

	public Sprite bronze, silver, gold;

	public TextMeshProUGUI category,qty,cost;

	int min = 0, max;

	int increasePerClick = 1;
	int currentAmount = 0;

	public int slot = 0;
	public Vector3 pos;
	public Quaternion rot;
	public Canvas tempCanvas;

	public void spawnShopItems()
	{
		//spawn items at start
	}

	public void setItems(int slots, Vector3 post, Quaternion rotn,Canvas tCanvas)
	{
		slot = slots;
		pos = post;
		rot = rotn;
		tempCanvas = tCanvas;
		Debug.Log (pos.ToString ());
	}

	void Start () 
	{
		currentAmount = 0;
		cat = Enum.GetNames(typeof(ItemCategory));
		max = cat.Length-1;
	    shopitem = FindObjectOfType<ShopItemTrigger> ();
		category.SetText(cat [currentAmount].ToString ());

		AddItems ();
	}

	// ChangeAmout(true) >> increase or ChangeAmount(false) >> decrease
	public void ChangeAmount(bool increase)
	{
		// clamp current value between min-max
		currentAmount = Mathf.Clamp(currentAmount + (increase ? increasePerClick : -increasePerClick), min, max);
		category.SetText (cat [currentAmount].ToString ());
		AddItems ();

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
				newItem.transform.Find ("Pic").GetComponent<Image> ().sprite = item.Picture;
				newItem.transform.Find ("Qty").GetComponent<TMP_Text> ().text = item.Quantity.ToString ();

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
				newItem.GetComponent<Button> ().onClick.AddListener(delegate {spawn(item);});
				//newItem.transform.Find ("Button").GetComponent<Button> ().(delegate {spawn(item);});
			}
		}
	}
	public void DisplayDetails(Item item)
	{
		cost.SetText (item.UnitCost.ToString ());
	}

	public void spawn(Item item)
	{
		 int inventory = GetItemID (item);


		if (item.Subcategory.EnumID == ItemSubcategory.Console)
			prefab = console;

		else if(item.Subcategory.EnumID == ItemSubcategory.ConsoleGame) 
			prefab = game;

		else if (item.Subcategory.EnumID == ItemSubcategory.CPU) 
			prefab = cpu;

		else if (item.Subcategory.EnumID == ItemSubcategory.Desktop) 
			prefab = desktop;

		else if (item.Subcategory.EnumID == ItemSubcategory.figurines) 
			prefab = figure;

		else if (item.Subcategory.EnumID == ItemSubcategory.GPU) 
			prefab = gpu;

		else if(item.Subcategory.EnumID == ItemSubcategory.Keyboard) 
			prefab = keyboard;

		else if (item.Subcategory.EnumID == ItemSubcategory.Laptop) 
			prefab = laptop;

		else if (item.Subcategory.EnumID== ItemSubcategory.Mouse) 
			prefab = mouse;

		else if (item.Subcategory.EnumID == ItemSubcategory.PCGame) 
			prefab = game;

		else if (item.Subcategory.EnumID == ItemSubcategory.MotherBoard) 
			prefab = motherboard;
		
		shopitem.SpawnObject(prefab,pos,rot,slot,item,tempCanvas,inventory);
		AddItems ();
	}

	public void ClearInventory()
	{

		if (scrollViewContent == null)
		{
			scrollViewContent = transform.Find("Panel/Scroll View/Viewport/Content");
		}

		foreach (Transform child in scrollViewContent)
		{
			Destroy(child.gameObject);
		}
	}
	int GetItemID(Item item)
	{
		Item[] inventory  = GameMaster.Instance.Player.Business.WarehouseInventory.Items.ToArray ();

		for (int i = 0; i <= inventory.Length; i++) 
		{
			if (item == inventory [i])
				return i;
		}
		return -1;
	}

}
