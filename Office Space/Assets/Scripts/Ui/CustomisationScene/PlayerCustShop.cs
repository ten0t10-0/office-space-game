using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCustShop : MonoBehaviour 
{
	CharacterCustomizationScript playerCus;
	ColourPicker colour;

	public TextMeshProUGUI price;

	public Sprite locked;

	List<int> currentOutfit;

//	Button headband1,shirtLong,pantsLong,armL,armR,onesie;

	[SerializeField] 
	private GameObject Container;

	private Transform Upperscroll;

	// Use this for initialization
	void Start () 
	{
		AddUpper ();
//		CurrentOutfit();
//		headband1 = transform.Find("AccessoriesPanel/Scroll View/Viewport/Content/HeadBand/Button").GetComponent<Button>();
//		armL = transform.Find("AccessoriesPanel/Scroll View/Viewport/Content/ArmL/Button").GetComponent<Button>();
//		armR = transform.Find("AccessoriesPanel/Scroll View/Viewport/Content/ArmR/Button").GetComponent<Button>();
//
//		shirtLong = transform.Find("UpperPanel/Scroll View/Viewport/Content/shirt1/Button").GetComponent<Button>();
//
//		pantsLong = transform.Find("LegsPanel/Scroll View/Viewport/Content/PantsLong/Button").GetComponent<Button>();
//
//		onesie = transform.Find("OutfitsPanel/Scroll View/Viewport/Content/onesie/Button").GetComponent<Button>();
//
//		headband1.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(2,ClothingSlot.Head);});
//		armL.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(0,ClothingSlot.LeftArm);});
//		armR.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(1,ClothingSlot.RightArm);});
//
//		shirtLong.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(5,ClothingSlot.Upper);});
//		pantsLong.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(4,ClothingSlot.Lower);});
//		onesie.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(3,ClothingSlot.Costume);});
//
		playerCus = GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ();
		colour = transform.Find ("Refresh").GetComponent<ColourPicker> ();
	}

	void SetClothing(int index,ClothingSlot slot)
	{
		playerCus.SetClothing (index);
		playerCus.UpdateClothingColor(slot,colour.textureColour);
	}


	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ClearScroll()
	{
		if (Upperscroll == null)
		{
			Upperscroll = transform.Find("UpperPanel/Scroll View/Viewport/Content");
		}
		foreach (Transform child in Upperscroll)
		{
			Destroy(child.gameObject);
		}
	}	
	public void AddUpper()
	{
		ClearScroll ();

		foreach (CharacterClothingSO item in GameMaster.Instance.CustomizationManager.Character.Clothing) 
		{
			if (item.ClothingSlot.Slot == ClothingSlot.Upper) 
			{
				GameObject newItem = Instantiate (Container, Upperscroll);
				SetItem (newItem, item);
			}
		}
	}
	public void AddLower()
	{
		ClearScroll ();

		foreach (CharacterClothingSO item in GameMaster.Instance.CustomizationManager.Character.Clothing) 
		{
			if (item.ClothingSlot.Slot == ClothingSlot.Lower) 
			{
				GameObject newItem = Instantiate (Container, Upperscroll);
				SetItem (newItem, item);
			}
		}
	}
	public void AddCostume()
	{
		ClearScroll ();

		foreach (CharacterClothingSO item in GameMaster.Instance.CustomizationManager.Character.Clothing) 
		{
			if (item.ClothingSlot.Slot == ClothingSlot.Costume) 
			{
				GameObject newItem = Instantiate (Container, Upperscroll);
				SetItem (newItem, item);
			}
		}
	}
	public void AddAcess()
	{

		ClearScroll ();
		foreach (CharacterClothingSO item in GameMaster.Instance.CustomizationManager.Character.Clothing) 
		{
			if (item.ClothingSlot.Slot == ClothingSlot.LeftArm || item.ClothingSlot.Slot == ClothingSlot.RightArm || item.ClothingSlot.Slot == ClothingSlot.Head) 
			{
				GameObject newItem = Instantiate (Container, Upperscroll);
				SetItem (newItem, item);
			}
		}
	}
	void SetItem(GameObject newItem, CharacterClothingSO item)
	{

		newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = item.Name;

		if (item.LevelRequirement > GameMaster.Instance.Player.Level) 
		{
			newItem.transform.Find ("Button/Image").GetComponent<Image> ().sprite = locked;
			newItem.transform.Find ("Button").GetComponent<Button> ().enabled = false;
		}
		newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate{slots(item);});
	}

	void slots(CharacterClothingSO clothing)
	{
		int i = 0;

		foreach (CharacterClothingSO item in GameMaster.Instance.CustomizationManager.Character.Clothing) 
		{
			if (item == clothing)
			{
				SetClothing (i, item.ClothingSlot.Slot);
				price.SetText(item.Price.ToString());
				Debug.Log("Bloooooooopo");
				break;
			}
			i++;
		}
	}
//	void CurrentOutfit()
//	{
//		 currentOutfit = new List<int>();
//		foreach (CharacterClothingSlotSO slot in GameMaster.Instance.CustomizationManager.Character.ClothingSlots)
//		{
//			playerCus.GetClothingIndexBySlot(slot);
//			//currentOutfit.Add(playerCus.GetClothingIndexBySlot(item));
//		}
//			
//	}
	public void updateBodyColor()
	{
		playerCus.UpdateBodyColor (colour.textureColour);
	}
	public void Cancel()
	{
		foreach (int clothing in currentOutfit) 
		{
			if (clothing != -1) 
			{
				playerCus.SetClothing (clothing);
			}
		}
	}
	public void unsetClothing()
	{
		playerCus.UnsetAllClothing ();
		SetClothing (4, ClothingSlot.Lower);
	}
}
