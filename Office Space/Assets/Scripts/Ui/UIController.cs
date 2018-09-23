using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour 
{
	public GameObject buildmode,endDayOffice,endDayShop,GameOver,NotificationCanvas,roof,appMontior,homescreen,screensaver;
	public GameObject DebtPass,DebtGameOver,DebtLifeLine,DebtNoLifeLineU,confirmDay;// dontforget graphic raycaster
	public Animator buildM,endDayOffA,endDayShopA;

	public TextMeshProUGUI orderFailed, orderComplete,profit,cusFail,cusCom,cusProfit;

	public Collider ShopDoor, ClosetDoor, PcTrigger;
	PcTrigger pcTrig;

	// Use this for initialization
	void Awake () 
	{
		pcTrig = FindObjectOfType<PcTrigger> ();
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
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = true;
		endDayOffice.SetActive (true);
		profit.SetText (GameMaster.Instance.Player.Business.GetProfits ().ToString());
		orderComplete.SetText (GameMaster.Instance.OrderManager.CountCompletedToday.ToString());
		orderFailed.SetText (GameMaster.Instance.OrderManager.CountFailedToday.ToString ());

		endDayOffA.SetBool ("EndOfficeO", true);
		//set varibles
	}
	public void EndDayShop()
	{
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = true;
		endDayOffice.SetActive (true);
		endDayOffA.SetBool ("EndShopO", true);
		//set varibles
	}
	public void NextDayBtn()
	{
        if (!GameMaster.Instance.IsDebtDay)
        {
            if (GameMaster.Instance.ShopUnlocked == false)
            {
                NotificationCanvas.gameObject.GetComponent<GraphicRaycaster>().enabled = false;
                GameMaster.Instance.GameModeManager.ChangeGameMode(GameMode.Office);
                GameMaster.Instance.NewDay();
                endDayOffice.SetActive(false);

                if (appMontior.activeInHierarchy || homescreen.activeInHierarchy)
                {
                    homescreen.SetActive(true);
                    screensaver.SetActive(true);
                    appMontior.SetActive(false);
                    pcTrig.CloseShop();
                }
            }
            else
            {
                confirmDay.SetActive(true);

                if (endDayOffice.activeInHierarchy)
                    endDayOffice.SetActive(false);
                else
                    endDayShop.SetActive(false);
            }
        }
        else
            GameMaster.Instance.NewDayWithDebt();
	}

	public void OfficeNextDayBtn()
	{
		confirmDay.SetActive (false);
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = false;
		GameMaster.Instance.GameModeManager.ChangeGameMode(GameMode.Office);
		GameMaster.Instance.NewDay ();
		endDayOffice.SetActive (false);

		if (appMontior.activeInHierarchy || homescreen.activeInHierarchy) 
		{
			homescreen.SetActive (true);
			screensaver.SetActive (true);
			appMontior.SetActive (false);
			pcTrig.CloseShop ();
		}
	}

	public void ShopNextDayBtn()
	{
		confirmDay.SetActive (false);
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = false;
		GameMaster.Instance.GameModeManager.ChangeGameMode(GameMode.Shop);
		GameMaster.Instance.NewDay ();
		endDayShop.SetActive (false);
	}

	public void PassDebtCheck()
	{
		DebtPass.SetActive (true);
	}

	public void FailDebtGameOver()
	{
		DebtGameOver.SetActive (true);
	}

	public void DebtFailUseLifeLine()
	{
		DebtLifeLine.SetActive (true);
	}

	public void DebtFailNoLifeLineUsed()
	{
		DebtNoLifeLineU.SetActive (true);
	}

	public void GameOverScreen()
	{
		GameOver.SetActive (true);
	}

//	public void GameOverBtn()
//	{
//		
//	}

}
