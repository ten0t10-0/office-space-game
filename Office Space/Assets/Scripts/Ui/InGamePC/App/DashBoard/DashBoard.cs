using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashBoard : MonoBehaviour 
{

	public TextMeshProUGUI time, money, company, failedOrders,CompleteOrders,currentOrders,customersat,markUp,amount,date;

	int failed, current,complete;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		failed = GameMaster.Instance.OrderManager.CountFailed;
		current = GameMaster.Instance.OrderManager.CountOpen;
		complete = GameMaster.Instance.OrderManager.CountCompleted;

		customersat.SetText ((GameMaster.Instance.Player.Business.CustomerTolerance_Total* 100).ToString ()+"%");
		markUp.SetText((GameMaster.Instance.Player.Business.MarkupPercentage_Total * 100).ToString()+"%");
		amount.SetText ( " $" + GameMaster.Instance.DebtAmounts [GameMaster.Instance.WeekCurrent].ToString());

        date.SetText(GameMaster.Instance.DebtDates[GameMaster.Instance.WeekCurrent].ToShortDateString());

		company.SetText((GameMaster.Instance.Player.Business.Name).ToString());
		money.SetText("$ " + (GameMaster.Instance.Player.Business.Money).ToString());
	
		time.SetText (GameMaster.Instance.GameTimeString12 ());

		failedOrders.SetText(failed.ToString());
		CompleteOrders.SetText(complete.ToString());
		currentOrders.SetText(current.ToString());
	}
}
