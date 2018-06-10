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
		//AddItems ();
	}
	void Update () 
	{
		AddItems ();
	}
	public void AddItems()
	{

		ClearInventory ();

		foreach (OrderItem item in GameMaster.Instance.Player.Business.WarehouseInventory.Items) 
		{
			GameObject newItem = Instantiate (itemContainer, scrollViewContent);
			newItem.transform.Find("Panel/Name").GetComponent<TMP_Text>().text = item.Name;
			newItem.transform.Find ("Image").GetComponent<Image> ().sprite = item.Picture;
			newItem.transform.Find("qty").GetComponent<TMP_Text>().text = item.Quantity.ToString();
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
