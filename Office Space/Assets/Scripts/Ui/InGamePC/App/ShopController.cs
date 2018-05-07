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
	public RectTransform SupplierContent;

	public GameObject qtyPanel;
	private PlayerUiController playerUI;

	[HideInInspector]
	public Item purchasedItem;

	public TextMeshProUGUI time;
	public TextMeshProUGUI money;

	string supp = "all";
	string cate = "all";

	public Button all,elec,furn;

	public Dropdown drop;

	void Start () 
	{
        playerUI = this.gameObject.GetComponent<PlayerUiController>();
		AddAllItems ();
		AddSupplier ();

		all.GetComponent<Button>().onClick.AddListener(delegate {SetCate("all");});
		elec.GetComponent<Button>().onClick.AddListener(delegate {SetCate(GameMaster.Instance.ItemManager.Database.Categories[1].name);});
		furn.GetComponent<Button>().onClick.AddListener(delegate {SetCate(GameMaster.Instance.ItemManager.Database.Categories[2].name);});
	
		money.SetText((GameMaster.Instance.Player.Business.Money).ToString());



	}

	void Update()
	{
		

	}
		
	public void AddSupplier()
	{
		//ClearSuppliers ();

		int iSupplier;

		for (iSupplier = 0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++)
		{
			SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers[iSupplier];

			//GameObject newSupp = Instantiate(Supplierbtn, SupplierContent);
			//newSupp.transform.Find("SuppName").GetComponent<TMP_Text>().text = supplier.Name;

			GameObject newSupp = (GameObject)Instantiate(Supplierbtn);
			newSupp.transform.SetParent(SupplierContent);
			newSupp.transform.Find("Button/Text").GetComponent<TMP_Text>().text = supplier.Name;

			newSupp.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {SetSupp(supplier.Name);});
		}
	}

	public void SetSupp(string supplier)
	{
	    supp = supplier;  
		AddByCateSupp (cate, supp);
	}

	public void SetCate(string cat)
	{
		cate = cat;  
		AddByCateSupp (cate, supp);
	}

	public void AddAllItems()
	{
		ClearInventory();

        int iSupplier, iItem;

        for (iSupplier = 0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++)
        {
            SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers[iSupplier];

            for (iItem = 0; iItem < supplier.Inventory.Items.Count; iItem++)
            {
                Item item = supplier.Inventory.Items[iItem];

                GameObject newItem = Instantiate(ItemContainer, scrollViewContent);

				newItem.GetComponent<ItemContainerScript>().SupplierIndex = iSupplier;
				newItem.GetComponent<ItemContainerScript>().ItemIndex = iItem;

				newItem.transform.Find("Image").GetComponent<Image>().sprite = item.Picture;
				newItem.transform.Find("Name").GetComponent<TMP_Text>().text = item.Name;
				newItem.transform.Find("Price").GetComponent<TMP_Text>().text = "$"+item.UnitCost.ToString();
				newItem.transform.Find("Supplier").GetComponent<TMP_Text>().text = supplier.Name;

				newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(BuyOnClick);

                Debug.Log(item.Category.name);
            }
        }
	}

	public void AddByCateSupp(string cat,string supp)
	{
		ClearInventory();

		int iSupplier, iItem;
		bool allSupp, allCat;

		if (supp == "all")
			allSupp = true;
		else
			allSupp = false;

		if (cate == "all")
			allCat = true;
		else
			allCat = false;

		
		for ( iSupplier=0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++)
		{
			SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers[iSupplier];

			if (supplier.Name==supp && allSupp==false)
			{	
			
				for (iItem = 0; iItem < supplier.Inventory.Items.Count; iItem++) 
				{
					Item item = supplier.Inventory.Items [iItem];
				
					if (item.Category.name == cat && allCat == false) 
					{
						GameObject newItem = Instantiate (ItemContainer, scrollViewContent);

						newItem.GetComponent<ItemContainerScript> ().SupplierIndex = iSupplier;
						newItem.GetComponent<ItemContainerScript> ().ItemIndex = iItem;

						newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
						newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
						newItem.transform.Find ("Price").GetComponent<TMP_Text> ().text = item.UnitCost.ToString();

						newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener (BuyOnClick);
					}
					else if (allCat==true)
						{
							GameObject newItem = Instantiate (ItemContainer, scrollViewContent);

							newItem.GetComponent<ItemContainerScript> ().SupplierIndex = iSupplier;
							newItem.GetComponent<ItemContainerScript> ().ItemIndex = iItem;

							newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
							newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
							newItem.transform.Find ("Price").GetComponent<TMP_Text> ().text = item.UnitCost.ToString();

							newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener (BuyOnClick);
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

	public void BuyOnClick()
	{
        int iSupplier, iItem;

        ItemContainerScript selected = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemContainerScript>();

        iSupplier = selected.GetComponent<ItemContainerScript>().SupplierIndex;
        iItem = selected.GetComponent<ItemContainerScript>().ItemIndex;

		purchasedItem = GameMaster.Instance.SupplierManager.Suppliers[iSupplier].Inventory.Items[iItem];
        playerUI.SetItem(purchasedItem);

        //Make sure we can find the item and a can afford it
        if (purchasedItem == null)
		{
			Debug.Log("Unable to find item");
			return;
		}
		else if (purchasedItem.UnitCost >= GameMaster.Instance.Player.Business.Money)
		{
			Debug.Log(string.Format("Not enough monies. Purchase Price: {0}; Player Moneyz: {1}", purchasedItem.UnitCost.ToString(), GameMaster.Instance.Player.Business.Money.ToString()));
			return;
		}
        else
        {
            Debug.Log(string.Format("Purchase info: Item Name: {0}; Purchase Price: {1}; Player remaining Moneyz: {2}", purchasedItem.Name, purchasedItem.UnitCost.ToString(), (GameMaster.Instance.Player.Business.Money - purchasedItem.UnitCost).ToString()));
        }
			
		qtyPanel.SetActive(true);

    }


}
