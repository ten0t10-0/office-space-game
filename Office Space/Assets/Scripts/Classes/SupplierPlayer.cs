using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierPlayer : Supplier
{
    public InventoryPlayer Inventory { get; set; }

    #region <Constructor>
    public SupplierPlayer(string name, float maximumInventorySpace) : base(name)
    {
        Inventory = new InventoryPlayer(maximumInventorySpace);
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString();
    }
}
