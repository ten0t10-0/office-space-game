using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID
{
    public int CategoryID, TypeID, QualityID;

    public ItemID(int categoryId, int typeId, int qualityId)
    {
        CategoryID = categoryId;
        TypeID = typeId;
        QualityID = qualityId;
    }

    public ItemID(string itemId)
    {
        CategoryID = int.Parse(itemId[0].ToString());
        TypeID = int.Parse(itemId[1].ToString());
        QualityID = int.Parse(itemId[2].ToString());
    }

    public override string ToString()
    {
        return CategoryID.ToString() + TypeID.ToString() + QualityID.ToString();
    }
}
