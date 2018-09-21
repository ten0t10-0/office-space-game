using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeShop : MonoBehaviour
{
    [HideInInspector]
    public GameMode GameMode = GameMode.Shop;
    [HideInInspector]
    public CustomerCaptureScript CustomerCaptureScript;

    public int CountCustomersAtCounter
    {
        get
        {
            return CustomerCaptureScript.Count;
        }
    }

    #region <FIELDS>
    public int ShopItemSlotCount = 17;

    [Range(0, 11)]
    public int DayStartHour = 9;
    [Range(12, 23)]
    public int DayEndHour = 17;

    /// <summary>
    /// Number of minutes that pass every second
    /// </summary>
    public int GameTimeSpeed = 5;
    #endregion

    #region <METHODS>

    public void GameTimeUpdate()
    {

    }

    public void SaleToCustomer(Customer customer, int iSlot, float salePercent)
    {
        Player player = GameMaster.Instance.Player;

        if (player.Business.Shop.ItemsOnDisplay[iSlot] != null)
        {
            float payment = player.Business.Shop.ItemsOnDisplay[iSlot].UnitCost * salePercent;

            //Remove item from shop
            player.Business.Shop.RemoveItem(iSlot);

            //Add money
            player.Business.IncreaseMoney(payment);
        }
        else
        {
            Debug.Log("***No item in specified slot.");
        }
    }

    public bool IsDayEndReady()
    {
        return true; //*
    }
    #endregion
}
