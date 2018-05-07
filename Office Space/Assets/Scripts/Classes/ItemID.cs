using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemID
{
    public int CategoryID, TypeID, QualityID;

    #region <Constructors>
    public ItemID(int categoryId, int typeId, int qualityId)
    {
        CategoryID = categoryId;
        TypeID = typeId;
        QualityID = qualityId;
    }
    #endregion
}
