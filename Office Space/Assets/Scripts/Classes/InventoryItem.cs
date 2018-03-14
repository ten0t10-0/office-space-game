using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item Item { get; set; }
    public int Quantity { get; set; }
    public float Condition { get; set; }
    public float Age { get; set; }

    #region <Constructors>
    public InventoryItem(Item item, int quantity, float condition)
    {
        Item = item;
        Quantity = quantity;
        Condition = condition;
        Age = 0f;
    }
    #endregion

    public float Value()
    {
        return Quantity * Item.UnitCost;
    }

    public float SpaceUsed()
    {
        return Quantity * Item.UnitSpace;
    }

    //TEMP:
    public override string ToString()
    {
        return Item.ToString() + "; Quantity: " + Quantity.ToString() + "; Condition: " + Condition.ToString() +"; Age: " + Age.ToString();
    }
}
