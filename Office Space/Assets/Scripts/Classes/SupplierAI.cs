using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public InventoryAI Inventory { get; set; }
    public float DiscountPercentage { get; set; }

    #region <Constructors>
    public SupplierAI(string name) : base(name)
    {
        Inventory = new InventoryAI();
        DiscountPercentage = 0f;
    }

    public SupplierAI(string name, float discount) : base(name)
    {
        Inventory = new InventoryAI();
        DiscountPercentage = discount;
    }
    #endregion

    public override string ToString()
    {
        return base.ToString() + "; Discount Percentage: " + DiscountPercentage.ToString();
    }
}
