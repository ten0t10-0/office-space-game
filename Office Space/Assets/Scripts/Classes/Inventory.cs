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

    public bool AddItem(Item item, int quantity, float condition, out string message)
    {
        //*Maybe rather store on and retrieve messages from classes/GameMaster*
        message = "ITEM_NOT_ADDED.";
        bool added = false;

        float itemSpaceUsed = quantity * item.UnitSpace;
        float totalSpaceUsed = TotalSpaceUsed();

        if (totalSpaceUsed < MaximumSpace)
        {
            if (totalSpaceUsed + itemSpaceUsed < MaximumSpace)
            {
                InventoryItems.Add(new InventoryItem(item, quantity, condition));
                //message = "Item(s) successfully stored!";
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
            //message = "Maximum space successfully changed!";
        }
        else
        {
            message = "Too many Items in Inventory!";
        }

        return changed;
    }

    public void IncreaseMaximumSpace(float newMaxSpaceIncrement, out string message)
    {
        message = "SPACE_UNCHANGED.";

        MaximumSpace += newMaxSpaceIncrement;
        //message = "Maximum space successfully increased by " + newSpaceIncrement.ToString() + "!";
    }

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

    //*IDEA: Clear all items in inventory function. Scenario: sell at half price informally. etc
    public void Clear()
    {
        InventoryItems.Clear();
    }

    //TEMP:
    public override string ToString()
    {
        return "MaximumSpace: " + MaximumSpace.ToString();
    }
}
