using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Clothing DB", menuName = "Character Customization/Database")]
public class CharacterCustomizationDatabaseSO : ScriptableObject
{
    public Material MaterialBodyDefault;

    public List<CharacterClothingSO> Clothing;
    public List<CharacterClothingSlotSO> ClothingSlots;

    public List<int> DefaultClothingIndexes;

    public List<Material> CustomMaterials;

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
}
