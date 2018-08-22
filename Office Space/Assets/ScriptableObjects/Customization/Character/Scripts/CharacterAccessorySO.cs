using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Accessory", menuName = "Character Customization/Objects/Accessories/Accessory")]
public class CharacterAccessorySO : ScriptableObject
{
    public string Name;
    public float Price;
    public int LevelRequirement;

    public GameObject GameObject;
}
