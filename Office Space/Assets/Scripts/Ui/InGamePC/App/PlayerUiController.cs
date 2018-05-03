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
	//public Image pic;

	public int max = 25;
	public int min = 1;
	float total = 0;
	int increasePerClick = 1;

	int currentAmount = 1;

	public Item purchasedItem;

	void Start()
	{
		//disable decrease btn on start up

		//GameObject sp = GameObject.Find("StockPanel");

	}

	public void SetItem(Item pi)
	{
		purchasedItem = pi;

        nameText.SetText(purchasedItem.GetItemSO().Name.ToString());
        //pic = purchasedItem.GetItemSO ().Picture;
        totalText.SetText(purchasedItem.GetItemSO().UnitCost.ToString());

        Debug.Log("booopl" + purchasedItem.ToString());
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

		total = float.Parse(amount.text) * purchasedItem.GetItemSO().UnitCost;
		totalText.SetText(total.ToString());

		// disable buttons i
		btnDecrease.interactable = currentAmount > min;
		btnIncrease.interactable = currentAmount < max;
	}



	public void BuyItemOnClick()
	{
		//deduct the money from the player


		//Update the balance in ui Somewhere

		//add the item to the players inventory

		//decrease the players inventory space

		//deactivate panel


	}

}
