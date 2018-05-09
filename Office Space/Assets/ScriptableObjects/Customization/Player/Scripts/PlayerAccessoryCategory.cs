using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Accessory Category", menuName = "Player Customization/Category/Accessory Category")]
public class PlayerAccessoryCategory : ScriptableObject
{
    public string ClothingPlaceholderName;

    public Material ClothingMaterialDefault;
}
