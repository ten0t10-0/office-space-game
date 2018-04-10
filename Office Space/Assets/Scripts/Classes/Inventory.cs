using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //*Check comments*
    public List<InventoryItem> InventoryItems { get; set; }
    public float MaximumSpace { get; private set; }

    private const float maximumSpace_DEFAULT = Mathf.Infinity;

    #region <Constructors>
    //Player Inventory
    public Inventory(float maximumSpace)
    {
        MaximumSpace = maximumSpace;
        InventoryItems = new List<InventoryItem>();
    }

    //AI Inventory
    public Inventory()
    {
        MaximumSpace = maximumSpace_DEFAULT;
        InventoryItems = new List<InventoryItem>();
    }
    #endregion

    #region <Calculated Properties>
    public float TotalSpaceUsed()
    {
        float spaceUsed = 0;

        foreach (InventoryItem item in InventoryItems)
            spaceUsed += item.SpaceUsed();

        return spaceUsed;
    }

    //*Inventory Valuation: research
    public float Valuation()
    {
        float value = 0;

        foreach (InventoryItem item in InventoryItems)
            value += item.Value();

        return value;
    }
    #endregion

    #region <Methods>
    public bool AddItem(Item item, int quantity, float condition, out string message)
    {
        message = "ITEM_NOT_ADDED.";
        bool added = false;

        float itemSpaceUsed = quantity * item.UnitSpace;
        float totalSpaceUsed = TotalSpaceUsed();

        if (totalSpaceUsed < MaximumSpace)
        {
            if (totalSpaceUsed + itemSpaceUsed < MaximumSpace)
            {
                InventoryItems.Add(new InventoryItem(item, quantity, condition));
                added = true;
                message = "Item(s) successfully stored!";
            }
            else
                message = "You do not have enough Inventory space to accomodate this order!";
        }
        else
        {
            message = "Your Inventory space is currently full!";
        }

        return added;
    }

    //This method can be used when the player purchases items from a supplier. Just pass the supplier's InventoryItem object as a param and all done.
    public bool AddItem(InventoryItem inventoryItem, out string message)
    {
        message = "ITEM_NOT_ADDED.";
        bool added = false;

        float itemSpaceUsed = inventoryItem.SpaceUsed();
        float totalSpaceUsed = TotalSpaceUsed();

        if (totalSpaceUsed < MaximumSpace)
        {
            if (totalSpaceUsed + itemSpaceUsed < MaximumSpace)
            {
                InventoryItems.Add(inventoryItem);
                added = true;
                message = "Item(s) successfully stored!";
            }
            else
                message = "You do not have enough Inventory space to accomodate this order!";
        }
        else
        {
            message = "Your Inventory space is currently full!";
        }

        return added;
    }

    public bool ChangeMaximumSpace(float newMaxSpaceAmount, out string message)
    {
        message = "SPACE_UNCHANGED.";
        bool changed = false;

        float totalSpaceUsed = TotalSpaceUsed();

        if (totalSpaceUsed < newMaxSpaceAmount)
        {
            MaximumSpace = newMaxSpaceAmount;
            changed = true;
            //message = "Maximum space successfully changed!";
        }
        else
        {
            //message = "Too many Items in Inventory!";
        }

        return changed;
    }

    public void IncreaseMaximumSpace(float newMaxSpaceIncrement, out string message)
    {
        message = "SPACE_UNCHANGED.";

        MaximumSpace += newMaxSpaceIncrement;
        //message = "Maximum space successfully increased by " + newSpaceIncrement.ToString() + "!";
    }

    //*IDEA: Clear all items in inventory function. Scenario: sell at half price informally. etc
    public void Clear()
    {
        InventoryItems.Clear();
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return "Valuation: " + Valuation().ToString() + "; TotalSpaceUsed: " + TotalSpaceUsed().ToString() + "; MaximumSpace: " + MaximumSpace.ToString();
    }
}
