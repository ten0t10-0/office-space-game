using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public InventoryAI Inventory { get; set; }
    public float MarkupPercentage { get; set; }

    protected const float markupPercentage_DEFAULT = 0.00f;

    #region <Constructors>
    public SupplierAI(string name, float markupPercent) : base(name)
    {
        Inventory = new InventoryAI();

        MarkupPercentage = markupPercent;
    }

    //***(TEMP)
    public SupplierAI(string name) : base(name)
    {
        Inventory = new InventoryAI();

        MarkupPercentage = markupPercentage_DEFAULT;
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Executes a purchase for this supplier.
    /// </summary>
    /// <param name="item">The item to be purchased.</param>
    /// <param name="quantity">The quantity of the item to be purchased.</param>
    /// <param name="payment">The total value of the purchase.</param>
    /// <param name="result">The result message.</param>
    /// <returns></returns>
    public override bool ExecutePurchase(Item item, int quantity, out float payment, out string result)
    {
        bool itemFound = false;

        if (Inventory.Items.Count != 0)
        {
            for (int i = 0; i < Inventory.Items.Count; i++)
            {
                if (Inventory.Items[i].ItemID == item.ItemID)
                {
                    itemFound = true;
                    break;
                }
            }
        }

        if (!itemFound)
            Inventory.AddItem(item, out result);

        payment = item.GetItemSO().UnitCost * quantity;
        result = "Items successfully purchased! (SUPPLIER)";
        return true;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Markup Percentage: " + MarkupPercentage.ToString();
    }
}
