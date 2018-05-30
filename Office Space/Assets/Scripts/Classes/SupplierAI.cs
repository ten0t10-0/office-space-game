using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public InventoryAI Inventory { get; set; }

    #region <Constructors>
    public SupplierAI(string name, int level) : base(name, level)
    {
        Inventory = new InventoryAI();
    }
    #endregion
}
