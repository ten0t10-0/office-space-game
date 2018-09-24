using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeOffice : MonoBehaviour
{
    [HideInInspector]
    public GameMode GameMode = GameMode.Office;

    #region <FIELDS>
    [HideInInspector]
    public float ChanceNextOrder;

    [Range(0, 11)]
    public int DayStartHour = 8;
    [Range(12, 23)]
    public int DayEndHour = 20;

    /// <summary>
    /// Number of minutes that pass every second
    /// </summary>
    public int GameTimeSpeed = 5;
    #endregion

    #region <METHODS>

    public void GameTimeUpdate()
    {
        DifficultySO difficultySetting = GameMaster.Instance.GetDifficultySetting();
        OrderManager orderManager = GameMaster.Instance.OrderManager;
        Player player = GameMaster.Instance.Player;

        if (!GameMaster.Instance.DayEnd)
        {
            if (difficultySetting.GenerateOrders)
            {
                if (orderManager.CountOpen < difficultySetting.MaxSimultaneousOpenOrders)
                {
                    //Debug.Log(chanceNextOrder.ToString());

                    if (GameMaster.Roll(ChanceNextOrder))
                    {
                        if (GameMaster.Roll(player.Business.CustomerTolerance_Total))
                        {
                            //***
                            orderManager.GenerateOrder();

                            Debug.Log("*ORDER GENERATED*");

                            //Debug.Log("ORDER:");
                            //foreach (OrderItem item in OrderManager.Orders[OrderManager.Orders.Count - 1].Items)
                            //    Debug.Log(string.Format("{0} x {1}", item.Quantity.ToString(), item.Name));
                        }
                        else
                        {
                            //TEMP:
                            Debug.Log("*TOLERANCE*");
                        }

                        ChanceNextOrder = difficultySetting.OrderGenerationRate;
                    }
                    else
                    {
                        ChanceNextOrder += difficultySetting.OrderGenerationRate; //keep increasing chance, otherwise player could potentially wait forever :p
                    }
                }
            }
        }

        #region <Close overdue orders> ***
        for (int i = 0; i < orderManager.Orders.Count; i++)
        {
            if (orderManager.Orders[i].Open)
            {
                if (GameMaster.Instance.GameDateTime >= orderManager.Orders[i].DateDue)
                    CancelOrder(i);
            }
        }
        #endregion
    }

    #region <SALES METHOD(S)>
    // SalePlayerToOnlinePlayer
    // SaleOnlinePlayerToPlayer
    // ^ Both would have to work hand-in-hand?

    /// <summary>
    /// Executes a sale where the player purchases items from a supplier. (VALIDATION REQUIRED: Player money and Player inventory space)
    /// </summary>
    /// <param name="iSupplier">The Supplier (index).</param>
    /// <param name="iSupplierItem">The Supplier's Item (index).</param>
    /// <param name="quantity">The quantity to be purchased.</param>
    /// <param name="performValidation">Set to false only if Player money AND Player inventory space has already been validated.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool SaleSupplierToPlayer(int iSupplier, int iSupplierItem, int quantity, bool performValidation, out string result)
    {
        Player player = GameMaster.Instance.Player;
        SupplierManager supplierManager = GameMaster.Instance.SupplierManager;

        bool successful = false;
        result = GameMaster.MSG_ERR_DEFAULT;

        if (iSupplier < supplierManager.Suppliers.Count)
        {
            if (iSupplierItem < supplierManager.Suppliers[iSupplier].Inventory.Items.Count)
            {
                OrderItem item = new OrderItem(supplierManager.Suppliers[iSupplier].Inventory.Items[iSupplierItem], quantity);
                float discount = supplierManager.Suppliers[iSupplier].DiscountPercentage;

                successful = player.Business.PurchaseItem(item, discount, performValidation, out result);
            }
            else
                Debug.Log("*INVALID SUPPLIER ITEM INDEX.");
        }
        else
            Debug.Log("*INVALID SUPPLIER INDEX.");

        return successful;
    }
    #endregion

    #region <ORDER METHODS>
    /// <summary>
    /// Complete the specified order with the specified dictionary of items. (Quantities must be validated before this method is called)
    /// </summary>
    /// <param name="iOrderToComplete">The index of the order to complete.</param>
    /// <param name="itemQuantities">Dictionary containing ItemID's as the key, and the quantity as the value.</param>
    public void CompleteOrder(int iOrderToComplete, Dictionary<int, int> itemQuantities, out float payment, out string result)
    {
        OrderManager orderManager = GameMaster.Instance.OrderManager;
        Player player = GameMaster.Instance.Player;
        AchievementManager achievementManager = GameMaster.Instance.AchievementManager;

        Debug.Log("*** Items inserted by player:");
        if (itemQuantities.Count > 0)
        {
            foreach (int key in itemQuantities.Keys)
            {
                Debug.Log(string.Format("{0} x '{1}'", itemQuantities[key], GameMaster.Instance.ItemManager.GetItemSO(key).Name));
            }
        }
        else
        {
            Debug.Log("<NONE>");
        }
        Debug.Log("*** ^^^^");

        result = GameMaster.MSG_ERR_DEFAULT;

        int score;
        float penaltyMult;

        List<OrderItem> items = new List<OrderItem>();

        result = "";
        string tempResult;

        foreach (int itemID in itemQuantities.Keys)
        {
            items.Add(new OrderItem(itemID, itemQuantities[itemID]));

            for (int iPlayerItem = 0; iPlayerItem < player.Business.WarehouseInventory.Items.Count; iPlayerItem++)
            {
                if (player.Business.WarehouseInventory.Items[iPlayerItem].ItemID == itemID)
                {
                    if (itemQuantities[itemID] < player.Business.WarehouseInventory.Items[iPlayerItem].Quantity)
                    {
                        player.Business.WarehouseInventory.Items[iPlayerItem].ReduceQuantity(itemQuantities[itemID], false, out tempResult);
                    }
                    else
                    {
                        player.Business.WarehouseInventory.RemoveItem(iPlayerItem, out tempResult);
                    }

                    result += tempResult + "; ";
                }
            }
        }

        orderManager.CompleteOrder(iOrderToComplete, items, GameMaster.Instance.GameDateTime, player.Business.GetTotalMarkup(), out payment, out score, out penaltyMult);

        player.Business.IncreaseMoney(payment);
        player.IncreaseExperience(score);

        Debug.Log("Order score: " + score.ToString());

        if (score > 0)
        {
            if (GameMaster.Instance.GetDifficultySetting().IncludeCustomerTolerance)
                player.Business.CustomerTolerance = Mathf.Clamp(player.Business.CustomerTolerance + (GameMaster.Instance.GetDifficultySetting().CustomerToleranceIncrement * penaltyMult), 0f, 1f);

            orderManager.CountCompleted++;
            orderManager.CountCompletedToday++;
            achievementManager.CheckAchievementsByType(AchievementType.OrdersCompleted);
        }
        else
        {
            if (GameMaster.Instance.GetDifficultySetting().IncludeCustomerTolerance)
                player.Business.CustomerTolerance = Mathf.Clamp(player.Business.CustomerTolerance - (GameMaster.Instance.GetDifficultySetting().CustomerToleranceDecrement), 0f, 1f);

            orderManager.CountFailed++;
            orderManager.CountFailedToday++;
            achievementManager.CheckAchievementsByType(AchievementType.OrdersFailed);
        }

        orderManager.CountOpen--;
    }

    /// <summary>
    /// Cancels (Fails) an order. Decreases Player Business reputation (Customer Tolerance).
    /// </summary>
    /// <param name="iOrderToCancel"></param>
    public void CancelOrder(int iOrderToCancel)
    {
        Player player = GameMaster.Instance.Player;
        GameMaster.Instance.OrderManager.CloseOrder(iOrderToCancel);

        if (GameMaster.Instance.GetDifficultySetting().IncludeCustomerTolerance)
            player.Business.CustomerTolerance = Mathf.Clamp(player.Business.CustomerTolerance - GameMaster.Instance.GetDifficultySetting().CustomerToleranceDecrement, 0f, 1f);

        Debug.Log("Tolerance now: " + player.Business.CustomerTolerance);

        GameMaster.Instance.OrderManager.CountFailed++;
        GameMaster.Instance.OrderManager.CountFailedToday++;
        GameMaster.Instance.OrderManager.CountOpen--;
        GameMaster.Instance.AchievementManager.CheckAchievementsByType(AchievementType.OrdersFailed);
    }
    #endregion

    public bool IsDayEndReady()
    {
        return GameMaster.Instance.OrderManager.GetOpenOrders().Count == 0;
    }

    #endregion
}
