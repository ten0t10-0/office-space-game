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
	public GameObject moneyA;
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

	public void SetItem(Item pi,int iSupp,int iItm)
	{
		purchasedItem = pi;
		iSupplier = iSupp;
		iItem = iItm;

        nameText.SetText(purchasedItem.Name);

		totalText.SetText(CalculateMarkUp(pi).ToString());
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

		total = float.Parse(amount.text) * CalculateMarkUp(purchasedItem);
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

		space = purchasedItem.UnitSpace * currentAmount; //space item takes up

		avalibleSpace = GameMaster.Instance.Player.Business.ShopInventory.AvailableSpace();

		if (total > GameMaster.Instance.Player.Business.Money) 
		{
			//ui not enough money!
		}
		if (space > avalibleSpace) 
		{
			//ui not enough spaace!
		}
			
		if ((total <= GameMaster.Instance.Player.Business.Money) && (space <= avalibleSpace))
		{
			valid = true;
			GameMaster.Instance.SaleSupplierToPlayer(iSupplier, iItem, currentAmount,valid, out result);

			buyPanel.SetActive (false);
			MoneyAnimation();

			playerMoney.SetText(GameMaster.Instance.Player.Business.Money.ToString());

		}
	}

	public float CalculateMarkUp(Item pi)
	{
		float itemPrice = 0;

		itemPrice = pi.UnitCost * (1 + GameMaster.Instance.SupplierManager.Suppliers[iSupplier].GetMarkup());

		return itemPrice;
			
	}

	void MoneyAnimation()
	{
		GameObject tempText = Instantiate (moneyA) as GameObject;

		tempText.GetComponent<Animator>().SetTrigger("buy");
		tempText.GetComponent<TMP_Text> ().text = "-" + total.ToString();

		Destroy (tempText, 4);
	}
}
