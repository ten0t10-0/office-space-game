using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public InventoryAI Inventory { get; set; }
    public float MarkupPercentage { get; private set; }

    public float AcceptedMarkup { get; private set; }

    protected const float markupPercentage_DEFAULT = 0.00f;

    #region <Constructors>
    public SupplierAI(string name, float markupPercent) : base(name)
    {
        Inventory = new InventoryAI();

        SetMarkup(markupPercent);
    }

    //***(TEMP)
    public SupplierAI(string name) : base(name)
    {
        Inventory = new InventoryAI();

        MarkupPercentage = markupPercentage_DEFAULT;
    }
    #endregion

    #region <Methods>
    public bool PurchaseItem(Item item, out string result)
    {
        Inventory.AddItem(item, out result);

        result = string.Format("{0} successfully purchased {1}!", Name, item.Name);

        return true;
    }

    public void SetMarkup(float markupPercent)
    {
        MarkupPercentage = markupPercent;

        AcceptedMarkup = 0f; //*** TEMP
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Markup Percentage: " + MarkupPercentage.ToString();
    }
}
