using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterCustomizationData
{
    [SerializeField]
    public List<ClothingSlot> CurrentClothing_Keys { get; set; }
    [SerializeField]
    public List<CharacterClothing> CurrentClothing_Values { get; set; }

    [SerializeField]
    public List<CharacterAccessory> CurrentAccessories { get; set; }

    public float BodyColorR { get; set; }
    public float BodyColorG { get; set; }
    public float BodyColorB { get; set; }
    public float BodyColorA { get; set; }

    #region <Constructors>
    public CharacterCustomizationData(Color bodyColor)
    {
        CurrentClothing_Keys = new List<ClothingSlot>();
        CurrentClothing_Values = new List<CharacterClothing>();

        CurrentAccessories = new List<CharacterAccessory>();

        UpdateBodyColorInfo(bodyColor);
    }
    #endregion

    #region <Methods>
    public void AddClothingData(CharacterClothing characterClothing)
    {
        Dictionary<ClothingSlot, CharacterClothing> dictionary = GetDictionary();

        CharacterClothingSO clothingSO = characterClothing.GetClothingSO();

        if (dictionary.ContainsKey(clothingSO.ClothingSlot.Slot))
        {
            dictionary[clothingSO.ClothingSlot.Slot] = characterClothing;
        }
        else
            dictionary.Add(clothingSO.ClothingSlot.Slot, characterClothing);

        SetUpLists(dictionary);
    }

    public Dictionary<ClothingSlot, CharacterClothing> GetDictionary()
    {
        Dictionary<ClothingSlot, CharacterClothing> dictionary = new Dictionary<ClothingSlot, CharacterClothing>();

        for (int i = 0; i < CurrentClothing_Keys.Count; i++)
        {
            dictionary.Add(CurrentClothing_Keys[i], CurrentClothing_Values[i]);
        }

        return dictionary;
    }

    public void SetUpLists(Dictionary<ClothingSlot, CharacterClothing> clothingDictionary)
    {
        CurrentClothing_Keys = new List<ClothingSlot>();
        CurrentClothing_Values = new List<CharacterClothing>();

        foreach (ClothingSlot slot in clothingDictionary.Keys)
        {
            CurrentClothing_Keys.Add(slot);
            CurrentClothing_Values.Add(clothingDictionary[slot]);
        }
    }

    public void UpdateBodyColorInfo(Color bodyColor)
    {
        BodyColorR = bodyColor.r;
        BodyColorG = bodyColor.g;
        BodyColorB = bodyColor.b;
        BodyColorA = bodyColor.a;
    }

    public Color GetBodyColor()
    {
        return new Color(BodyColorR, BodyColorG, BodyColorB, BodyColorA);
    }
    #endregion
}
