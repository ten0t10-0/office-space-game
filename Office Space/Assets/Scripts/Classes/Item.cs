using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemID ItemID { get; set; }

    #region <Constructors>
    public Item (ItemID itemId)
    {
        ItemID = itemId;
    }
    #endregion

    #region <Methods>
    /// <summary>
    /// Returns the ItemSO object that this is representing.
    /// </summary>
    /// <returns></returns>
    public ItemSO GetItemSO()
    {
        return GameMaster.Instance.ItemManager.Categories[ItemID.CategoryID].Types[ItemID.TypeID].Items[ItemID.QualityID];
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
        return GetItemSO().ToString();
    }
}
