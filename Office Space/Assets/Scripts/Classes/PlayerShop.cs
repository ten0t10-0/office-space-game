using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShop
{
    [SerializeField]
    public OrderItem[] ItemsOnDisplay { get; set; }

    public PlayerShop(int displaySlotCount)
    {
        ItemsOnDisplay = new OrderItem[displaySlotCount];
    }

    /// <summary>
    /// ***Called from Player.Business.MoveItemsToShop()
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="quantity"></param>
    /// <param name="iSlot"></param>
    /// <returns></returns>
    public bool AddItem(int itemID, int quantity, int iSlot)
    {
        bool successful = true;

        if (ItemsOnDisplay[iSlot] == null)
        {
            ItemsOnDisplay[iSlot] = new OrderItem(itemID, quantity);
        }
        else
        {
            successful = false;
        }

        return successful;
    }

    /// <summary>
    /// ***Called from Player.Business.MoveItemsToInventory()
    /// </summary>
    /// <param name="iSlot"></param>
    public void RemoveItem(int iSlot)
    {
        ItemsOnDisplay[iSlot] = null;
    }

    public float Valuation()
    {
        float value = 0f;

        foreach (OrderItem item in ItemsOnDisplay)
        {
            if (item != null)
            {
                value += item.TotalValue();
            }
        }

        return value;
    }

    public void ClearItems()
    {
        int slotCount = ItemsOnDisplay.Length;

        ItemsOnDisplay = new OrderItem[slotCount];
    }

    public override string ToString()
    {
        return "Slot count: " + ItemsOnDisplay.Length.ToString();
    }
}
