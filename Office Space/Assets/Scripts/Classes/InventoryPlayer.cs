using System.Collections;
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

    public bool AddItem(InventoryItem inventoryItem, out string result)
    {
        bool added;
        result = GameMaster.MSG_ERR_DEFAULT;

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

        GameMaster.Instance.Log(result);
        return itemRemoved;
    }

    /// <summary>
    /// Changes the Inventory Maximum Space to the specified number.
    /// </summary>
    /// <param name="newMaxSpaceAmount">The new Maximum Space value.</param>
    /// <param name="message">String containing the result message.</param>
    /// <returns></returns>
    public bool ChangeMaximumSpace(float newMaxSpaceAmount, out string result)
    {
        bool changed;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (newMaxSpaceAmount > 0)
        {
            float totalSpaceUsed = TotalSpaceUsed();

            if (totalSpaceUsed < newMaxSpaceAmount)
            {
                MaximumSpace = newMaxSpaceAmount;
                changed = true;
                result = "Maximum space successfully changed!";
            }
            else
            {
                changed = false;
                result = "Too many Items in Inventory!";
            }
        }
        else
        {
            changed = false;
            result = "New maximum space amount must be greater than or equal to 0.";
        }

        GameMaster.Instance.Log(result);
        return changed;
    }

    /// <summary>
    /// Increases the Inventory Maximum Space by the specified increment.
    /// </summary>
    /// <param name="newMaxSpaceIncrement">The number to add to the maximum Inventory space.</param>
    /// <param name="message">String containing the result message.</param>
    public bool IncreaseMaximumSpace(float newMaxSpaceIncrement, out string result)
    {
        bool changed;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (newMaxSpaceIncrement > -1)
        {
            MaximumSpace += newMaxSpaceIncrement;

            changed = true;
            result = "Maximum space successfully increased by " + newMaxSpaceIncrement.ToString() + "!";
        }
        else
        {
            changed = false;
            result = "Increment must be a positive number!";
        }

        GameMaster.Instance.Log(result);
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
