using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour 
{
	
	[SerializeField]
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	SupplierManager sm = new SupplierManager();


	public void PopulateInventory()
	{

		ClearInventory();
		int i = 0;

		foreach (SupplierAI supplier in sm.Suppliers)
		{
			i++;
		
			foreach( InventoryItem item in sm.Suppliers[i].Inventory.Items)
			{
				GameObject newItem = Instantiate(ItemContainer, scrollViewContent);

				newItem.transform.localScale = Vector3.one;

				newItem.transform.Find("Image").GetComponent<Image> ().sprite = item.GetItemSO().Picture;
				newItem.transform.Find("Name").GetComponent<Text>().text = item.GetItemSO().Name;
				newItem.transform.Find("Price").GetComponent<Text>().text = item.GetItemSO().UnitCost.ToString();


			}
		}
	}


	/// Clears out any existing inventory UI items
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
