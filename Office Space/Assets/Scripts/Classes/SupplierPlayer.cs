using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierPlayer : Supplier
{
    public float Money { get; set; }
    public InventoryPlayer WarehouseInventory { get; set; }
    public InventoryPlayerShop ShopInventory { get; set; }

    #region <Constructor>
    public SupplierPlayer(string name, float money, float maximumInventorySpace, float maximumShopInventorySpace) : base(name)
    {
        Money = money;
        WarehouseInventory = new InventoryPlayer(maximumInventorySpace);
        ShopInventory = new InventoryPlayerShop(maximumShopInventorySpace);
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
    public bool PurchaseItem(OrderItem item, float markup, bool performValidation, out string result)
    {
        bool successful = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        float totalCost = GameMaster.MarkupPrice(item.TotalValue(), markup);
        float totalSpaceUsed = item.TotalSpaceUsed();

        bool valid = true;

        if (performValidation)
        {
            valid = ValidatePurchaseItem(totalCost, out result);

            if (valid)
            {
                valid = WarehouseInventory.ValidateAddItem(totalSpaceUsed, out result);
            }
        }

        if (valid)
        {
            Money -= totalCost;
            WarehouseInventory.AddItem(item, false, out result);

            successful = true;
            result = string.Format("Purchase successful: {0}", result);
        }

        return successful;
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

    /// <summary>
    /// Moves some or all of the specified Items to the Shop Inventory. Validation will check Shop Inventory space and the specified quantity.
    /// </summary>
    /// <param name="iInventoryItem"></param>
    /// <param name="quantity"></param>
    /// <param name="performValidation">Set to false only if Shop Inventory space AND Inventory Item quantity has already been validated.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool MoveItemsToShop(int iInventoryItem, int quantity, bool performValidation, out string result)
    {
        bool successful = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        OrderItem shopItemToBeAdded = WarehouseInventory.Items[iInventoryItem].Clone();
        shopItemToBeAdded.Quantity = quantity;

        bool valid;

        if (!performValidation)
        { valid = true; }
        else
        {
            valid = WarehouseInventory.Items[iInventoryItem].ValidateReduceQuantity(quantity, out result);

            if (valid)
                valid = ShopInventory.ValidateAddItem(shopItemToBeAdded.TotalSpaceUsed(), out result);
        }

        if (valid)
        {
            ShopInventory.AddItem(shopItemToBeAdded, false, out result);

            if (quantity < WarehouseInventory.Items[iInventoryItem].Quantity)
                WarehouseInventory.Items[iInventoryItem].ReduceQuantity(quantity, false, out result);
            else
                WarehouseInventory.RemoveItem(iInventoryItem, out result);

            successful = true;
            result = string.Format("{0} x '{1}' has successfully been moved to Shop!", shopItemToBeAdded.Quantity.ToString(), shopItemToBeAdded.Name);
        }

        return successful;
    }

    public bool MoveItemsToWarehouse(int iShopInventoryItem, int quantity, bool performValidation, out string result)
    {
        bool successful = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        OrderItem warehouseItemToBeAdded = ShopInventory.Items[iShopInventoryItem].Clone();
        warehouseItemToBeAdded.Quantity = quantity;

        bool valid;

        if (!performValidation)
        { valid = true; }
        else
        {
            valid = WarehouseInventory.ValidateAddItem(warehouseItemToBeAdded.TotalSpaceUsed(), out result);

            if (valid)
                valid = ShopInventory.Items[iShopInventoryItem].ValidateReduceQuantity(quantity, out result);
        }

        if (valid)
        {
            WarehouseInventory.AddItem(warehouseItemToBeAdded, false, out result);

            if (quantity < ShopInventory.Items[iShopInventoryItem].Quantity)
                ShopInventory.Items[iShopInventoryItem].ReduceQuantity(quantity, false, out result);
            else
                ShopInventory.RemoveItem(iShopInventoryItem, out result);

            successful = true;
            result = string.Format("{0} x '{1}' has successfully been moved back to Warehouse!", warehouseItemToBeAdded.Quantity.ToString(), warehouseItemToBeAdded.Name);
        }

        return successful;
    }

    public float TotalValuation()
    {
        return WarehouseInventory.Valuation() + ShopInventory.Valuation();
    }

    ///// <summary>
    ///// Executes a sale of the specified item(s). Validation will check if the player has enough quantities of the item available. If valid, adds money and reduces/removes item from inventory.
    ///// </summary>
    ///// <param name="iItemToSell"></param>
    ///// <param name="markup"></param>
    ///// <param name="performValidation"></param>
    ///// <param name="result"></param>
    ///// <returns></returns>
    //public bool SellItem(int iItemToSell, int quantity, float markup, bool performValidation, out string result)
    //{
    //    bool succeeded = false;
    //    result = GameMaster.MSG_ERR_DEFAULT;

    //    OrderItem item = Inventory.Items[iItemToSell].Clone();
    //    item.Quantity = quantity;

    //    float totalCost = GameMaster.MarkupPrice(item.TotalValue(), markup);

    //    bool valid;

    //    if (!performValidation)
    //    {
    //        valid = true;
    //    }
    //    else
    //    {
    //        valid = Inventory.Items[iItemToSell].ValidateReduceQuantity(quantity, out result);
    //    }

    //    if (valid)
    //    {
    //        Money += totalCost;

    //        if (quantity != Inventory.Items[iItemToSell].Quantity)
    //        {
    //            succeeded = Inventory.Items[iItemToSell].ReduceQuantity(quantity, false, out result);
    //        }
    //        else
    //        {
    //            succeeded = Inventory.RemoveItem(iItemToSell, out result);
    //        }

    //        result = string.Format("Sale successful: {0}", result);
    //    }

    //    return succeeded;
    //}
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Money: " + Money.ToString();
    }
}
