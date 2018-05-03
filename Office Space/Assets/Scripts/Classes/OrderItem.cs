using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderItem : Item
{
    public int Quantity { get; set; }

    #region <Constructors>
    public OrderItem(ItemID itemId, int quantity) : base(itemId)
    {
        Quantity = quantity;
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Returns the total cost of these items.
    /// </summary>
    /// <returns></returns>
    public override float TotalValue()
    {
        return Quantity * GetItemSO().UnitCost;
    }

    /// <summary>
    /// Returns the total capacity/space used by these items.
    /// </summary>
    /// <returns></returns>
    public override float TotalSpaceUsed()
    {
        return Quantity * GetItemSO().UnitSpace;
    }

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

        if (Quantity > quantityToRemove)
        {
            Quantity -= quantityToRemove;

            itemsRemoved = true;
            result = quantityToRemove.ToString() + " of these items have been successfully removed! (New quantity: " + Quantity.ToString() + ")";
        }
        else
        {
            itemsRemoved = false;
            result = "Quantity to be removed is too high!";
        }

        GameMaster.Instance.Log(result);
        return itemsRemoved;
    }
    #endregion

    /// <summary>
    /// Returns Item SO details and Quantity.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return base.ToString() + "; Quantity: " + Quantity.ToString();
    }
}
