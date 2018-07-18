using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OfficeItem
{
    public int ItemID { get; set; }

    public string ObjectName { get; set; }

    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }

    public float RotationX { get; set; }
    public float RotationY { get; set; }
    public float RotationZ { get; set; }

    #region <Constructors>
    public OfficeItem(int itemId, Vector3 position, Quaternion rotation)
    {
        ItemID = itemId;

        ObjectName = null;

        UpdatePositionData(position);

        UpdateRotationData(rotation);
    }

    public OfficeItem(string essentialObjectName, Vector3 position, Quaternion rotation)
    {
        ItemID = -1;

        ObjectName = essentialObjectName;

        UpdatePositionData(position);

        UpdateRotationData(rotation);
    }
    #endregion

    #region <Methods>
    public OfficeItemSO GetSO()
    {
        if (ItemID > -1)
            return GameMaster.Instance.CustomizationManager.Office.Items[ItemID];
        else
            return null;
    }

    public void UpdatePositionData(Vector3 position)
    {
        PositionX = position.x;
        PositionY = position.y;
        PositionZ = position.z;
    }

    public void UpdateRotationData(Quaternion rotation)
    {
        Vector3 rotationEuler = rotation.eulerAngles;

        RotationX = rotationEuler.x;
        RotationY = rotationEuler.y;
        RotationZ = rotationEuler.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(PositionX, PositionY, PositionZ);
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(new Vector3(RotationX, RotationY, RotationZ));
    }
    #endregion
}
