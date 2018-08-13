using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierPlayer : Supplier
{
    public float Money { get; private set; }
    public float CustomerTolerance { get; set; }
    public float MarkupPercentage { get; set; }
    public InventoryPlayer WarehouseInventory { get; set; }
    public PlayerShop Shop { get; set; }

    #region <Constructor>
    public SupplierPlayer(string name, float money, float markup, float maximumInventorySpace, int shopItemSlotCount) : base(name)
    {
        Money = money;
        MarkupPercentage = markup;
        CustomerTolerance = 1f;
        WarehouseInventory = new InventoryPlayer(maximumInventorySpace);
        Shop = new PlayerShop(shopItemSlotCount);
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Executes a purchase for the specified items. Validation will check if there is enough money AND inventory space. If both are true, deducts money and adds item to inventory.
    /// </summary>
    /// <param name="item">The item(s) to purchase.</param>
    /// <param name="discount">The discount percentage on the price of the item(s).</param>
    /// <param name="performValidation">Set to false only if money AND inventory space has already been validated.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool PurchaseItem(OrderItem item, float discount, bool performValidation, out string result)
    {
        bool successful = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        float totalCost = GameMaster.DiscountPrice(item.TotalValue(), discount);
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
            DecreaseMoney(totalCost);
            WarehouseInventory.AddItem(item, false, out result);

            successful = true;
            result = string.Format("Purchase successful: {0}", result);
        }

        return successful;
    }

    /// <summary>
    /// Checks if the player has enough money to cater for the specified item cost.
    /// </summary>
    /// <param name="itemTotalCostWithDiscount">Total cost (with discount) of the item to be purchased.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool ValidatePurchaseItem(float itemTotalCostWithDiscount, out string result)
    {
        bool valid = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (itemTotalCostWithDiscount <= Money)
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
    /// Moves some or all of the specified Items to the Shop.
    /// </summary>
    /// <param name="iInventoryItem">The index of the OrderItem in the Player Inventory to be moved to the shop.</param>
    /// <param name="quantity"></param>
    /// <param name="iSlot"></param>
    /// <returns></returns>
    public void MoveItemsToShop(int iInventoryItem, int quantity, int iSlot)
    {
        string temp = GameMaster.MSG_GEN_NA;

        bool itemMoved = Shop.AddItem(WarehouseInventory.Items[iInventoryItem].ItemID, quantity, iSlot);

        if (itemMoved)
        {
            if (quantity < WarehouseInventory.Items[iInventoryItem].Quantity)
                WarehouseInventory.Items[iInventoryItem].ReduceQuantity(quantity, false, out temp);
            else
                WarehouseInventory.RemoveItem(iInventoryItem, out temp);
        }
        else
        {
            Debug.Log("***The specified Shop Item slot is already in use.");
        }
    }

    public void MoveItemsToInventory(int iSlot)
    {
        string temp = GameMaster.MSG_GEN_NA;

        OrderItem item = Shop.ItemsOnDisplay[iSlot];

        bool itemMoved = WarehouseInventory.AddItem(item, false, out temp);

        if (itemMoved)
        {
            Shop.RemoveItem(iSlot);
        }
        else
        {
            Debug.Log("***Moving items to Inventory fail: " + temp);
        }
    }

    public void IncreaseMoney(float increment)
    {
        if (increment >= 0)
        {
            Money += increment;
        }
        else
            Debug.Log("Increment amount must be a positive number!");

        GameMaster.Instance.AchievementManager.CheckAchievementsByType(AchievementType.PlayerMoney);
    }

    public void DecreaseMoney(float decrement)
    {
        if (decrement >= 0)
        {
            Money -= decrement;
        }
        else
            Debug.Log("Decrement amount must be a positive number!");
    }

    public float TotalValuation()
    {
        return WarehouseInventory.Valuation() + Shop.Valuation();
    }

    public float GetTotalMarkup()
    {
        return MarkupPercentage * CustomerTolerance;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Money: " + Money.ToString() + "; CustomerTolerance: " + CustomerTolerance.ToString();
    }
}
