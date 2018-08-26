using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour 
{
	
	[SerializeField] //items
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	[SerializeField] //suppliers
	private GameObject Supplierbtn;
	private Transform SupplierContent;

	public GameObject qtyPanel;
	private PlayerUiController playerUI;

	[HideInInspector]
	public Item purchasedItem;

	public Sprite bronze, silver, gold;

	string supplier = "all",category = "all",subCategory = "all";

	public GameObject moneyMsg;

	Button computerBtn, allBtn, hardwareBtn, componentBtn, gamingBtn, merchBtn, 
	//computers
	desktopBtn,LaptopBtn,
	//components
	gpuBtn,CpuBtn,
	//gaming
	pcgameBtn,ConsoleBtn,
	//merch
	figurineBtn,
	//hardware
	keyboardBtn,MiceBtn
	;


	// time money other stuff new script

	void Start () 
	{
		playerUI = this.gameObject.GetComponent<PlayerUiController>();
		AddByCateSupp (category, supplier, subCategory);
		AddSupplier();

		// intilise buttons
		allBtn = transform.Find("CategoryPanel/catListPanel/allBtn").GetComponent<Button>();
		computerBtn = transform.Find("CategoryPanel/catListPanel/ComputersBtn").GetComponent<Button>();
		hardwareBtn = transform.Find("CategoryPanel/catListPanel/HardwareBtn").GetComponent<Button>();
		componentBtn = transform.Find("CategoryPanel/catListPanel/ComponetsBtn").GetComponent<Button>();
		gamingBtn = transform.Find("CategoryPanel/catListPanel/gamningBtn").GetComponent<Button>();
		merchBtn = transform.Find("CategoryPanel/catListPanel/merchBtn").GetComponent<Button>();

		desktopBtn = transform.Find("CategoryPanel/catListPanel/ComputersBtn/computerPanel/Computerbtn").GetComponent<Button>();
		LaptopBtn = transform.Find("CategoryPanel/catListPanel/ComputersBtn/computerPanel/Laptopbtn").GetComponent<Button>();

		gpuBtn = transform.Find("CategoryPanel/catListPanel/ComponetsBtn/componentsPanel/GPUbtn").GetComponent<Button>();
		CpuBtn = transform.Find("CategoryPanel/catListPanel/ComponetsBtn/componentsPanel/CPUbtn").GetComponent<Button>();

		pcgameBtn = transform.Find("CategoryPanel/catListPanel/gamningBtn/GamingPanel/PcGamesBtn").GetComponent<Button>();
		ConsoleBtn = transform.Find("CategoryPanel/catListPanel/gamningBtn/GamingPanel/ConsoleGameBtn").GetComponent<Button>();

		figurineBtn = transform.Find("CategoryPanel/catListPanel/merchBtn/MerchandicePanel/FigurineBtn").GetComponent<Button>();

		keyboardBtn =  transform.Find("CategoryPanel/catListPanel/HardwareBtn/HardwarePanel/KeyboardBtn").GetComponent<Button>();
		MiceBtn =  transform.Find("CategoryPanel/catListPanel/HardwareBtn/HardwarePanel/MiceBtn").GetComponent<Button>();

		// When button clicked
		allBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate("all");});
		computerBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(ItemCategory.Computers.ToString());});
		hardwareBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(ItemCategory.Hardware.ToString());});
		componentBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(ItemCategory.Components.ToString());});
		gamingBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(ItemCategory.Gaming.ToString());});
		merchBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(ItemCategory.Merchandise.ToString());});
		computerBtn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(ItemCategory.Computers.ToString());});

		desktopBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Computers.ToString(),ItemSubcategory.Desktop.ToString());});
		LaptopBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Computers.ToString(),ItemSubcategory.Laptop.ToString());});

		gpuBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Components.ToString(),ItemSubcategory.GPU.ToString());});
		CpuBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Components.ToString(),ItemSubcategory.CPU.ToString());});

		pcgameBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Gaming.ToString(),ItemSubcategory.PCGame.ToString());});
		ConsoleBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Gaming.ToString(),ItemSubcategory.ConsoleGame.ToString());});

		figurineBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Merchandise.ToString(),ItemSubcategory.figurines.ToString());});

		keyboardBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Hardware.ToString(),ItemSubcategory.Keyboard.ToString());});
		MiceBtn.GetComponent<Button>().onClick.AddListener(delegate {SetSubCate(ItemCategory.Hardware.ToString(),ItemSubcategory.Mouse.ToString());});


	}

	void Update()
	{

	}

	public void AddSupplier()
	{
		ClearSupp ();

		int iSupplier;

		for (iSupplier = 0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++)
		{
			SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers[iSupplier];

			GameObject newSupp = Instantiate(Supplierbtn, SupplierContent);
			//newSupp.transform.Find("SuppName").GetComponent<TMP_Text>().text = supplier.Name;
			newSupp.transform.Find("Button/Text").GetComponent<TMP_Text>().text = supplier.Name;

			newSupp.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {SetSupp(supplier.Name);});
		}
	}

	public void SetSupp(string supplier)
	{
		supplier = supplier;  
		AddByCateSupp (category, supplier,subCategory);
	}

	public void SetCate(string cat)
	{
		category = cat;
		subCategory = "all";
		AddByCateSupp (category, supplier, subCategory);
	}

	public void SetSubCate(string cat,string Subcat)
	{
		subCategory = Subcat;
		category = cat;
		AddByCateSupp (category, supplier, subCategory);


	}

	public void AddByCateSupp(string cat,string supp,string subcat)
	{
		ClearInventory();

		int iSupplier, iItem;
		bool allSupp, allCat,allSubcat;

		if (supp == "all")
			allSupp = true;
		else
			allSupp = false;

		if (cat == "all")
			allCat = true;
		else
			allCat = false;

		if (subcat == "all")
			allSubcat = true;
		else
			allSubcat = false;

		// gets SOs from suppliers and displays them based on categoryId and supplier
		for ( iSupplier=0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++)
		{
			SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers[iSupplier];

			if (supplier.Name == supp && allSupp == false) 
			{	

				for (iItem = 0; iItem < supplier.Inventory.Items.Count; iItem++) 
				{
					Item item = supplier.Inventory.Items[iItem];

					if (item.Category.Name == cat && allCat == false && allSubcat == true) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);

						SetItem (newItem, iSupplier, iItem, item);
					} 
					else if (allSubcat == false && allCat == false && item.Subcategory.EnumID.ToString() == subcat ) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
						SetItem (newItem, iSupplier, iItem, item);
					}
					else if (allCat == true ) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
						SetItem (newItem, iSupplier, iItem, item);
					}
				}
			} 
			else if (allSupp == true) 
			{
				for (iItem = 0; iItem < supplier.Inventory.Items.Count; iItem++) 
				{
					Item item = supplier.Inventory.Items [iItem];

					if (item.Category.Name == cat && allCat == false && allSubcat == true) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);

						SetItem (newItem, iSupplier, iItem, item);
					} 	
					else if (allSubcat == false && allCat == false && item.Subcategory.EnumID.ToString() == subcat ) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
						SetItem (newItem, iSupplier, iItem, item);
					}
					else if (allCat == true ) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
						SetItem (newItem, iSupplier, iItem, item);
					}


				}
			} 
		}
	}

	/// Clears out any existing shop UI items
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

	public void ClearSupp()
	{

		if (SupplierContent == null)
		{
			SupplierContent = transform.Find("Button/SupplierList/Viewport/Content");
		}

		foreach (Transform child in SupplierContent)
		{
			Destroy(child.gameObject);
		}
	}

	public void BuyOnClick()
	{
		int iSupplier, iItem;

		ItemContainerScript selected = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemContainerScript>();

		iSupplier = selected.GetComponent<ItemContainerScript>().SupplierIndex;
		iItem = selected.GetComponent<ItemContainerScript>().ItemIndex;

		purchasedItem = GameMaster.Instance.SupplierManager.Suppliers[iSupplier].Inventory.Items[iItem];
		playerUI.SetItem(purchasedItem,iSupplier,iItem);

		//Make sure we can find the item and a can afford it
		if (purchasedItem == null)
		{
			Debug.Log("Unable to find item");
			return;
		}
		else if (purchasedItem.UnitCost >= GameMaster.Instance.Player.Business.Money)
		{
			moneyMsg.transform.Find ("noMoneyText").GetComponent<TMP_Text> ().text = "You Do Not Have Enough Money!";
			moneyMsg.SetActive (true);
			StartCoroutine(moneyError());
			Debug.Log(string.Format("Not enough monies. Purchase Price: {0}; Player Moneyz: {1}", purchasedItem.UnitCost.ToString(), GameMaster.Instance.Player.Business.Money.ToString()));
			return;
		}
		else
		{
			Debug.Log(string.Format("Purchase info: Item Name: {0}; Purchase Price: {1}; Player remaining Moneyz: {2}", purchasedItem.Name, purchasedItem.UnitCost.ToString(), (GameMaster.Instance.Player.Business.Money - purchasedItem.UnitCost).ToString()));
		}			
		qtyPanel.SetActive(true);
	}
		

	void SetItem(GameObject newItem,int iSupplier,int iItem, Item item)
	{
		newItem.GetComponent<ItemContainerScript> ().SupplierIndex = iSupplier;
		newItem.GetComponent<ItemContainerScript> ().ItemIndex = iItem;

		newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
		newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
		newItem.transform.Find ("Supplier").GetComponent<TMP_Text> ().text = GameMaster.Instance.SupplierManager.Suppliers [iSupplier].Name.ToString ();
		newItem.transform.Find ("Price").GetComponent<TMP_Text> ().text = GameMaster.Instance.CurrencySymbol + CalculateDiscount(item, iSupplier).ToString();

		if (item.Quality == ItemQuality.Low)
		{
			newItem.transform.Find ("Quality").GetComponent<Image> ().sprite = bronze;;

		}
		else if (item.Quality == ItemQuality.Medium)
		{
			newItem.transform.Find ("Quality").GetComponent<Image> ().sprite = silver;;

		}
		else if (item.Quality == ItemQuality.High)
		{
			newItem.transform.Find ("Quality").GetComponent<Image> ().sprite = gold;;
		}

		newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener(BuyOnClick);
	}
		
	public float CalculateDiscount(Item pi,int iSupplier)
	{
		float itemPrice = 0;

        itemPrice = GameMaster.DiscountPrice(pi.UnitCost, GameMaster.Instance.SupplierManager.Suppliers[iSupplier].DiscountPercentage);

		return itemPrice;

	}
	IEnumerator moneyError()
	{
		yield return new WaitForSeconds(2);
		moneyMsg.SetActive (false);
	}
}
