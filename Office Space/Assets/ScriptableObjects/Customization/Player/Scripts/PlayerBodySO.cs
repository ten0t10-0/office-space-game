using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Body Object", menuName = "Player Customization/Body")]
public class PlayerBodySO : ScriptableObject
{
    public string Name;
    public PlayerClothingSlotSO ClothingSlot;

    public Mesh Mesh;
}
