﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryPlayerShop : InventoryPlayer
{
    [SerializeField]
    public List<SpecialItem> ItemsOnSpecial { get; private set; }

    public InventoryPlayerShop(float maximumSpace) : base(maximumSpace)
    {
        ItemsOnSpecial = new List<SpecialItem>();
    }

    public void SetItemsOnSpecial(int iItem, float discountPercent, out string result)
    {
        result = GameMaster.MSG_ERR_DEFAULT;

        SpecialItem specialItem = new SpecialItem(Items[iItem], discountPercent);

        ItemsOnSpecial.Add(specialItem);
        Items.RemoveAt(iItem);

        result = string.Format("All '{0}' items are now discounted at {1}%!", specialItem.Name, (specialItem.DiscountPercentage * 100).ToString());
    }

    public void UnsetItemsOnSpecial(int iSpecialItem, out string result)
    {
        result = GameMaster.MSG_ERR_DEFAULT;

        OrderItem item = ItemsOnSpecial[iSpecialItem].Clone();

        Items.Add(item);
        ItemsOnSpecial.RemoveAt(iSpecialItem);

        result = string.Format("All '{0}' items are no longer on special!", item.Name);
    }

    public override float Valuation()
    {
        float value = base.Valuation();

        foreach (OrderItem item in ItemsOnSpecial)
            value += item.TotalValue();

        return value;
    }

    public override float TotalSpaceUsed()
    {
        float spaceUsed = base.TotalSpaceUsed();

        foreach (OrderItem item in ItemsOnSpecial)
            spaceUsed += item.TotalSpaceUsed();

        return spaceUsed;
    }

    /// <summary>
    /// Removes all items and special/discounted items from the shop inventory.
    /// </summary>
    public override void ClearInventory()
    {
        Items.Clear();
        ItemsOnSpecial.Clear();
    }
}