using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController 
{
	
	public void PopulateInventory()
	{

		foreach (SupplierAI s in GameMaster.Instance.SupplierManager.Suppliers)
		{
		
				foreach (InventoryItem item in s.Inventory.Items)
			{
					Debug.Log(item.ToString());


			}
		}
	}


}
