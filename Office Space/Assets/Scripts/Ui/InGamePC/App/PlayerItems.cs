using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour 
{
	[SerializeField]
	private GameObject itemContainer;
	private Transform scrollViewContent;

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
			GameObject newItem = Instantiate (itemContainer, scrollViewContent);
			newItem.transform.Find("Panel/Name").GetComponent<TMP_Text>().text = item.Name;
			newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
		}
	}

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
