using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfficeUiCustomisation : MonoBehaviour {


	public TextMeshProUGUI money;
	public TextMeshProUGUI date;
	public TextMeshProUGUI time;

	[SerializeField] 
	private GameObject Container;

	private Transform scrollView;

	// Use this for initialization
	void Start () 
	{
		time.SetText (GameMaster.Instance.GameTimeString12 ());
		money.SetText((GameMaster.Instance.Player.Business.Money).ToString());

		AddFurniture ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void AddFurniture()
	{
		ClearScroll ();

		foreach (OfficeItemSO item in GameMaster.Instance.CustomizationManager.Office.Items) 
		{
			if (item.Type.Category == OfficeItemCategory.Furniture) 
			{
				GameObject newItem = Instantiate (Container, scrollView);
				SetItem (newItem, item);
			}
		}
	}

	public void ClearScroll()
	{
		if (scrollView == null)
		{
			scrollView = transform.Find("CustomisationPanel/CustomisationPanel/whiteScreenPanel/Scroll View/Viewport/Content");
		}
		foreach (Transform child in scrollView)
		{
			Destroy(child.gameObject);
		}
	}
	void SetItem(GameObject newItem, OfficeItemSO item)
	{
		newItem.transform.Find("Name").GetComponent<TMP_Text> ().text = item.Name;
		newItem.transform.Find("PriceText").GetComponent<TMP_Text> ().text = "$" + item.Price.ToString();

	}
}
