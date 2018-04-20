using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateInventoryItemList {
		[MenuItem("Assets/Create/Inventory Item List")]
		public static InventoryItemList  Create()
		{
			InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

			AssetDatabase.CreateAsset(asset, "Assets/ScriptbleObject/InventoryItemList.asset");
			AssetDatabase.SaveAssets();
			return asset;
		}
}
