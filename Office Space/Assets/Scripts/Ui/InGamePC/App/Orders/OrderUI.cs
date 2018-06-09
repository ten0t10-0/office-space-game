using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUI : MonoBehaviour {

	public TextMeshProUGUI companyName, customer, timeRemaining, Date,total;

	[SerializeField] //Orders
	private GameObject OrderContainer;
	private Transform scrollViewContent;

	[SerializeField] //Orders
	private GameObject ItemContainer;
	private Transform scrollContent;

	[HideInInspector]
	public Item purchasedItem;

	public Sprite bronze, silver, gold;

	// Use this for initialization
	void Start () 
	{
		
		DisplayOrders ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		DisplayOrders ();
		companyName.SetText((GameMaster.Instance.Player.Business.Name).ToString());
	}

	public void DisplayOrders()
	{
		ClearOrders ();

		List<Order> order = GameMaster.Instance.OrderManager.GetOpenOrders();

		for (int i = 0; i < order.Count; i++)
		{
			GameObject newItem = Instantiate (OrderContainer, scrollViewContent);

			newItem.transform.Find ("time").GetComponent<TMP_Text> ().text = order[i].GetTimeRemaining().ToString();
			newItem.transform.Find ("Customer").GetComponent<TMP_Text> ().text = order[i].Customer.FullName();

			newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {SetOrder(order,i);});

		}
	}

	void SetOrder(List<Order> order,int i)
	{
		Debug.Log ("Blooooooooooooop");

		ClearItems();

		customer.SetText (order [i].Customer.FullName ());
		Date.SetText(order[i].DateDue.ToString());
		total.SetText(order[i].TotalValue().ToString());


		foreach (OrderItem item in order[i].Items)
		{
			GameObject newItems = Instantiate (ItemContainer, scrollContent);
			newItems.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
			newItems.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
			//newItems.transform.Find ("Qty").GetComponent<TMP_Text> ().text = order[i].

			if (item.Quality == ItemQuality.Low)
			{
				newItems.transform.Find("Quality").GetComponent<Image> ().sprite = bronze;;

			}
			else if (item.Quality == ItemQuality.Medium)
			{
				newItems.transform.Find ("Quality").GetComponent<Image> ().sprite = silver;;

			}
			else if (item.Quality == ItemQuality.High)
			{
				newItems.transform.Find ("Quality").GetComponent<Image> ().sprite = gold;;
			}
				
		}
			



	}
//	public void AddByCateSupp (string cat, string supp, string subcat)
//	{
//		ClearInventory ();
//
//
//		// gets SOs from suppliers and displays them based on categoryId and supplier
//		for (iSupplier = 0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++) {
//			SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers [iSupplier];
//
//			if (supplier.Name == supp && allSupp == false) {	
//
//				for (iItem = 0; iItem < supplier.Inventory.Items.Count; iItem++) {
//					Item item = supplier.Inventory.Items [iItem];
//
//					GameObject newItem = Instantiate (ItemContainer, scrollViewContent);
//
//					SetItem (newItem, iSupplier, iItem, item);
//				}
//			}
//		}
//	}
//	void SetItem(GameObject newItem,int iSupplier,int iItem, Item item)
//	{
//		newItem.GetComponent<ItemContainerScript> ().SupplierIndex = iSupplier;
//		newItem.GetComponent<ItemContainerScript> ().ItemIndex = iItem;
//
//		newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
//		newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
//		newItem.transform.Find ("Supplier").GetComponent<TMP_Text> ().text = GameMaster.Instance.SupplierManager.Suppliers [iSupplier].Name.ToString ();
//		newItem.transform.Find ("Price").GetComponent<TMP_Text> ().text = "$ " + CalculateMarkUp(item, iSupplier).ToString();
//
//		if (item.Quality == ItemQuality.Low)
//		{
//			newItem.transform.Find ("Quality").GetComponent<Image> ().sprite = bronze;;
//
//		}
//		else if (item.Quality == ItemQuality.Medium)
//		{
//			newItem.transform.Find ("Quality").GetComponent<Image> ().sprite = silver;;
//
//		}
//		else if (item.Quality == ItemQuality.High)
//		{
//			newItem.transform.Find ("Quality").GetComponent<Image> ().sprite = gold;;
//		}
//
//		newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener(BuyOnClick);
//	}
//
	public void ClearOrders()
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
	public void ClearItems()
	{
		if (scrollContent == null)
		{
			scrollContent = transform.Find("OrderInvoice/Items/Scroll View/Viewport/Content");
		}
		foreach (Transform childs in scrollContent)
		{
			Destroy(childs.gameObject);
		}
	}
	public float CalculateMarkUp(Item pi,int iSupplier)
	{
		float itemPrice = 0;

		itemPrice = pi.UnitCost * (1 + GameMaster.Instance.SupplierManager.Suppliers[iSupplier].MarkupPercent);

		return itemPrice;

	}
//	public float CalculateMarkUp(Item pi,int iSupplier)
//	{
//		float itemPrice = 0;
//
//		itemPrice = pi.UnitCost * (1 + GameMaster.Instance.SupplierManager.Suppliers[iSupplier].MarkupPercent);
//
//		return itemPrice;
//
//	}
}
