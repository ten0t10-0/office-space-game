using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Clothing", menuName = "Character Customization/Objects/Clothing")]
public class CharacterClothingSO : ScriptableObject
{
    public string Name;
    public CharacterClothingSlotSO ClothingSlot;

    /// <summary>
    /// Index 0 = Clothing mesh, Index 1 = Body mesh.
    /// </summary>
    public Mesh[] Meshes;
}
