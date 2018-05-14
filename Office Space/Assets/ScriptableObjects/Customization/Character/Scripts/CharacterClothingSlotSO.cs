using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Clothing Slot", menuName = "Character Customization/Clothing Slot")]
public class CharacterClothingSlotSO : ScriptableObject
{
    public string Name;

    public ClothingSlot Slot;

    public bool IsAccessory;

    public Material MaterialDefault;

    /// <summary>
    /// Index 0 = Clothing, Index 1 = Body.
    /// </summary>
    public string[] PlaceholderNames;

    public CharacterBodySO Body;
}
