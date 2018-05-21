﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Items/Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> Items;
    public List<ItemCategorySO> Categories;
    public List<ItemSubcategorySO> Subcategories;
}
