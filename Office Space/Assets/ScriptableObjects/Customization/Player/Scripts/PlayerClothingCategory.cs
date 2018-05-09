using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Clothing Category", menuName = "Player Customization/Category/Clothing Category")]
public class PlayerClothingCategory : ScriptableObject
{
    public string BodyPlaceholderName;
    public string ClothingPlaceholderName;

    public Material ClothingMaterialDefault;
}
