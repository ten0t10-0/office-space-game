using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiOrder : MonoBehaviour 
{
//	[SerializeField] //Orders
//	private GameObject OrderContainer;
//	private Transform scrollViewContent;
//
//	[SerializeField] //items
//	private GameObject ItemContainer;
//	private Transform scrollContent;
//
//	[SerializeField] //inventory
//	private GameObject inventoryContainer;
//	private Transform iscrollContent;
//
//
//	GameObject SelectedOrder;
//
//
//	void Start () 
//	{
//		PopulateOrders ();
//	}
//	
//
//	void Update () 
//	{
//		
//	}
//
//
//	//displays the orders
//	void PopulateOrders()
//	{
//		ClearOrders ();
//
//		List<Order> order = GameMaster.Instance.OrderManager.GetOpenOrders();
//
//		for (int i = 0; i < order.Count; i++) 
//		{
//			GameObject newItem = Instantiate (OrderContainer, scrollViewContent);
//
//			newItem.transform.Find ("Button/time").GetComponent<TMP_Text> ().text = order [i].GetTimeRemaining ().ToString ();
//			newItem.transform.Find ("Button/Customer").GetComponent<TMP_Text> ().text = order [i].Customer.FullName ();
//
//			newItem.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener (delegate {SetOrder (order, i);});
//		}
//	}
//	// displays the order information to the invoice
//	void SetOrder(List<Order> order,int i, GameObject setOrder)
//	{
//		i = i - 1;
//
//		ClearItems();
//
//		SelectedOrder = setOrder;
//
////		customer.SetText (order [i].Customer.FullName().ToString());
////
////		//Date.SetText(order[i].DateDue.ToString());
////
////		timeRemaining.SetText(order[i].GetTimeRemaining().ToString());
////		total.SetText("$" + order[i].TotalValue().ToString());
//
//
//		foreach (OrderItem item in order[i].Items)
//		{
//			GameObject newItems = Instantiate (ItemContainer, scrollContent);
//			newItems.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
//			newItems.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;
//			newItems.transform.Find ("Supplier").GetComponent<TMP_Text> ().text = item.Quantity.ToString();
//
//			if (item.Quality == ItemQuality.Low)
//			{
//				newItems.transform.Find("Quality").GetComponent<Image> ().sprite = bronze;;
//
//			}
//			else if (item.Quality == ItemQuality.Medium)
//			{
//				newItems.transform.Find ("Quality").GetComponent<Image> ().sprite = silver;;
//
//			}
//			else if (item.Quality == ItemQuality.High)
//			{
//				newItems.transform.Find ("Quality").GetComponent<Image> ().sprite = gold;;
//			}
//			newItems.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {selectItem(item,newItems,i);});
//		}
//
//	}
//
//	#region clear scroll views
//	//clear scroll views
//	public void ClearOrders()
//	{
//		if (scrollViewContent == null)
//		{
//			scrollViewContent = transform.Find("Scroll View/Viewport/Content");
//		}
//		foreach (Transform child in scrollViewContent)
//		{
//			Destroy(child.gameObject);
//		}
//	}
//	public void ClearItems()
//	{
//		if (scrollContent == null)
//		{
//			scrollContent = transform.Find("OrderInvoice/Items/Scroll View/Viewport/Content");
//		}
//		foreach (Transform childs in scrollContent)
//		{
//			Destroy(childs.gameObject);
//		}
//	}
//	public void ClearInventory()
//	{
//		if (iscrollContent == null)
//		{
//			iscrollContent = transform.Find("orderInventory/Scroll View/Viewport/Content");
//		}
//		foreach (Transform childs in iscrollContent)
//		{
//			Destroy(childs.gameObject);
//		}
//	}
//	#endregion
}
