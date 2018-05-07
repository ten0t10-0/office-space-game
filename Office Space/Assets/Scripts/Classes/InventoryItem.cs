﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : OrderItem
{
    public float Age { get; set; } //in days

    #region <Constructors>
    public InventoryItem(int itemId, int quantity) : base(itemId, quantity)
    {
        Age = 0f;
    }

    public InventoryItem(Item supplierItem, int quantity) : base(supplierItem.ItemID, quantity)
    {
        Age = 0f;
    }

    public InventoryItem(string itemType, string itemDescription, int quantity) : base(itemType, itemDescription, quantity)
    {
        Age = 0f;
    }
    #endregion

    #region <Methods>

    //Currently does nothing, but maybe take age into account when determining total value?
    /// <summary>
    /// Returns the total cost of these items.
    /// </summary>
    /// <returns></returns>
    public override float TotalValue()
    {
        return base.TotalValue();
    }
    #endregion

    /// <summary>
    /// Returns Item SO details, Quantity and Age.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return base.ToString() + "; Age: " + Age.ToString();
    }
}
