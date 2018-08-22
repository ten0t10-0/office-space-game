using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Accessory Preset", menuName = "Character Customization/Objects/Accessories/Preset")]
public class CharacterAccessoryPresetSO : ScriptableObject
{
    public string Name;
    public List<int> AccessoryIDList;
}
