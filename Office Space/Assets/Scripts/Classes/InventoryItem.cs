using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : OrderItem
{
    public float Age { get; set; } //in days

    #region <Constructors>
    public InventoryItem(ItemID itemId, int quantity) : base(itemId, quantity)
    {
        Age = 0f;
    }
    #endregion

    #region <Methods>

    //Currently does nothing, but maybe take age into account when determining total value?
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
