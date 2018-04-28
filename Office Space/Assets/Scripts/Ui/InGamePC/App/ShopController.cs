using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour 
{
	
	SupplierManager sm = new SupplierManager();

	public void PopulateInventory()
	{

		foreach (SupplierAI s in sm.Suppliers)
		{
		
				foreach (InventoryItem item in s.Inventory.Items)
			{
					Debug.Log(item.ToString());


			}
		}
	}


}
