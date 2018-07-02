using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Clothing Slot", menuName = "Character Customization/Clothing Slot")]
public class CharacterClothingSlotSO : ScriptableObject
{
    public string Name;

    public ClothingSlot Slot;

    public Material MaterialDefault;

    public Mesh BodyMesh;
}
