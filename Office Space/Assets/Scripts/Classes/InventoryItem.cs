using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item Item { get; set; }
    public int Quantity { get; set; }
    public float Condition { get; set; } //for now, affects what is returned in the Value() method.
    public float Age { get; set; } //in days

    #region <Constructors>
    public InventoryItem(Item item, int quantity, float condition)
    {
        Item = item;
        Quantity = quantity;
        Condition = condition;
        Age = 0f;
    }
    #endregion

    #region <Calculated Properties>
    public float Value()
    {
        return Quantity * Item.UnitCost * Condition;
    }

    public float SpaceUsed()
    {
        return Quantity * Item.UnitSpace;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return Item.ToString() + "; Quantity: " + Quantity.ToString() + "; Condition: " + Condition.ToString() +"; Age: " + Age.ToString();
    }
}
