using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //*Check comments*
    public List<InventoryItem> Items { get; set; }
    public float MaximumSpace { get; private set; }

    private const float maximumSpace_DEFAULT = Mathf.Infinity;

    #region <Constructors>
    //Player Inventory
    public Inventory(float maximumSpace)
    {
        MaximumSpace = maximumSpace;
        Items = new List<InventoryItem>();
    }

    //AI Inventory
    public Inventory()
    {
        MaximumSpace = maximumSpace_DEFAULT;
        Items = new List<InventoryItem>();
    }
    #endregion

    #region <Calculated Properties>
    public float TotalSpaceUsed()
    {
        float spaceUsed = 0;

        foreach (InventoryItem item in Items)
            spaceUsed += item.TotalSpaceUsed();

        return spaceUsed;
    }

    //*Inventory Valuation: research
    public float Valuation()
    {
        float value = 0;

        foreach (InventoryItem item in Items)
            value += item.TotalValue();

        return value;
    }
    #endregion

    #region <Methods>
    //This method can be used when the player purchases items from a supplier. Just pass the supplier's InventoryItem object as a param (AFTER the desired quantity is set) and all done.
    public bool AddItem(InventoryItem inventoryItem, out string result)
    {
        return ContinueAddItem(inventoryItem, out result);
    }
    //***(TEMP)
    public bool AddItem(Item item, float condition, out string result)
    {
        return ContinueAddItem(new InventoryItem(item, condition), out result);
    }

    private bool ContinueAddItem(InventoryItem inventoryItem, out string result)
    {
        result = GameMaster.MSG_ERR_DEFAULT;
        bool added;

        if (MaximumSpace != maximumSpace_DEFAULT)
        {
            float itemSpaceUsed = inventoryItem.TotalSpaceUsed();
            float totalSpaceUsed = TotalSpaceUsed();

            if (totalSpaceUsed < MaximumSpace)
            {
                if (totalSpaceUsed + itemSpaceUsed < MaximumSpace)
                {
                    Items.Add(inventoryItem);
                    added = true;
                    result = "Item(s) successfully added!";
                }
                else
                {
                    added = false;
                    result = "You do not have enough Inventory space!";
                }
            }
            else
            {
                added = false;
                result = "Your Inventory space is currently full!";
            }
        }
        else
        {
            Items.Add(inventoryItem);
            added = true;
            result = "Item(s) successfully added!";
        }

        return added;
    }

    /// <summary>
    /// Removes the specified item from the Inventory's Items list.
    /// </summary>
    /// <param name="itemToRemoveId">The ID/index of the item in the list to remove.</param>
    /// <param name="result">String containing the result message.</param>
    /// <returns></returns>
    public bool RemoveItem(int itemToRemoveId, out string result)
    {
        bool itemRemoved;
        result = GameMaster.MSG_ERR_DEFAULT;

        try
        {
            Items.RemoveAt(itemToRemoveId);

            itemRemoved = true;
            result = "Item removed!";
        }
        catch
        {
            itemRemoved = false;
            result = "Item does not exist!";
        }

        return itemRemoved;
    }

    /// <summary>
    /// Changes the Inventory Maximum Space to the specified number.
    /// </summary>
    /// <param name="newMaxSpaceAmount">The new Maximum Space value.</param>
    /// <param name="message">String containing the result message.</param>
    /// <returns></returns>
    public bool ChangeMaximumSpace(float newMaxSpaceAmount, out string message)
    {
        bool changed;
        message = GameMaster.MSG_ERR_DEFAULT;

        if (MaximumSpace != maximumSpace_DEFAULT)
        {
            if (newMaxSpaceAmount > 0)
            {
                float totalSpaceUsed = TotalSpaceUsed();

                if (totalSpaceUsed < newMaxSpaceAmount)
                {
                    MaximumSpace = newMaxSpaceAmount;
                    changed = true;
                    //message = "Maximum space successfully changed!";
                }
                else
                {
                    changed = false;
                    //message = "Too many Items in Inventory!";
                }
            }
            else
            {
                changed = false;
                //message = "New maximum space amount must be greater than or equal to 0."
            }
        }
        else
        {
            changed = true;
            //message = GameMaster.MSG_GEN_NA;
        }

        return changed;
    }

    /// <summary>
    /// Increases the Inventory Maximum Space by the specified increment.
    /// </summary>
    /// <param name="newMaxSpaceIncrement">The (positive!*) number to add to the maximum Inventory space.</param>
    /// <param name="message">String containing the result message.</param>
    public void IncreaseMaximumSpace(float newMaxSpaceIncrement, out string message)
    {
        message = GameMaster.MSG_ERR_DEFAULT;

        if (MaximumSpace != maximumSpace_DEFAULT)
        {
            MaximumSpace += newMaxSpaceIncrement;
            message = "Maximum space successfully increased by " + newMaxSpaceIncrement.ToString() + "!";
        }
        else
        {
            message = GameMaster.MSG_GEN_NA;
        }
    }

    //*IDEA: Clear all items in inventory function. Scenario: sell at half price informally. etc
    //public void Clear()
    //{
    //    Items.Clear();
    //}
    #endregion

    //TEMP:
    public override string ToString()
    {
        return "Valuation: " + Valuation().ToString() + "; TotalSpaceUsed: " + TotalSpaceUsed().ToString() + "; MaximumSpace: " + MaximumSpace.ToString();
    }
}
