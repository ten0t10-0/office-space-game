using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashBoard : MonoBehaviour 
{

	public TextMeshProUGUI time, money, company, failedOrders,CompleteOrders,currentOrders;

	int failed, current,complete;

	// Use this for initialization
	void Start () 
	{
		failed = ListCount(GameMaster.Instance.OrderManager.GetFailedOrders());
		current = ListCount(GameMaster.Instance.OrderManager.GetOpenOrders());
		complete = ListCount(GameMaster.Instance.OrderManager.GetCompletedOrders());


//		company.SetText((GameMaster.Instance.Player.Business.Name).ToString());

		failedOrders.SetText(failed.ToString());
		CompleteOrders.SetText(complete.ToString());
		currentOrders.SetText(current.ToString());
	}
	
	// Update is called once per frame
	void Update () 
	{
		money.SetText("$ " + (GameMaster.Instance.Player.Business.Money).ToString());
	
		time.SetText (GameMaster.Instance.GameTimeString12 ());
	}
	public int ListCount(List<Order> list)
	{
		 int c = list.Count;
		return c;

	}
}
