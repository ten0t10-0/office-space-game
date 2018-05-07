using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Category", menuName = "Item Category")]
public class ItemCategorySO : ScriptableObject
{
    public ItemTypeSO[] Types;
}
