using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour 
{
	
	[SerializeField]
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	
	void Start () 
	{
		AddAllItems();
	}

	public void AddAllItems()
	{
		ClearInventory();

        foreach (SupplierAI s in GameMaster.Instance.SupplierManager.Suppliers)
        {
			foreach (InventoryItem item in s.Inventory.Items)
			{
				GameObject newItem = Instantiate(ItemContainer,scrollViewContent);

				newItem.transform.Find("Image").GetComponent<Image> ().sprite = item.GetItemSO().Picture;
				newItem.transform.Find("Name").GetComponent<TMP_Text>().text = item.GetItemSO().Name.ToString();
				newItem.transform.Find("Price").GetComponent<TMP_Text>().text = item.GetItemSO().UnitCost.ToString();

				//checks if button pressed
				newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(BuyOnClick);
			}
		}
	}

	/// Clears out any existing shop UI items
	public void ClearInventory()
	{

		if (scrollViewContent == null)
		{
			scrollViewContent = transform.Find("Scroll View/Viewport/Content");
		}

		foreach (Transform child in scrollViewContent)
		{
			Destroy(child.gameObject);
		}
	}	

	public void BuyOnClick()
	{

		InventoryItem purchasedItem = GameMaster.Instance.SupplierManager.Suppliers[0].Inventory.Items.Find(x => x.GetItemSO().Name.Equals(EventSystem.current.currentSelectedGameObject.transform.parent.Find("Name").GetComponent<TMP_Text>().text));

		//Make sure we can find the item and a can afford it
		if (purchasedItem == null)
		{
			Debug.Log("Unable to find item");
			return;
		}
		else if (purchasedItem.GetItemSO().UnitCost >= GameMaster.Instance.Player.Money)
		{
			Debug.Log(string.Format("Not enough monies. Purchase Price: {0}; Player Moneyz: {1}", purchasedItem.GetItemSO().UnitCost.ToString(), GameMaster.Instance.Player.Money.ToString()));
			return;
		}
	}
}
