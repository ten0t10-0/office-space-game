using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCustShop : MonoBehaviour 
{
	CharacterCustomizationData Char;
	CharacterCustomizationScript playerCus;
	ColourPicker colour;

	public TextMeshProUGUI price,priceD;
	public GameObject moneyD;
	public Sprite locked;
	public Sprite equipped;
	public Sprite upper, lower, access, outfit;
	public Animator shelf;

	List<int> currentOutfit;

	public Color textureColour;
	bool clicked = false;

//	Button headband1,shirtLong,pantsLong,armL,armR,onesie;

	[SerializeField] 
	private GameObject Container;

	private Transform Upperscroll;

	ClothingSlot currentSlot;

	CharacterClothingSO tempItem;

	// Use this for initialization
	void Start () 
	{
		
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
		CurrentOutfit();
		AddUpper ();
		shelf.SetBool ("Shelf1", true);
	}

	void SetClothing(int index,ClothingSlot slot)
	{
		playerCus.SetClothing (index);
		playerCus.UpdateClothingColor(slot,colour.textureColour);
	}

	public void PurchaseItems()
	{
		if (tempItem == null) 
		{
			Debug.Log ("Nothing happens");
		} 
		else 
		{
			if (GameMaster.Instance.Player.Business.Money < tempItem.Price) 
			{
				Debug.Log ("Not enough money");
			} 
			else 
			{
				CurrentOutfit();
				GameMaster.Instance.Player.Business.DecreaseMoney (tempItem.Price);
				priceD.SetText ("-$" + tempItem.Price.ToString ());
				moneyD.SetActive (true);
				Debug.Log("Item Purchased");
			}
		}
	
	}


	// Update is called once per frame
	void Update ()
	{
		if (currentSlot != null) 
		{
			playerCus.UpdateClothingColor(currentSlot,colour.textureColour);
		}
		price.SetText (GameMaster.Instance.Player.Business.Money.ToString ());
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
				newItem.transform.Find ("Button/Image").GetComponent<Image> ().sprite = upper;
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
				newItem.transform.Find ("Button/Image").GetComponent<Image> ().sprite = lower;
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
                //if (!item.Special)
              //  {
                    GameObject newItem = Instantiate(Container, Upperscroll);
                    SetItem(newItem, item);
                    newItem.transform.Find("Button/Image").GetComponent<Image>().sprite = outfit;
              //  }
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
				newItem.transform.Find ("Button/Image").GetComponent<Image> ().sprite = access;
			}
		}
	}
	void SetItem(GameObject newItem, CharacterClothingSO item)
	{
        newItem.transform.Find("Button/Image").GetComponent<Image>().sprite = outfit;
        newItem.transform.Find("Name").GetComponent<TMP_Text>().text = item.Name;
		newItem.transform.Find ("Button/Text").GetComponent<TMP_Text> ().text = "$"+item.Price.ToString ();

        if (selectedItems(item))
        {
            newItem.transform.Find("Button/Equipped").GetComponent<Image>().sprite = equipped;
            Debug.Log("Bloop");
        }

        if (item.LevelRequirement > GameMaster.Instance.Player.Level)
        {
            newItem.transform.Find("Image").GetComponent<Image>().gameObject.SetActive(true);
        }
        newItem.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { slots(item); });
    }

	void slots(CharacterClothingSO clothing)
	{
		int i = 0;
		clicked = true;
		tempItem = clothing;
		foreach (CharacterClothingSO item in GameMaster.Instance.CustomizationManager.Character.Clothing) 
		{
			if (item == clothing)
			{
				SetClothing (i, item.ClothingSlot.Slot);
				currentSlot = item.ClothingSlot.Slot;
				price.SetText(item.Price.ToString());
				break;
			}
			i++;
		}
	}
	void CurrentOutfit()
	{
    	Char = playerCus.GetCustomizationData ();
	}
	public void updateBodyColor()
	{
		playerCus.UpdateBodyColor (colour.textureColour);
	}
	public void Cancel()
	{
		playerCus.SetAppearanceByData (Char);
	}
	public void unsetClothing()
	{
		playerCus.UnsetAllClothing ();
	}

	public bool selectedItems(CharacterClothingSO item)
	{
        int iCurrent = playerCus.GetClothingIndexBySlot(item.ClothingSlot.Slot);
        CharacterClothingSO itemFound = null;

        if (iCurrent != -1)
            itemFound = GameMaster.Instance.CustomizationManager.Character.Clothing[iCurrent];


        return itemFound == item;
	}
		
	public CharacterClothingSO PurchasedItems(CharacterClothingSO item)
	{
		foreach(var purchase in GameMaster.Instance.Player.PurchasedClothing)
		{
			if (GameMaster.Instance.CustomizationManager.Character.Clothing[purchase] == item) 
			{
				return item;
			} else
				return null;
		}
		return null;
	}

	public void buttonClick()
	{
		if (clicked == true) 
		{
			clicked= false;
			playerCus.SetAppearanceByData (Char);
		}
	}

	public void shelf1()
	{
		shelf.SetBool ("Shelf1", true);
		shelf.SetBool ("Shelf2", false);
		shelf.SetBool ("Shelf3", false);
		shelf.SetBool ("Shelf4", false);
	}
	public void shelf2()
	{
		
		shelf.SetBool ("Shelf2", true);
		shelf.SetBool ("Shelf3", false);
		shelf.SetBool ("Shelf4", false);
		shelf.SetBool ("Shelf1", false);
	}
	public void shelf3()
	{
		shelf.SetBool ("Shelf3", true);
		shelf.SetBool ("Shelf1", false);
		shelf.SetBool ("Shelf2", false);
		shelf.SetBool ("Shelf4", false);
	}	
	public void shelf4()
	{
		shelf.SetBool ("Shelf3", true);
		shelf.SetBool ("Shelf1", false);
		shelf.SetBool ("Shelf2", false);
		shelf.SetBool ("Shelf4", false);
	}
	public void shelfClose()
	{
		shelf.SetBool ("Shelf3", false);
		shelf.SetBool ("Shelf1", false);
		shelf.SetBool ("Shelf2", false);
		shelf.SetBool ("Shelf4", false);
	}
	
}
