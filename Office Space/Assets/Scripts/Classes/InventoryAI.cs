using System.Collections;
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
    public bool AddItem(Item item, out string result)
    {
        bool added = false;
        bool itemFound = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (Items.Count != 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].ItemID == item.ItemID)
                {
                    itemFound = true;
                    added = false;

                    result = "Item already added.";

                    i = Items.Count; //end
                }
            }
        }

        if (!itemFound)
        {
            Items.Add(item);

            added = true;

            result = string.Format("'{0}' successfully added!", item.Name);
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
            result = "Item does not exist.";
        }

        GameMaster.Instance.Log(result);
        return itemRemoved;
    }

    public override void Clear()
    {
        Items.Clear();
    }
    #endregion

    public override string ToString()
    {
        return "-AI Inventory-";
    }
}
