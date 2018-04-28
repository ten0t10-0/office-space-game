using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemID ItemID { get; set; }
    public int Quantity { get; set; }
    public float Age { get; set; } //in days

    private const int quantity_DEFAULT = -1;

    #region <Constructors>
    public InventoryItem(ItemID itemId, int quantity)
    {
        ItemID = itemId;
        Quantity = quantity;

        Age = 0f;
    }

    //Default quantity:
    public InventoryItem(ItemID itemId)
    {
        ItemID = itemId;

        Age = 0f;

        Quantity = quantity_DEFAULT;
    }
    #endregion

    #region <Calculated Properties>

    /// <summary>
    /// Returns the ItemSO object that this is representing.
    /// </summary>
    /// <returns></returns>
    public ItemSO GetItemSO()
    {
        return GameMaster.Instance.ItemManager.Categories[ItemID.CategoryID].Types[ItemID.TypeID].Items[ItemID.QualityID];
    }

    /// <summary>
    /// Returns a float containing the total value of these items.
    /// </summary>
    /// <returns></returns>
    public float TotalValue()
    {
        return Quantity * GetItemSO().UnitCost;
    }

    /// <summary>
    /// Returns a float containing the total inventory space these items use up.
    /// </summary>
    /// <returns></returns>
    public float TotalSpaceUsed()
    {
        return Quantity * GetItemSO().UnitSpace;
    }
    #endregion

    #region <Methods>

    /// <summary>
    /// Reduces the quantity of these items by the specified number.
    /// </summary>
    /// <param name="quantityToRemove">The quantity of items to be removed.</param>
    /// <param name="result">String containing the result message.</param>
    /// <returns></returns>
    public bool RemoveItems(int quantityToRemove, out string result)
    {
        bool itemsRemoved;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (Quantity != quantity_DEFAULT)
        {
            if (Quantity > quantityToRemove)
            {
                Quantity -= quantityToRemove;

                itemsRemoved = true;
                result = quantityToRemove.ToString() + " of these items removed! (New quantity: " + Quantity.ToString() + ")";
            }
            else
            {
                itemsRemoved = false;
                result = "Invalid quantity to be removed!";
            }
        }
        else
        {
            itemsRemoved = true;
            result = GameMaster.MSG_GEN_NA;
        }

        return itemsRemoved;
    }

    /// <summary>
    /// Sets these items to have the default quantity. (Intended for AI Suppliers only)
    /// </summary>
    public void ResetQuantity()
    {
        Quantity = quantity_DEFAULT;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return GetItemSO().ToString() + "; Quantity: " + Quantity.ToString() + "; Age: " + Age.ToString();
    }
}
