using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Subcategory", menuName = "Items/Item Subcategory")]
public class ItemSubcategorySO : ScriptableObject
{
    public ItemSubcategory EnumID;
    public ItemCategorySO Category;
    public string Name;
}
