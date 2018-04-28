using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour 
{
	
	[SerializeField]
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	
	void Start () 
	{
		PopulateInventory ();
	}

	public void PopulateInventory()
	{
		ClearInventory();

        foreach (SupplierAI s in GameMaster.Instance.SupplierManager.Suppliers)
        {
			foreach (InventoryItem item in s.Inventory.Items)
			{
				GameObject newItem = Instantiate(ItemContainer,scrollViewContent);

				Debug.Log(item.GetItemSO().Name);
				newItem.transform.Find("Image").GetComponent<Image> ().sprite = item.GetItemSO().Picture;
				newItem.transform.Find("Name").GetComponent<TMP_Text>().text = item.GetItemSO().Name.ToString();
				newItem.transform.Find("Price").GetComponent<TMP_Text>().text = item.GetItemSO().UnitCost.ToString();
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

}
