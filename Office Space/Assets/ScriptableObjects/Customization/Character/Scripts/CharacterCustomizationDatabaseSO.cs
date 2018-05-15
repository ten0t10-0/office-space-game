using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingSlot { Costume, Upper, Lower, Head, LeftArm, RightArm }

[CreateAssetMenu(fileName = "New Char Clothing DB", menuName = "Character Customization/Database")]
public class CharacterCustomizationDatabaseSO : ScriptableObject
{
    public Material MaterialBodyDefault;

    public List<CharacterClothingSO> Clothing;
    public List<CharacterClothingSlotSO> ClothingSlots;

    public List<int> DefaultClothingIndexes;

    public List<Material> CustomMaterials;

    private GameObject player;
    private CharacterCustomizationScript playerCustomizationScript;

    /// <summary>
    /// Sets the specified object as the player object, and binds it with the specified clothing.
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="currentClothing"></param>
    public void SetPlayer(GameObject playerObject, CharacterCustomizationData customizationData)
    {
        player = playerObject;
        playerCustomizationScript = player.GetComponent<CharacterCustomizationScript>();

        playerCustomizationScript.SetAppearanceByData(customizationData);
    }

    /// <summary>
    /// Returns a list of clothing items based on the default indexes defined in GameMaster.
    /// </summary>
    /// <returns></returns>
    private List<CharacterClothing> GetDefaultClothing()
    {
        List<CharacterClothing> defaultClothing = new List<CharacterClothing>();

        foreach (int i in DefaultClothingIndexes)
        {
            defaultClothing.Add(new CharacterClothing(i));
        }

        return defaultClothing;
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
