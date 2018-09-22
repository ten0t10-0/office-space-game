using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesUI : MonoBehaviour 
{
	public Button dayC1,dayC2,dayC3,markUp,inventory1,inventory2,inventory3;
	int i = 0;
	public GameObject confirm,moneyLow,purchaseMoney,moneyLoc,moneyLowLoc;

	// Use this for initialization
	void Start () 
	{
		dayC1.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(1);});
		dayC2.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(2);});
		dayC3.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(3);});
		markUp.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(4);});
		inventory1.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(5);});
		inventory2.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(6);});
		inventory3.GetComponent<Button>().onClick.AddListener(delegate {SetConfirm(7);});
	}

	void SetConfirm(int index)
	{
		confirm.SetActive (true);
		i = index;
	}

	public void ConfirmButton()
	{
		switch (i) 
		{
		case 1:
			{
				if (GameMaster.Instance.Player.Business.Money > 1000) 
				{
					GameMaster.Instance.UpgradeManager.PurchaseActiveUpgrade (0);
					PurchaseS (1000);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		case 2:
			{
				
				if (GameMaster.Instance.Player.Business.Money > 1750) 
				{
					GameMaster.Instance.UpgradeManager.PurchaseActiveUpgrade (1);
					PurchaseS (1750);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		case 3:
			{
				if (GameMaster.Instance.Player.Business.Money > 2500) 
				{
					GameMaster.Instance.UpgradeManager.PurchaseActiveUpgrade (2);
					PurchaseS (2500);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		case 4:
			{
				if (GameMaster.Instance.Player.Business.Money > 10000) 
				{
					GameMaster.Instance.UpgradeManager.PurchaseActiveUpgrade (3);
					PurchaseS (10000);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		case 5:
			{
				if (GameMaster.Instance.Player.Business.Money > 10000) 
				{
					GameMaster.Instance.UpgradeManager.PurchasePassiveUpgrade (0);
					PurchaseS (10000);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		case 6:
			{
				if (GameMaster.Instance.Player.Business.Money > 25000) 
				{
					GameMaster.Instance.UpgradeManager.PurchasePassiveUpgrade (1);
					PurchaseS (25000);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		case 7:
			{

				if (GameMaster.Instance.Player.Business.Money > 75000) 
				{
					GameMaster.Instance.UpgradeManager.PurchasePassiveUpgrade (2);
					PurchaseS (75000);
				}
				else 
				{
					PurchaseLow ();
				}
				break;
			}
		}
	}
	
	public void CheckActive()
	{
		foreach (UpgradeActive id in GameMaster.Instance.Player.CurrentUpgradesActive) 
		{
			DisableButton (id.UpgradeID);
		}
		foreach (int id in GameMaster.Instance.Player.UnlockedUpgradesPassive) 
		{
			DisableButtonPassive (id);
		}
	}
		
	void DisableButton(int i)
	{
		switch (i) 
		{
		case 0:
			{
				dayC1.interactable = false ;
				break;
			}
		case 1:
			{
				dayC2.interactable = false ;
				break;
			}
		case 2:
			{
				dayC3.interactable = false ;
				break;
			}
		case 3:
			{
				markUp.interactable = false ;
				break;
			}
		}
	}
	void DisableButtonPassive(int i)
	{
		switch (i) 
		{
		case 0:
			{
				inventory1.interactable= false ;
				break;
			}
		case 1:
			{
				inventory2.interactable = false ;
				break;
			}
		case 2:
			{
				inventory3.interactable = false ;
				break;
			}
		}
	}

	void PurchaseLow()
	{
		GameObject pur = Instantiate (moneyLow, moneyLowLoc.transform);
		Destroy (pur, 3f);
	}
	void PurchaseS(int m)
	{
		purchaseMoney.transform.Find("MoneyPopUpText").GetComponent<TMP_Text> ().text = "- "+ m.ToString();
		GameObject pur = Instantiate (purchaseMoney, moneyLoc.transform);
		Destroy (pur, 3f);
	}

}
