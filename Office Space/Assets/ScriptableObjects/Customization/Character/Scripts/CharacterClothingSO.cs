using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Clothing", menuName = "Character Customization/Objects/Clothing")]
public class CharacterClothingSO : ScriptableObject
{
    public string Name;
    public float Price;
    public int LevelRequirement;
    public CharacterClothingSlotSO ClothingSlot;

    public bool Special = false;

    /// <summary>
    /// Index 0 = Clothing mesh, Index 1 = Body mesh.
    /// </summary>
    public Mesh[] Meshes;

    public Material[] CustomMaterials;

    public bool HasBodyMesh
    {
        get { return Meshes.Length == 2; }
    }

    public bool HasCustomMaterial
    {
        get { return CustomMaterials.Length != 0; }
    }
}
