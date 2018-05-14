using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerClothing
{
    public int ClothingID { get; set; }

    #region <Constructors>
    public PlayerClothing(int clothingId)
    {
        ClothingID = clothingId;
    }
    #endregion

    #region <Methods>
    public PlayerClothingSO GetPlayerClothingSO()
    {
        return GameMaster.Instance.CustomizationManager.Player.Clothing[ClothingID];
    }
    #endregion
}
