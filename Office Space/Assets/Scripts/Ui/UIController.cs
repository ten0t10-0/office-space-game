using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour 
{
	public GameObject buildmode,endDayOffice,endDayShop,GameOver,NotificationCanvas,roof;
	public GameObject DebtPass,DebtGameOver,DebtLifeLine,DebtNoLifeLineU,confirmDay;// dontforget graphic raycaster
	public Animator buildM,endDayOffA,endDayShopA;

	public TextMeshProUGUI orderFailed, orderComplete,profit,cusFail,cusCom,cusProfit;

	public Collider ShopDoor, ClosetDoor, PcTrigger;

	// Use this for initialization
	void Start () 
	{
		endDayOffice.SetActive (true);
		endDayOffA.SetBool ("EndShopO", true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void BuildModeUIOpen()
	{
		buildmode.SetActive (true);
		buildM.SetBool ("BuildO", true);
		ShopDoor.enabled = false;
		ClosetDoor.enabled = false;
		PcTrigger.enabled = false;
	}
	public void BuildModeUIClose()
	{
		
		buildM.SetBool ("BuildO", false);
		ShopDoor.enabled = true;
		ClosetDoor.enabled = true;
		PcTrigger.enabled = true;
		StartCoroutine (BuildClose ());

	}
	IEnumerator BuildClose()
	{
		yield return new WaitForSeconds(1);
		buildmode.SetActive (false);
	}

	public void EndDayOffice()
	{
		endDayOffice.SetActive (true);
		profit.SetText (GameMaster.Instance.Player.Business.GetProfits ().ToString());
		orderComplete.SetText (GameMaster.Instance.OrderManager.CountCompletedToday.ToString());
		orderFailed.SetText (GameMaster.Instance.OrderManager.CountFailedToday.ToString ());

		endDayOffA.SetBool ("EndOfficeO", true);
		//set varibles
	}
	public void EndDayShop()
	{
		endDayOffice.SetActive (true);
		endDayOffA.SetBool ("EndShopO", true);
		//set varibles
	}
	public void NextDayBtn()
	{
		if (GameMaster.Instance.ShopUnlocked == false) 
		{
			GameMaster.Instance.GameModeManager.ChangeGameMode(GameMode.Office);
		}
		else 
		{
			confirmDay.SetActive (true);
		}
	}

}
