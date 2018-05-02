﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryAI : Inventory
{
    [SerializeField]
    public List<Item> Items { get; set; }

    #region <Constructors>
    public InventoryAI()
    {
        Items = new List<Item>();
    }
    #endregion

    #region <Methods>
    public bool AddItem(ItemID itemId)
    {
        bool added;
        string result = GameMaster.MSG_ERR_DEFAULT;

        Items.Add(new Item(itemId));

        added = true;
        result = "Item(s) successfully added!";

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

    public override void Clear()
    {
        Items.Clear();
    }
    #endregion
}