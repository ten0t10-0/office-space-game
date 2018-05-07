using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Type", menuName = "Item Type")]
public class ItemTypeSO : ScriptableObject
{
    public string Name;
    public ItemCategory Category;
}
