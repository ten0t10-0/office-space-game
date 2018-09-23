using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfficeUiCustomisation : MonoBehaviour 
{
	public TextMeshProUGUI money;
	public TextMeshProUGUI date;
	public TextMeshProUGUI time,confirmName;
	public Button tables, chairs, lights, misc,station;
	public Sprite chair, lightS, other, stationary, table;

	public GameObject tablet,confirm;
	public Animator phone;

	OfficeItemSO tempItem;

	[SerializeField] 
	private GameObject Container;

	OfficeItemCategory tempCat;

	private Transform scrollView;

	// Use this for initialization
	void Start () 
	{
		time.SetText (GameMaster.Instance.GameTimeString12 ());
		money.SetText((GameMaster.Instance.Player.Business.Money).ToString());

		tables.GetComponent<Button>().onClick.AddListener(delegate {AddItems(OfficeItemCategory.Tables);});
		chairs.GetComponent<Button>().onClick.AddListener(delegate {AddItems(OfficeItemCategory.Chairs);});
		lights.GetComponent<Button>().onClick.AddListener(delegate {AddItems(OfficeItemCategory.Lights);});
		misc.GetComponent<Button>().onClick.AddListener(delegate {AddItems(OfficeItemCategory.Miscellaneous);});
		station.GetComponent<Button>().onClick.AddListener(delegate {AddItems(OfficeItemCategory.Stationery);});

		AddItems (OfficeItemCategory.Tables);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void AddItems(OfficeItemCategory cat)
	{
		ClearScroll ();

		foreach (OfficeItemSO item in GameMaster.Instance.CustomizationManager.Office.Items) 
		{
			if (item.Category == cat) 
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
		if (item.LevelRequirement > GameMaster.Instance.Player.Level) 
		{
			//newItem.transform.Find ("Button/Image").GetComponent<Image> ().sprite = locked;
		}

		newItem.transform.Find("Name").GetComponent<TMP_Text> ().text = item.Name;
		newItem.transform.Find("Button/PriceText").GetComponent<TMP_Text> ().text = "$" + item.Price.ToString();

		if(GameMaster.Instance.Player.Level < item.LevelRequirement)
		newItem.transform.Find ("Image").GetComponent<Image> ().gameObject.SetActive (true);

		if (item.Category == OfficeItemCategory.Chairs)
			newItem.transform.Find ("Button/Picture").GetComponent<Image> ().sprite = chair;
		if (item.Category == OfficeItemCategory.Lights)
			newItem.transform.Find ("Button/Picture").GetComponent<Image> ().sprite = lightS;
		if (item.Category == OfficeItemCategory.Miscellaneous)
			newItem.transform.Find ("Button/Picture").GetComponent<Image> ().sprite = other;
		if (item.Category == OfficeItemCategory.Stationery)
			newItem.transform.Find ("Button/Picture").GetComponent<Image> ().sprite = stationary;
		if (item.Category == OfficeItemCategory.Tables)
			newItem.transform.Find ("Button/Picture").GetComponent<Image> ().sprite = table;

		newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate{Confirm(item);});


	}

	public void buyItem()
	{
		if (GameMaster.Instance.CustomizationManager.Office.MaxNumberOfObjects > GameMaster.Instance.CustomizationManager.Office.CurrentObjects.Count) 
		{
			//money and animation
			if (GameMaster.Instance.Player.Business.Money > tempItem.Price) 
			{
				GetItem (tempItem);
				GameMaster.Instance.Player.Business.DecreaseMoney (tempItem.Price);
			} 
			else 
			{
				Debug.Log ("Not enough money");
			}

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
				phone.SetBool ("PhoneH", false);
				phone.SetBool ("PhoneO", false);
                GameMaster.Instance.EnableBuildMode();

                GameMaster.Instance.CustomizationManager.Office.InitializeOfficeObject(i, out random);
				GameMaster.Instance.CustomizationManager.Office.SelectObject (random);
				GameMaster.Instance.ModeSetPlay ();
				tablet.SetActive (false);

				break;
			}
			i++;
		}
	}

	public void Confirm(OfficeItemSO item)
	{
		tempItem = item;
		confirmName.SetText (item.Name);
		confirm.SetActive (true);
	}

}
