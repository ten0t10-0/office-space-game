using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  
public class Item
{
	public string Id;
	public string Name; 
	public ItemCategory Category; 
	public ItemQuality Quality;  //Low-end, Medium-end, High-end
	public float UnitCost;
	public float UnitSpace; 

	public Sprite pic; 

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
