using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateScrollView : MonoBehaviour 
{

	ShopController shop = new ShopController ();

	public void DefaultList()
	{
		shop.PopulateInventory ();
	}


}
