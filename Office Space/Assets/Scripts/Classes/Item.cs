using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int ItemID { get; set; }

    #region <Properties>

    #region [ItemSO Wrapping]
    private string Description { get { return GetItemSO().Description; } }

    public ItemTypeSO Type { get { return GetItemSO().Type; } }
    public ItemQuality Quality { get { return GetItemSO().Quality; } }
    public float UnitCost { get { return GetItemSO().UnitCost; } }
    public float UnitSpace { get { return GetItemSO().UnitSpace; } }
    public Sprite Picture { get { return GetItemSO().Picture; } }
    #endregion

    public ItemCategory Category
    {
        get { return Type.Category; }
    }

    public string Name
    {
        get { return Description + ' ' + Type.Name; }
    }
    #endregion

    #region <Constructors>
    public Item(int itemId)
    {
        ItemID = itemId;
    }

    public Item(string itemType, string itemDescription)
    {
        ItemID = GameMaster.Instance.ItemManager.GetItemID(itemType, itemDescription);
    }
    #endregion

    #region <Methods>
    private ItemSO GetItemSO()
    {
        return GameMaster.Instance.ItemManager.FetchItem(ItemID);
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
        return string.Format("Name: {0}; Type: {1}; Category: {2}, Quality: {3}; UnitCost: {4}; UnitSpace: {5}", Name, Type.Name, Category.ToString(), Quality.ToString(), UnitCost.ToString(), UnitSpace.ToString());
    }
}
