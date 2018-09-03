using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUiController : MonoBehaviour 
{
	public InputField amount;
	public Button btnDecrease; 
	public Button btnIncrease;
	public TextMeshProUGUI totalText;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI playerMoney;

	public GameObject buyPanel;
	public GameObject moneyA,moneyMsg,location;
	//public GameObject pic;

	public int max = 25;
	public int min = 1;
	float total = 0;
	int increasePerClick = 1;

	int currentAmount = 1;

	public Item purchasedItem;
	public int iSupplier,iItem;

	void Start()
	{
		//disable decrease btn on start up

		//GameObject sp = GameObject.Find("StockPanel");

	}
	void Awake()
	{
	}

	public void SetItem(Item pi,int iSupp,int iItm)
	{
		purchasedItem = pi;
		iSupplier = iSupp;
		iItem = iItm;

        nameText.SetText(purchasedItem.Name);

		totalText.SetText(CalculateDiscount(pi).ToString());
    }

	public void InputChanged(InputField at)
	{
		amount.text=at.ToString();
	}	

	// ChangeAmout(true) >> increase or ChangeAmount(false) >> decrease
	public void ChangeAmount(bool increase)
	{
		// clamp current value between min-max
		currentAmount = Mathf.Clamp(currentAmount + (increase ? increasePerClick : -increasePerClick), min, max);
		amount.text = currentAmount.ToString();

		total = float.Parse(amount.text) * CalculateDiscount(purchasedItem);
		totalText.SetText(total.ToString());

		// disable buttons i
		btnDecrease.interactable = currentAmount > min;
		btnIncrease.interactable = currentAmount < max;
	}

	public void BuyItemOnClick()
	{

		bool valid=false;
		float space = 0, avalibleSpace = 0;
		string result;

		total = float.Parse(amount.text) * CalculateDiscount(purchasedItem);

		space = purchasedItem.UnitSpace * currentAmount; //space item takes up

		avalibleSpace = GameMaster.Instance.Player.Business.WarehouseInventory.AvailableSpace();

		if (total > GameMaster.Instance.Player.Business.Money) 
		{
			moneyMsg.transform.Find ("noMoneyText").GetComponent<TMP_Text> ().text = "You Do Not Have Enough Money!";
			moneyMsg.SetActive (true);

			StartCoroutine(moneyErrors());
		}
		if (space > avalibleSpace) 
		{
			moneyMsg.transform.Find("noMoneyText").GetComponent<TMP_Text> ().text = "You Do Not Have Enough Space!";
			moneyMsg.SetActive (true);

			StartCoroutine(moneyErrors());
		}
			
		if ((total <= GameMaster.Instance.Player.Business.Money) && (space <= avalibleSpace))
		{
			valid = true;
			GameMaster.Instance.GetOfficeGMScript().SaleSupplierToPlayer(iSupplier, iItem, currentAmount,valid, out result);

			buyPanel.SetActive (false);

			moneyA.transform.Find("MoneyPopUpText").GetComponent<TMP_Text> ().text = "- "+ total.ToString();
			GameObject pur = Instantiate (moneyA, location.transform);
			Destroy (pur, 2f);
			//moneyA.SetActive (true);

			//StartCoroutine(moneyPopUp());
	
		}
	}

	public float CalculateDiscount(Item pi)
	{
		float itemPrice = 0;

        itemPrice = GameMaster.DiscountPrice(pi.UnitCost, GameMaster.Instance.SupplierManager.Suppliers[iSupplier].DiscountPercentage);

		return itemPrice;
			
	}
	IEnumerator moneyPopUp()
	{
		yield return new WaitForSeconds(2);
		moneyA.SetActive (false);
	}

	IEnumerator moneyErrors()
	{
		yield return new WaitForSeconds(2);
		moneyMsg.SetActive (false);
	}

//	void MoneyAnimation()
//	{
//		GameObject tempText = Instantiate (moneyA) as GameObject;
//
//		tempText.GetComponent<Animator> ().SetBool ("buy", true);
//		tempText.GetComponent<TMP_Text> ().text = "-" + total.ToString();
//
//		Destroy (tempText, 4);
//	}
}
