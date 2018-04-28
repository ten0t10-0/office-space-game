using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour 
{
	
	[SerializeField]
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	


	public void PopulateInventory()
	{
	
		scrollViewContent = transform.Find("Scroll View/Viewport/Content");

        foreach (SupplierAI s in GameMaster.Instance.SupplierManager.Suppliers)
        {
		
		
			foreach (InventoryItem item in s.Inventory.Items)
			{
				GameObject newItem = Instantiate(ItemContainer,scrollViewContent);

				newItem.transform.localScale = Vector3.one;

				newItem.transform.Find("Image").GetComponent<Image> ().sprite = item.GetItemSO().Picture;
				newItem.transform.Find("Name").GetComponent<Text>().text = item.GetItemSO().Name;
				newItem.transform.Find("Price").GetComponent<Text>().text = item.GetItemSO().UnitCost.ToString();


			}
		}
	}
		

}
