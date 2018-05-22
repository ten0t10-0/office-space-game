using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : OrderItem
{
    public float DiscountPercentage { get; set; }

    public SpecialItem(OrderItem orderItem, float discountPercent) : base(orderItem.ItemID, orderItem.Quantity)
    {
        DiscountPercentage = discountPercent;
    }

    /// <summary>
    /// Returns Item SO details, Quantity and DiscountPercentage.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return base.ToString() + "; Discount: " + DiscountPercentage.ToString();
    }
}
