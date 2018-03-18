using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string Name { get; set; }
    public ItemCategory Category { get; set; }
    public ItemQuality Quality { get; set; }  //Low-end, Medium-end, High-end
    public float UnitCost { get; set; }
    public float UnitSpace { get; set; }

    #region <Constructors>
    public Item (string name, ItemCategory itemCategory, ItemQuality itemQuality, float unitCost, float unitSpace)
    {
        Name = name;
        Category = itemCategory;
        Quality = itemQuality;
        UnitCost = unitCost;
        UnitSpace = unitSpace;
    }

    //Generic Item?
    //public Item(string name, float unitCost, float unitSpace)
    //{
    //    Name = name;
    //    Category = ItemCategory.None;
    //    Quality = ItemQuality.None;
    //    UnitCost = unitCost;
    //    UnitSpace = unitSpace;
    //}
    #endregion

    //TEMP:
    public override string ToString()
    {
        return "Name: " + Name + "; Category: " + Category.ToString() + "; Quality: " + Quality.ToString() + "; UnitCost: " + UnitCost.ToString() + "; UnitSpace: " + UnitSpace.ToString();
    }
}
