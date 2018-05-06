using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemID ItemID { get; set; }

    #region <Constructors>
    public Item(ItemID itemId)
    {
        ItemID = itemId;
    }

    public Item(int categoryId, int typeId, int qualityId)
    {
        ItemID = new ItemID(categoryId, typeId, qualityId);
    }

    public Item(string itemIdString)
    {
        ItemID = new ItemID(itemIdString);
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Returns the ItemSO object that this is representing.
    /// </summary>
    /// <returns></returns>
    public ItemSO GetItemSO()
    {
        return GameMaster.Instance.ItemManager.Database.Categories[ItemID.CategoryID].Types[ItemID.TypeID].Items[ItemID.QualityID];
    }

    /// <summary>
    /// Returns the cost per unit.
    /// </summary>
    /// <returns></returns>
    public virtual float TotalValue()
    {
        return GetItemSO().UnitCost;
    }

    /// <summary>
    /// Returns the capacity/space used per unit.
    /// </summary>
    /// <returns></returns>
    public virtual float TotalSpaceUsed()
    {
        return GetItemSO().UnitSpace;
    }
    #endregion

    /// <summary>
    /// Returns Item SO details.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("ItemID: {0}; {1}", ItemID.ToString(), GetItemSO().ToString());
    }
}
