using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierPlayer : Supplier
{
    public float Money { get; set; }
    public InventoryPlayer Inventory { get; set; }

    #region <Constructor>
    public SupplierPlayer(string name, float money, float maximumInventorySpace) : base(name)
    {
        Money = money;
        Inventory = new InventoryPlayer(maximumInventorySpace);
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Executes a purchase for this player business.
    /// </summary>
    /// <param name="item">The item to be purchased.</param>
    /// <param name="quantity">The quantity of the item to be purchased.</param>
    /// <param name="payment">The total value of the purchase.</param>
    /// <param name="result">The result message.</param>
    /// <returns></returns>
    public override bool ExecutePurchase(Item item, int quantity, out float payment, out string result)
    {
        bool success;

        InventoryItem inventoryItem = new InventoryItem(item.ItemID, quantity);

        payment = inventoryItem.TotalValue();

        if (Money >= payment)
        {
            success = Inventory.AddItem(inventoryItem, out result);
            

            if (success)
            {
                Money -= payment;
                result = string.Format("Purchase for {0} x '{1}' completed!", quantity.ToString(), item.Name);
            }
        }
        else
        {
            success = false;
            result = string.Format("You do not have enough money to purchase these items. Purchase for {0} x '{1}' cancelled.", quantity.ToString(), item.Name);
        }

        return success;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Money: " + Money.ToString();
    }
}
