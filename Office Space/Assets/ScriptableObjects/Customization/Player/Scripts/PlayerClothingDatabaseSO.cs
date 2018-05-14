using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingSlot { Costume, Upper, Lower, Head, LeftArm, RightArm }

[CreateAssetMenu(fileName = "New Player Customization DB", menuName = "Player Customization/Player Customization DB")]
public class PlayerClothingDatabaseSO : ScriptableObject
{
    public Material MaterialBodyDefault;

    public List<PlayerClothingSO> Clothing;
    public List<PlayerClothingSlotSO> ClothingSlots;

    public List<int> DefaultClothingIndexes;

    public List<Material> CustomMaterials;

    private GameObject player;
    private CharacterCustomizationScript playerCustomizationScript;

    /// <summary>
    /// Sets the specified object as the player object, and binds it with default clothing.
    /// </summary>
    /// <param name="playerObject"></param>
    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
        playerCustomizationScript = player.GetComponent<CharacterCustomizationScript>();

        playerCustomizationScript.SetClothingByList(GetDefaultClothing());
    }

    /// <summary>
    /// Sets the specified object as the player object, and binds it with the specified clothing.
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="currentClothing"></param>
    public void SetPlayer(GameObject playerObject, List<PlayerClothing> currentClothing)
    {
        player = playerObject;
        playerCustomizationScript = player.GetComponent<CharacterCustomizationScript>();

        playerCustomizationScript.SetClothingByList(currentClothing);
    }

    /// <summary>
    /// Returns a list of clothing items based on the default indexes defined in GameMaster.
    /// </summary>
    /// <returns></returns>
    private List<PlayerClothing> GetDefaultClothing()
    {
        List<PlayerClothing> defaultClothing = new List<PlayerClothing>();

        foreach (int i in DefaultClothingIndexes)
        {
            defaultClothing.Add(new PlayerClothing(i));
        }

        return defaultClothing;
    }

    public PlayerClothingSlotSO GetClothingSlotSO(ClothingSlot slot)
    {
        PlayerClothingSlotSO slotSO = ClothingSlots[0];

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
