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
    /// Executes a purchase for the specified items. Validation will check if there is enough money AND inventory space. If both are true, deducts money and adds item to inventory.
    /// </summary>
    /// <param name="item">The item(s) to purchase.</param>
    /// <param name="markup">The markup percentage on the price of the item(s).</param>
    /// <param name="performValidation">Set to false only if money AND inventory space has already been validated.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool PurchaseItem(InventoryItem item, float markup, bool performValidation, out string result)
    {
        bool succeeded = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        float totalCost = item.TotalValue() * (1f + markup);
        float totalSpaceUsed = item.TotalSpaceUsed();

        bool valid;

        if (!performValidation)
        {
            valid = true;
        }
        else
        {
            valid = ValidatePurchaseItem(totalCost, out result);

            if (succeeded)
            {
                valid = Inventory.ValidateAddItem(totalSpaceUsed, out result);
            }
        }

        if (valid)
        {
            Money -= totalCost;
            succeeded = Inventory.AddItem(item, false, out result);

            result = string.Format("Purchase successful: {0}", result);
        }

        return succeeded;
    }

    /// <summary>
    /// Checks if the player has enough money to cater for the specified item cost.
    /// </summary>
    /// <param name="itemTotalCostWithMarkup">Total cost (with markup) of the item to be purchased.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool ValidatePurchaseItem(float itemTotalCostWithMarkup, out string result)
    {
        bool valid = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (itemTotalCostWithMarkup <= Money)
        {
            valid = true;
            result = "[MONEY VALIDATION PASSED]";
        }
        else
        {
            result = "You do not have enough money.";
        }

        return valid;
    }

    //public bool SellItem(int iItemToSell, float markup, bool skipValidation, out string result)
    //{
    //    bool succeeded = true;
    //    result = GameMaster.MSG_ERR_DEFAULT;

    //    if (performValidation)
    //    {

    //    }

    //    if (succeeded)
    //    {

    //    }

    //    return succeeded;
    //}

    //public bool ValidateSellItem()
    //{

    //}
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Money: " + Money.ToString();
    }
}
