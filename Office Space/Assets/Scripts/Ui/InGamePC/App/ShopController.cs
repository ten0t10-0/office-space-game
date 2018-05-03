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

	public GameObject qtyPanel;
	private PlayerUiController playerUI;

	[HideInInspector]
	public Item purchasedItem;

	void Start () 
	{
        playerUI = this.gameObject.GetComponent<PlayerUiController>();
		AddAllItems();
	}

	public void AddAllItems()
	{
		ClearInventory();

        int iSupplier, iItem;

        for (iSupplier = 0; iSupplier < GameMaster.Instance.SupplierManager.Suppliers.Count; iSupplier++)
        {
            SupplierAI supplier = GameMaster.Instance.SupplierManager.Suppliers[iSupplier];

            for (iItem = 0; iItem < supplier.Inventory.Items.Count; iItem++)
            {
                Item item = supplier.Inventory.Items[iItem];

                GameObject newItem = Instantiate(ItemContainer, scrollViewContent);

                newItem.GetComponent<ItemContainerScript>().SupplierIndex = iSupplier;
                newItem.GetComponent<ItemContainerScript>().ItemIndex = iItem;

                newItem.transform.Find("Image").GetComponent<Image>().sprite = item.GetItemSO().Picture;
                newItem.transform.Find("Name").GetComponent<TMP_Text>().text = item.GetItemSO().Name.ToString();
                newItem.transform.Find("Price").GetComponent<TMP_Text>().text = item.GetItemSO().UnitCost.ToString();

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
        int iSupplier, iItem;

        ItemContainerScript selected = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemContainerScript>();

        iSupplier = selected.GetComponent<ItemContainerScript>().SupplierIndex;
        iItem = selected.GetComponent<ItemContainerScript>().ItemIndex;

		purchasedItem = GameMaster.Instance.SupplierManager.Suppliers[iSupplier].Inventory.Items[iItem];
        playerUI.SetItem(purchasedItem);

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
        else
        {
            Debug.Log(string.Format("Purchase info: Item Name: {0}; Purchase Price: {1}; Player remaining Moneyz: {2}", purchasedItem.GetItemSO().Name, purchasedItem.GetItemSO().UnitCost, GameMaster.Instance.Player.Money - purchasedItem.GetItemSO().UnitCost));
        }
			
		qtyPanel.SetActive(true);

    }


}
