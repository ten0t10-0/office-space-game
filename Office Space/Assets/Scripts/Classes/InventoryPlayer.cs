﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryPlayer : Inventory
{
    [SerializeField]
    public List<InventoryItem> Items { get; set; }
    public float MaximumSpace { get; private set; }

    #region <Constructors>
    public InventoryPlayer(float maximumSpace)
    {
        Items = new List<InventoryItem>();
        MaximumSpace = maximumSpace;
    }
    #endregion

    #region <Methods>
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

    //This method can be used when the player purchases items from a supplier. Just pass the supplier's InventoryItem object as a param (AFTER the desired quantity is set) and all done.
    public bool AddItem(ItemID itemId, int quantity)
    {
        bool added;
        string result = GameMaster.MSG_ERR_DEFAULT;

        InventoryItem inventoryItem = new InventoryItem(itemId, quantity);

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

        GameMaster.Instance.Log(result);
        return added;
    }

    /// <summary>
    /// Removes the specified item from the Inventory's Items list.
    /// </summary>
    /// <param name="itemToRemoveId">The ID/index of the item in the list to remove.</param>
    /// <param name="result">String containing the result message.</param>
    /// <returns></returns>
    public bool RemoveItem(int itemToRemoveId)
    {
        bool itemRemoved;
        string result = GameMaster.MSG_ERR_DEFAULT;

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

        GameMaster.Instance.Log(result);
        return itemRemoved;
    }

    /// <summary>
    /// Changes the Inventory Maximum Space to the specified number.
    /// </summary>
    /// <param name="newMaxSpaceAmount">The new Maximum Space value.</param>
    /// <param name="message">String containing the result message.</param>
    /// <returns></returns>
    public bool ChangeMaximumSpace(float newMaxSpaceAmount)
    {
        bool changed;
        string message = GameMaster.MSG_ERR_DEFAULT;

        if (newMaxSpaceAmount > 0)
        {
            float totalSpaceUsed = TotalSpaceUsed();

            if (totalSpaceUsed < newMaxSpaceAmount)
            {
                MaximumSpace = newMaxSpaceAmount;
                changed = true;
                message = "Maximum space successfully changed!";
            }
            else
            {
                changed = false;
                message = "Too many Items in Inventory!";
            }
        }
        else
        {
            changed = false;
            message = "New maximum space amount must be greater than or equal to 0.";
        }

        GameMaster.Instance.Log(message);
        return changed;
    }

    /// <summary>
    /// Increases the Inventory Maximum Space by the specified increment.
    /// </summary>
    /// <param name="newMaxSpaceIncrement">The number to add to the maximum Inventory space.</param>
    /// <param name="message">String containing the result message.</param>
    public bool IncreaseMaximumSpace(float newMaxSpaceIncrement)
    {
        bool changed;
        string message = GameMaster.MSG_ERR_DEFAULT;

        if (newMaxSpaceIncrement > -1)
        {
            MaximumSpace += newMaxSpaceIncrement;

            changed = true;
            message = "Maximum space successfully increased by " + newMaxSpaceIncrement.ToString() + "!";
        }
        else
        {
            changed = false;
            message = "Increment must be a positive number!";
        }

        GameMaster.Instance.Log(message);
        return changed;
    }

    public override void Clear()
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