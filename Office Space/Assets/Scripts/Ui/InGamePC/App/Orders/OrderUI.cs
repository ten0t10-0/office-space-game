using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUI : MonoBehaviour {

	public TextMeshProUGUI companyName, customer, timeRemaining, Date,total, timerem;

	[SerializeField] //Orders
	private GameObject OrderContainer;
	private Transform scrollViewContent;

	[SerializeField] //items
	private GameObject ItemContainer;
	private Transform scrollContent;

	[SerializeField] //items
	private GameObject inventoryContainer;
	private Transform iscrollContent;

	[SerializeField] //items
	private GameObject TimeContainer;
	private Transform TimeScrollContent;

	Dictionary <int,int> completeOrder = new Dictionary<int,int>();

	public Sprite bronze, silver, gold, tick,plus;

	public GameObject qtyPanel,inventoryPanel,orderPanel;

	int qty, currentAmount = 1, increasePerClick = 1, min = 1, max = 25,ordersNum,i;

	public InputField amount;
	public Button btnDecrease,btnIncrease,confirm;

	public GameObject purchase;

	GameObject selectcontainer,selectedOrder;
	OrderItem selectOrder;
	OrderItem tempitem;
	float currCountdownValue;

	List<Item> tempItem = new List<Item> ();

	// Use this for initialization
	void Start () 
	{
		
		DisplayOrders ();
		AddInventory ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		DisplayTime ();
		companyName.SetText((GameMaster.Instance.Player.Business.Name).ToString());

//	if (GameMaster.Instance.OrderManager.Orders [i].Open == false) 
//	{
//			clearInvoice ();
//	}

	}

	public void DisplayOrders()
	{
		ClearOrders ();

		List<Order> order = GameMaster.Instance.OrderManager.Orders;

		for (int i = 0; i < order.Count; i++)
		{
			if (order [i].Open) 
			{
				GameObject newItem = Instantiate (OrderContainer, scrollViewContent);
				newItem.transform.Find ("Button/Customer").GetComponent<TMP_Text> ().text = order [i].Customer.FullName ();
				newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener (delegate {SetOrder (order, i, newItem);});
			}
		}
	}
		
	public void DisplayTime()
	{
		ClearTime ();
		List<Order> order = GameMaster.Instance.OrderManager.GetOpenOrders ();

		for (int i = 0; i < order.Count; i++) 
		{
			GameObject newItem = Instantiate (TimeContainer, TimeScrollContent);
			if (order [i].Open)
				newItem.transform.Find ("Button/Time").GetComponent<TMP_Text> ().text = order [i].GetTimeRemaining ().ToString ();

		}
	}

	// displays the order information to the invoice
	void SetOrder(List<Order> order,int i, GameObject setOrder)
	{
		i = i - 1;

		ordersNum = i; 

		ClearItems();
		selectedOrder = setOrder;

		customer.SetText (order [i].Customer.FullName().ToString());
		//Date.SetText(order[i].DateDue.ToString());

		timeRemaining.SetText(order[i].GetTimeRemaining().ToString());
		total.SetText("$" + order[i].TotalValue().ToString());


		foreach (OrderItem item in order[i].Items)
		{
			GameObject newItems = Instantiate (ItemContainer, scrollContent);
			newItems.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
			newItems.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
			newItems.transform.Find ("Supplier").GetComponent<TMP_Text> ().text = item.Quantity.ToString();

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
			newItems.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {selectItem(item,newItems,i);});
		}
			
	}

	public void AddInventory()
	{
		tempItem.Clear ();
		foreach (OrderItem item in GameMaster.Instance.Player.Business.WarehouseInventory.Items) 
		{
			tempItem.Add(item);
		}
			
		TempInventory ();

	}
	public void TempInventory()
	{
		ClearInventory ();

		foreach(OrderItem item in tempItem)
		{

				GameObject newItem = Instantiate (inventoryContainer, iscrollContent);
				newItem.transform.Find ("Panel/Name").GetComponent<TMP_Text> ().text = item.Name;
				newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
				newItem.transform.Find ("qty").GetComponent<TMP_Text> ().text = item.Quantity.ToString ();

				newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener (delegate {quantityPanel (item);});
		
		}
	}

	public void selectItem(OrderItem item,GameObject newitem, int orderNum )
	{
		inventoryPanel.SetActive (true);
		selectOrder = item;
		selectcontainer = newitem;
	
		AddInventory ();
	}

	public void quantityPanel(OrderItem item)
	{
		max = item.Quantity;
		qtyPanel.SetActive (true);
		tempitem = item;
	}

	public void AddItem()
	{
		
		selectcontainer.transform.Find ("Button").GetComponent<Button> ().interactable = false;
		tempitem.Quantity = tempitem.Quantity - currentAmount;

		completeOrder.Add (selectOrder.ItemID,currentAmount);
		qtyPanel.SetActive (false);
		inventoryPanel.SetActive (false);
		selectcontainer.transform.Find ("Button").GetComponent<Button> ().image.sprite = tick;
	}

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
	public void ClearInventory()
	{
		if (iscrollContent == null)
		{
			iscrollContent = transform.Find("orderInventory/Scroll View/Viewport/Content");
		}
		foreach (Transform childs in iscrollContent)
		{
			Destroy(childs.gameObject);
		}
	}
	public void ClearTime()
	{
		if (TimeScrollContent == null)
		{
			TimeScrollContent = transform.Find("Scroll ViewTime/Viewport/Content");
		}
		foreach (Transform childs in TimeScrollContent)
		{
				Destroy(childs.gameObject);
		}
	}
	public float CalculateDiscount(Item pi,int iSupplier)
	{
		float itemPrice;

        itemPrice = GameMaster.DiscountPrice(pi.UnitCost, GameMaster.Instance.SupplierManager.Suppliers[iSupplier].DiscountPercentage);

		return itemPrice;
	}
	public void ChangeAmount(bool increase)
	{
		// clamp current value between min-max
		currentAmount = Mathf.Clamp(currentAmount + (increase ? increasePerClick : -increasePerClick), min, max);
		amount.text = currentAmount.ToString();

		// disable buttons i
		btnDecrease.interactable = currentAmount > min;
		btnIncrease.interactable = currentAmount < max;
	}
	public void CompleteOrders()
	{
		string a;
        float paymentTotal;
		Debug.Log ("Bloooooooooooop"+ ordersNum);
		GameMaster.Instance.CompleteOrder(ordersNum, completeOrder, out paymentTotal, out a);

		purchase.SetActive(true);
		purchase.transform.Find("MoneyPopUpText").GetComponent<TMP_Text> ().text = "+" + paymentTotal.ToString();
		StartCoroutine(moneyPopUp());
		Destroy (selectedOrder);
		clearInvoice ();
		completeOrder.Clear();
	}
	IEnumerator moneyPopUp()
	{
		yield return new WaitForSeconds(2);
		purchase.SetActive (false);
	}

//	public void Reset()
//	{
//		
//
//	}
	public void clearInvoice()
	{
		customer.SetText ("");
		Date.SetText("");
		timeRemaining.SetText("");
		total.SetText("");
		ClearItems ();
	}

	void updateText(int ordersNum,GameObject newItem)
	{
			if (newItem != null && newItem.activeInHierarchy)
				newItem.transform.Find ("Button/time").GetComponent<TMP_Text> ().text = GameMaster.Instance.OrderManager.Orders [ordersNum].GetTimeRemaining ().ToString ();
	}


}
