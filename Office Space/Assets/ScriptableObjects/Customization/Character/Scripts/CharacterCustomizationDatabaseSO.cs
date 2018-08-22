using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Clothing DB", menuName = "Character Customization/Database")]
public class CharacterCustomizationDatabaseSO : ScriptableObject
{
    [HideInInspector]
    public Material MaterialBlank;

    public Material MaterialBodyDefault;

    public List<CharacterClothingSO> Clothing;
    public List<CharacterClothingSlotSO> ClothingSlots;

    public List<CharacterAccessorySO> Accessories;

    public List<int> DefaultClothingIndexes;
    public List<CharacterAccessoryPresetSO> AccessoryPresets;

    public int MaxAccessories = 5;

    public List<Material> CustomMaterials;

    public List<Color> SkinColors;

    private GameObject player;

    /// <summary>
    /// Sets the specified object as the player object, and binds it with the specified clothing.
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="currentClothing"></param>
    public void SetPlayer(GameObject playerObject, CharacterCustomizationData customizationData)
    {
        player = playerObject;

        player.GetComponent<CharacterCustomizationScript>().SetAppearanceByData(customizationData);
    }

    public CharacterClothingSlotSO GetClothingSlotSO(ClothingSlot slot)
    {
        CharacterClothingSlotSO slotSO = ClothingSlots[0];

        for (int i = 0; i < ClothingSlots.Count; i++)
        {
            if (ClothingSlots[i].Slot == slot)
            {
                slotSO = ClothingSlots[i];

                i = ClothingSlots.Count;
            }
        }

        return slotSO;
    }

    public List<int> GetClothingIDsBySlot(ClothingSlot slot)
    {
        List<int> clothing = new List<int>();

        for (int i = 0; i < Clothing.Count; i++)
        {
            if (Clothing[i].ClothingSlot.Slot == slot)
                clothing.Add(i);
        }

        return clothing;
    }

    public int? GetDefaultClothingBySlot(ClothingSlot slot)
    {
        int? clothingIndex = null;

        foreach (int iDefaultClothing in DefaultClothingIndexes)
            if (Clothing[iDefaultClothing].ClothingSlot.Slot == slot)
            {
                clothingIndex = iDefaultClothing;
                break;
            }

        return clothingIndex;
    }

    public int GetRandomClothingBySlot(ClothingSlot slot)
    {
        List<int> clothing = GetClothingIDsBySlot(slot);

        return clothing[Random.Range(0, clothing.Count)];
    }
}
