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

	}
	
	// Update is called once per frame
	void Update () 
	{
		failed = GameMaster.Instance.OrderManager.GetFailedOrders().Count;
		current = GameMaster.Instance.OrderManager.GetOpenOrders().Count;
		complete = GameMaster.Instance.OrderManager.GetCompletedOrders().Count;

		company.SetText((GameMaster.Instance.Player.Business.Name).ToString());
		money.SetText("$ " + (GameMaster.Instance.Player.Business.Money).ToString());
	
		time.SetText (GameMaster.Instance.GameTimeString12 ());

		failedOrders.SetText(failed.ToString());
		CompleteOrders.SetText(complete.ToString());
		currentOrders.SetText(current.ToString());
	}
//	public int ListCount(List<Order> list)
//	{
//		 int c = list.Count;
//		return c;
//
//	}
}
