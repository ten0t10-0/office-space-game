using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int ItemID { get; set; }

    #region <Properties>

    #region [ItemSO Wrapping]
    public string Name { get { return GetItemSO().Name; } }
    public ItemSubcategorySO Subcategory { get { return GetItemSO().Subcategory; } }
    public ItemQuality Quality { get { return GetItemSO().Quality; } }
    public float UnitCost { get { return GetItemSO().UnitCost; } }
    public float UnitSpace { get { return GetItemSO().UnitSpace; } }
    public Sprite Picture { get { return GetItemSO().Picture; } }
    #endregion

    public ItemCategorySO Category
    {
        get { return Subcategory.Category; }
    }

    public int BaseShippingTime
    {
        get { return Subcategory.BaseShippingTime; }
    }

    #endregion

    #region <Constructors>
    public Item(int itemId)
    {
        ItemID = itemId;
    }

    public Item(string itemName)
    {
        ItemID = GameMaster.Instance.ItemManager.GetItemID(itemName);
    }
    #endregion

    #region <Methods>
    private ItemSO GetItemSO()
    {
        return GameMaster.Instance.ItemManager.GetItemSO(ItemID);
    }

    /// <summary>
    /// Returns the cost per unit.
    /// </summary>
    /// <returns></returns>
    public virtual float TotalValue()
    {
        return UnitCost;
    }

    /// <summary>
    /// Returns the capacity/space used per unit.
    /// </summary>
    /// <returns></returns>
    public virtual float TotalSpaceUsed()
    {
        return UnitSpace;
    }
    #endregion

    /// <summary>
    /// Returns Item SO details.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("Name: '{0}'; Category: '{1}'; Subcategory: '{2}', Quality: {3}; UnitCost: {4}; UnitSpace: {5}", Name, Category.Name, Subcategory.Name, Quality.ToString(), UnitCost.ToString(), UnitSpace.ToString());
    }
}
