using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderItem : Item
{
    public int Quantity { get; set; }

    #region <Constructors>
    public OrderItem(int itemId, int quantity) : base(itemId)
    {
        Quantity = quantity;
    }

    public OrderItem(Item supplierItem, int quantity) : base(supplierItem.ItemID)
    {
        Quantity = quantity;
    }

    public OrderItem(string itemName, int quantity) : base(itemName)
    {
        Quantity = quantity;
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Returns the total cost of these items. (Quantity * Unit Cost)
    /// </summary>
    /// <returns></returns>
    public override float TotalValue()
    {
        return Quantity * UnitCost;
    }

    /// <summary>
    /// Returns the total capacity/space used by these items. (Quantity * Unit Space)
    /// </summary>
    /// <returns></returns>
    public override float TotalSpaceUsed()
    {
        return Quantity * UnitSpace;
    }

    /// <summary>
    /// Reduces the quantity of these items by the specified number.
    /// </summary>
    /// <param name="quantityToRemove">The quantity of items to be removed.</param>
    /// <param name="performValidation">Set to false only if quantity has already been validated.</param>
    /// <param name="result">String containing the result message.</param>
    /// <returns></returns>
    public bool ReduceQuantity(int quantityToRemove, bool performValidation, out string result)
    {
        bool itemsRemoved = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        bool valid;

        if (!performValidation)
        {
            valid = true;
        }
        else
        {
            valid = ValidateReduceQuantity(quantityToRemove, out result);
        }

        if (valid)
        {
            Quantity -= quantityToRemove;

            itemsRemoved = true;
            result = string.Format("{0} x '{1}' successfully removed! (New quantity: " + Quantity.ToString() + ")", quantityToRemove.ToString(), Name);
        }

        GameMaster.Instance.Log(result);
        return itemsRemoved;
    }

    /// <summary>
    /// Checks if the specified quantity is low enough to be deducted from the item's quantity.
    /// </summary>
    /// <param name="quantityToRemove"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool ValidateReduceQuantity(int quantityToRemove, out string result)
    {
        bool valid = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (quantityToRemove < Quantity)
        {
            valid = true;
            result = "[QUANTITY VALIDATION PASSED]";
        }
        else if (quantityToRemove == Quantity)
        {
            valid = true;
            result = "[**REMOVE ITEM METHOD!**]";
        }
        else
        {
            result = "Quantity to be removed is too high.";
        }

        return valid;
    }

    /// <summary>
    /// Returns a base class representation of this.
    /// </summary>
    /// <returns></returns>
    public Item ToItem()
    {
        return new Item(ItemID);
    }

    /// <summary>
    /// Returns a COPY of this as an Order Item.
    /// </summary>
    /// <returns></returns>
    public OrderItem Clone()
    {
        return new OrderItem(ItemID, Quantity);
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
