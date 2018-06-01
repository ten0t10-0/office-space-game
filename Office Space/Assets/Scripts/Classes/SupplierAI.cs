using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public InventoryAI Inventory { get; set; }
    public float MarkupPercent { get; set; }

    #region <Constructors>
    public SupplierAI(string name) : base(name)
    {
        Inventory = new InventoryAI();
        MarkupPercent = 0f;
    }

    public SupplierAI(string name, float markup) : base(name)
    {
        Inventory = new InventoryAI();
        MarkupPercent = markup;
    }
    #endregion

    public override string ToString()
    {
        return base.ToString() + "; Markup Percentage: " + MarkupPercent.ToString();
    }
}
