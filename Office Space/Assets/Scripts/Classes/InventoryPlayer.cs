using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryPlayer : Inventory
{
    [SerializeField]
    public List<OrderItem> Items { get; set; }
    public float MaximumSpace { get; private set; }

    #region <Constructors>
    public InventoryPlayer(float maximumSpace)
    {
        Items = new List<OrderItem>();
        MaximumSpace = maximumSpace;
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Adds items to the player's inventory. Validation will check if there is enough inventory space.
    /// </summary>
    /// <param name="orderItem">The item(s) to add.</param>
    /// <param name="performValidation">Set to false only if Inventory space has already been validated.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool AddItem(OrderItem orderItem, bool performValidation, out string result)
    {
        bool successful = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        bool valid = true;

        if (performValidation)
        {
            valid = ValidateAddItem(orderItem.TotalSpaceUsed(), out result);
        }

        if (valid)
        {
            int iItemFound = -1;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].ItemID == orderItem.ItemID)
                {
                    iItemFound = i;

                    i = Items.Count; //end
                }
            }

            if (iItemFound == -1)
            {
                Items.Add(orderItem);
            }
            else
            {
                Items[iItemFound].Quantity += orderItem.Quantity;
            }

            successful = true;
            result = string.Format("{0} x '{1}' successfully added!", orderItem.Quantity.ToString(), orderItem.Name);
        }

        GameMaster.Instance.Log(result);
        return successful;
    }

    /// <summary>
    /// Checks if the player has enough inventory space to cater for the specified space amount.
    /// </summary>
    /// <param name="itemTotalSpaceUsed">The total space to be used by the item.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool ValidateAddItem(float itemTotalSpaceUsed, out string result)
    {
        bool valid = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        float inventoryTotalSpaceUsed = TotalSpaceUsed();

        if (inventoryTotalSpaceUsed < MaximumSpace)
        {
            if ((inventoryTotalSpaceUsed + itemTotalSpaceUsed) <= MaximumSpace)
            {
                valid = true;
                result = "[INVENTORY VALIDATION PASSED]";
            }
            else
            {
                result = "You do not have enough Inventory space.";
            }
        }
        else
        {
            result = "Your Inventory space is currently full.";
        }

        return valid;
    }

    /// <summary>
    /// Removes the specified item from the Inventory's Items list.
    /// </summary>
    /// <param name="itemToRemoveId">The ID/index of the item in the list to remove.</param>
    /// <param name="result">String containing the result message.</param>
    /// <returns></returns>
    public bool RemoveItem(int itemToRemoveId, out string result)
    {
        bool itemRemoved = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (itemToRemoveId < Items.Count)
        {
            Items.RemoveAt(itemToRemoveId);

            itemRemoved = true;
            result = "Item removed!";
        }
        else
        {
            result = "Item does not exist.";
        }

        GameMaster.Instance.Log(result);
        return itemRemoved;
    }

    /// <summary>
    /// Increases the Inventory Maximum Space by the specified increment.
    /// </summary>
    /// <param name="newMaxSpaceIncrement">The number to add to the maximum Inventory space.</param>
    /// <param name="message">String containing the result message.</param>
    public bool IncreaseMaximumSpace(float newMaxSpaceIncrement, out string result)
    {
        bool changed = true;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (newMaxSpaceIncrement > -1)
        {
            MaximumSpace += newMaxSpaceIncrement;

            result = "Maximum space successfully increased by " + newMaxSpaceIncrement.ToString() + "!";
        }
        else
        {
            changed = false;
            result = "*INVALID INCREMENT";
        }

        GameMaster.Instance.Log(result);
        return changed;
    }

    /// <summary>
    /// Returns the total space used by all items in this inventory.
    /// </summary>
    /// <returns></returns>
    public float TotalSpaceUsed()
    {
        float spaceUsed = 0;

        foreach (OrderItem item in Items)
            spaceUsed += item.TotalSpaceUsed();

        return spaceUsed;
    }

    //*Inventory Valuation: research
    public float Valuation()
    {
        float value = 0;

        foreach (OrderItem item in Items)
            value += item.TotalValue();

        return value;
    }

    /// <summary>
    /// Removes all items from the player's inventory.
    /// </summary>
    public override void ClearInventory()
    {
        Items.Clear();
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return "Valuation: " + Valuation().ToString() + "; TotalSpaceUsed: " + TotalSpaceUsed().ToString() + "; MaximumSpace: " + MaximumSpace.ToString();
    }
}
