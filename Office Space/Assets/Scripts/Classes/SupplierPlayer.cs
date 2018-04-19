using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplierPlayer : Supplier
{
    #region <Constructor>
    public SupplierPlayer(string name, float maximumInventorySpace) : base(name)
    {
        Inventory = new Inventory(maximumInventorySpace);
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString();
    }
}
