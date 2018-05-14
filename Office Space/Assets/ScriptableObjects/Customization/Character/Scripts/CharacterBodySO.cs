using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Body Object", menuName = "Character Customization/Objects/Body")]
public class CharacterBodySO : ScriptableObject
{
    public string Name;
    public CharacterClothingSlotSO ClothingSlot;

    public Mesh Mesh;
}
