using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour 
{
	public GameObject itemContainer;
	public RectTransform Content;

	// Use this for initialization
	void Start () 
	{
		AddItems ();
	}
	public void AddItems()
	{

		ClearInventory ();

		foreach (InventoryItem item in GameMaster.Instance.Player.Business.Inventory.Items) 
		{
			GameObject newItem = (GameObject)Instantiate(itemContainer);
			newItem.transform.SetParent(Content);
			newItem.transform.Find("Panel/Name").GetComponent<TMP_Text>().text = item.Name;
			newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
		}
	}

	public void ClearInventory()
	{

		foreach (Transform child in Content)
		{
			Destroy(child.gameObject);
		}
	}
		
}
