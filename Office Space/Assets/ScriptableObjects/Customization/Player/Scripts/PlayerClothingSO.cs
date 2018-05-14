using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Clothing", menuName = "Player Customization/Clothing")]
public class PlayerClothingSO : ScriptableObject
{
    public string Name;
    public PlayerClothingSlotSO ClothingSlot;

    /// <summary>
    /// Index 0 = Clothing mesh, Index 1 = Body mesh.
    /// </summary>
    public Mesh[] Meshes;
}
