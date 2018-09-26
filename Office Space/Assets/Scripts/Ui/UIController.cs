using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject buildmode, endDayOffice, endDayShop, GameOver, NotificationCanvas, roof, appMontior, homescreen, screensaver, hudcanvas, orderNot;
    public GameObject DebtPass, DebtGameOver, DebtLifeLine, DebtNoLifeLineU, confirmDay;// dontforget graphic raycaster
    public GameObject newDay, newGameTutorial;
    public Animator buildM, endDayOffA, endDayShopA, orderNote, newDayA, doorShop;
    CustomerInteractionUI CustomerInteractionUI;
    ShopInventoryUi ShopInventoryUi;
    public TextMeshProUGUI orderFailed, orderComplete, profit, cusFail, cusCom, cusProfit, orderScore, orderItems, orderTimeBonus, newDateT;

    public Collider ShopDoor, ClosetDoor, PcTrigger;
    PcTrigger pcTrig;
    ServeCustomer serveCust;

    // Use this for initialization
    void Awake()
    {
        CustomerInteractionUI = FindObjectOfType<CustomerInteractionUI>();
        pcTrig = FindObjectOfType<PcTrigger>();
        ShopInventoryUi = FindObjectOfType<ShopInventoryUi>();
    }

    // Update is called once per frame
    void Update()
    {
		if (GameMaster.Instance.Player.Level < 10)
			ShopDoor.enabled = false;
    }

    public void Tutorial()
    {
        newGameTutorial.SetActive(true);
        newGameTutorial.GetComponent<TutorialNewGame>().startTrigger();
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
		GameMaster.Instance.ModeSetUI();
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = true;
		endDayOffice.SetActive (true);
		profit.SetText (GameMaster.Instance.Player.Business.GetProfits ().ToString());
		orderComplete.SetText (GameMaster.Instance.OrderManager.CountCompletedToday.ToString());
		orderFailed.SetText (GameMaster.Instance.OrderManager.CountFailedToday.ToString ());

		if (hudcanvas.activeInHierarchy)
			hudcanvas.SetActive (false);

		endDayOffA.SetBool ("EndOfficeO", true);
		//set varibles
	}
	public void EndDayShop()
	{
		GameMaster.Instance.ModeSetUI();
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = true;
		cusFail.SetText (CustomerInteractionUI.customerFailed.ToString ());
		cusCom.SetText (CustomerInteractionUI.customerServed.ToString ());
		cusProfit.SetText (GameMaster.Instance.Player.Business.GetProfits ().ToString());
		endDayShop.SetActive (true);
		endDayShopA.SetBool ("EndShopO", true);
		if (hudcanvas.activeInHierarchy)
			hudcanvas.SetActive (false);
		//set varibles
	}
	public void NextDayBtn()
	{
		if (!GameMaster.Instance.IsDebtDay) {
			if (GameMaster.Instance.ShopUnlocked == false) {
				NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = false;
                Debug.Log("NewDayBtn");
				GameMaster.Instance.GameModeManager.ChangeGameMode (GameMode.Office);
				GameMaster.Instance.NewDay ();
				endDayOffice.SetActive (false);
				GameMaster.Instance.ModeSetPlay ();
				hudcanvas.SetActive (true);

				if (appMontior.activeInHierarchy || homescreen.activeInHierarchy) {
					homescreen.SetActive (true);
					screensaver.SetActive (true);
					appMontior.SetActive (false);
					pcTrig.CloseShop ();
				}
			} else 
			{
				confirmDay.SetActive (true);

				if (endDayOffice.activeInHierarchy)
					endDayOffice.SetActive (false);
				else
					endDayShop.SetActive (false);
			}
		} 
		else 
		{
			if (endDayOffice.activeInHierarchy)
				endDayOffice.SetActive (false);
			
			if (endDayShop.activeInHierarchy)
				endDayShop.SetActive (false);
			
			GameMaster.Instance.NewDayWithDebt ();
		}
	}

	public void OfficeNextDayBtn()
	{
		confirmDay.SetActive (false);
		NotificationCanvas.gameObject.GetComponent<GraphicRaycaster> ().enabled = false;
        Debug.Log("OfficeNext");
        GameMaster.Instance.GameModeManager.ChangeGameMode(GameMode.Office);
		GameMaster.Instance.NewDay ();
		endDayOffice.SetActive (false);
		GameMaster.Instance.ModeSetPlay();
		hudcanvas.SetActive (true);

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
		GameMaster.Instance.ModeSetPlay();
		hudcanvas.SetActive (true);
	}

	public void PassDebtCheck()
	{
		DebtPass.SetActive (true);
		DebtPass.GetComponent<DebtCheckPass> ().StartUp ();
	}

	public void FailDebtGameOver()
	{
		DebtGameOver.SetActive (true);
		DebtGameOver.GetComponent<DebtGameOver> ().StartUp ();
	}

	public void DebtFailUseLifeLine()
	{
		DebtLifeLine.SetActive (true);
		DebtLifeLine.GetComponent<DebtLifeLineUsed> ().StartUp ();
	}

	public void DebtFailNoLifeLineUsed()
	{
		DebtNoLifeLineU.SetActive (true);
		DebtNoLifeLineU.GetComponent<DebtNoLifeLineUsed> ().StartUp ();
	}

	public void GameOverScreen()
	{
		GameOver.SetActive (true);
	}

//	public void GameOverBtn()
//	{
//		
//	}
	public void NewDayAfterDebt()
	{
		if (GameMaster.Instance.ShopUnlocked == false)
		{
			NotificationCanvas.gameObject.GetComponent<GraphicRaycaster>().enabled = false;
            Debug.Log("NewDayDebt");
            GameMaster.Instance.GameModeManager.ChangeGameMode(GameMode.Office);
			GameMaster.Instance.NewDay();
			endDayOffice.SetActive(false);
			GameMaster.Instance.ModeSetPlay();
			hudcanvas.SetActive (true);
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
	public void OrderNotification(string score,string item, string time)
	{
		orderItems.SetText (item);
		orderScore.SetText (score);
		orderTimeBonus.SetText (time);
		orderNot.SetActive (true);
		orderNote.SetBool ("OrderC", true);
		StartCoroutine (OrderNoti ());
	}

	IEnumerator OrderNoti()
	{
		yield return new WaitForSeconds(5);
		orderNot.SetActive (false);
	}

	public void NewDayDate()
	{
		newDateT.SetText (" "+ GameMaster.Instance.GameDateTime.Day.ToString ()+ " " + GameMaster.Instance.GameDateTime.DayOfWeek.ToString ());
		newDay.SetActive (true);
	}

	IEnumerator NewDayShow()
	{
		yield return new WaitForSeconds(5);
		newDay.SetActive (false);
	}

    public void NextDayReset()
    {
        CustomerInteractionUI.NextDayReset();
        //ShopInventoryUi.ClearInventory();
    }
	public void NewGameTutorial()
	{
		newGameTutorial.SetActive (true);
		newGameTutorial.GetComponent<TutorialNewGame> ().startTrigger ();
	}

    public void CloseShopDoor()
    {
        doorShop.SetBool("SOpen", false);
    }
}
