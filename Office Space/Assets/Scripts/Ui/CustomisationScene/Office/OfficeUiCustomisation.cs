using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfficeUiCustomisation : MonoBehaviour {


	public TextMeshProUGUI money;
	public TextMeshProUGUI date;
	public TextMeshProUGUI time;

	public GameObject tablet;

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
			scrollView = transform.Find("whiteScreenPanel/Scroll View/Viewport/Content");
		}
		foreach (Transform child in scrollView)
		{
			Destroy(child.gameObject);
		}
	}

	void SetItem(GameObject newItem, OfficeItemSO item)
	{
		newItem.transform.Find("Name").GetComponent<TMP_Text> ().text = item.Name;
		newItem.transform.Find("Button/PriceText").GetComponent<TMP_Text> ().text = "$" + item.Price.ToString();

		newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate{buyItem(item);});
	}

	void buyItem(OfficeItemSO item)
	{
		if (GameMaster.Instance.CustomizationManager.Office.MaxNumberOfObjects > GameMaster.Instance.CustomizationManager.Office.CurrentObjects.Count) 
		{
			//money and animation
			GetItem (item);
		} 
		else 
		{
			//gui to many items
			Debug.Log("too many items");
		}
	}
	void GetItem(OfficeItemSO officeitem)
	{
		int i = 0;
		int random;

		foreach (OfficeItemSO item in GameMaster.Instance.CustomizationManager.Office.Items) 
		{
			if (item == officeitem)
			{
                GameMaster.Instance.BuildMode = true;

                GameMaster.Instance.CustomizationManager.Office.InitializeOfficeObject(i, out random);
				GameMaster.Instance.CustomizationManager.Office.SelectObject (random);
				tablet.SetActive (false);
				break;
			}
			i++;
		}
	}
}
