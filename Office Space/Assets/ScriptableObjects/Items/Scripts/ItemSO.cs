using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string Name;
    public ItemSubcategorySO Subcategory;
    public ItemQuality Quality;
    public float UnitCost;
    public float UnitSpace;

    public Sprite Picture;
}
