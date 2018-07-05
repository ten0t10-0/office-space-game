using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustShop : MonoBehaviour 
{
	CharacterCustomizationScript playerCus;
	ColourPicker colour;
	Button headband1,shirtLong,pantsLong,armL,armR,onesie;

	// Use this for initialization
	void Start () 
	{
		
		headband1 = transform.Find("AccessoriesPanel/Scroll View/Viewport/Content/HeadBand/Button").GetComponent<Button>();
		armL = transform.Find("AccessoriesPanel/Scroll View/Viewport/Content/ArmL/Button").GetComponent<Button>();
		armR = transform.Find("AccessoriesPanel/Scroll View/Viewport/Content/ArmR/Button").GetComponent<Button>();

		shirtLong = transform.Find("UpperPanel/Scroll View/Viewport/Content/shirt1/Button").GetComponent<Button>();

		pantsLong = transform.Find("LegsPanel/Scroll View/Viewport/Content/PantsLong/Button").GetComponent<Button>();

		onesie = transform.Find("OutfitsPanel/Scroll View/Viewport/Content/onesie/Button").GetComponent<Button>();

		headband1.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(2,ClothingSlot.Head);});
		armL.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(0,ClothingSlot.LeftArm);});
		armR.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(1,ClothingSlot.RightArm);});

		shirtLong.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(5,ClothingSlot.Upper);});
		pantsLong.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(4,ClothingSlot.Lower);});
		onesie.GetComponent<Button>().onClick.AddListener(delegate {SetClothing(3,ClothingSlot.Costume);});

		playerCus = GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ();
		colour = transform.Find ("Refresh").GetComponent<ColourPicker> ();
	}

	void SetClothing(int index,ClothingSlot slot)
	{
		playerCus.SetClothing (index);
		playerCus.UpdateClothingColor(slot,colour.textureColour);
		Debug.Log("bloooooop");
	}


	// Update is called once per frame
	void Update () {
		
	}
}
