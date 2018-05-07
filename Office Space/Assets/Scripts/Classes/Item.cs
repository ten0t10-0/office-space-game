using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemID ItemID { get; set; }

    #region <Properties>
    
    #region [ItemSO Wrapping]
    //public string Description
    //{
    //    get { return GetItemSO().Description; }
    //}
    public float UnitCost
    {
        get { return GetItemSO().UnitCost; }
    }
    public float UnitSpace
    {
        get { return GetItemSO().UnitSpace; }
    }

    public Sprite Picture
    {
        get { return GetItemSO().Picture; }
    }
    #endregion

    public string Name
    {
        get { return GetItemSO().Description + ' ' + Type.name; }
    }

    public ItemCategorySO Category
    {
        get { return GameMaster.Instance.ItemManager.Database.Categories[ItemID.CategoryID]; }
    }
    public ItemTypeSO Type
    {
        get { return GameMaster.Instance.ItemManager.Database.Categories[ItemID.CategoryID].Types[ItemID.TypeID]; }
    }
    #endregion

    #region <Constructors>
    public Item(ItemID itemId)
    {
        ItemID = itemId;
    }

    public Item(int categoryId, int typeId, int qualityId)
    {
        ItemID = new ItemID(categoryId, typeId, qualityId);
    }
    #endregion

    #region <Methods>
    private ItemSO GetItemSO()
    {
        return GameMaster.Instance.ItemManager.Database.Categories[ItemID.CategoryID].Types[ItemID.TypeID].Items[ItemID.QualityID];
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
        return string.Format("Name: {0}; Category: {1}; Type: {2}, Quality: {3}; UnitCost: {4}; UnitSpace: {5}", Name, Category.name, Type.name, GetItemSO().Quality.ToString(), UnitCost.ToString(), UnitSpace.ToString());
    }
}
