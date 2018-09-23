using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Order> Orders;

    [HideInInspector]
    public int CountOpen = 0;
    [HideInInspector]
    public int CountCompleted = 0;
    [HideInInspector]
    public int CountFailed = 0;
    [HideInInspector]
    public int CountCompletedToday = 0;
    [HideInInspector]
    public int CountFailedToday = 0;

    public List<ItemSubcategory> ExcludedItemSubcategories = new List<ItemSubcategory>
        {
            ItemSubcategory.Nothing
        };

    /// <summary>
    /// (WIP) Generates an Order based on the difficulty specified in the GameMaster instance.
    /// </summary>
    public void GenerateOrder()
    {
        DateTime currentDate = GameMaster.Instance.GameDateTime;
        DifficultySO diffSetting = GameMaster.Instance.GetDifficultySetting();

        #region [CUSTOMER]
        Customer customer = GameMaster.Instance.CustomerManager.GenerateCustomer();
        #endregion

        #region [ORDER ITEMS]
        //<algorithm> (check current suppliers' inventories to ensure only items that are possible to buy get generated; make use of difficulty to determine item requirements such as number of Items and each of their quality & quantity values).*
        List<OrderItem> items = new List<OrderItem>();
        int numberOfItems = UnityEngine.Random.Range(1, diffSetting.MaxOrderItems + 1);

        List<int> itemIDsAvailable = GetAvailableItemIDs(diffSetting);

        for (int c = 1; c <= numberOfItems; c++)
        {
            int itemIdIndex = UnityEngine.Random.Range(0, itemIDsAvailable.Count);

            int itemId = itemIDsAvailable[itemIdIndex];
            int quantity = UnityEngine.Random.Range(1, diffSetting.MaxOrderItemQuantity + 1);

            items.Add(new OrderItem(itemId, quantity));

            itemIDsAvailable.RemoveAt(itemIdIndex);
        }
        #endregion

        #region [DUE DATE]
        DateTime? dueDate;
        //<algorithm> (make use of difficulty to determine the order due date; take into account number of items in order - add time to due date).*

        #region (Example)
        //Loop through each generated order item, get the highest shipping time and add that time, multiplied by the multiplier in the difficulty setting, to the order due date.
        //Also, take into account the number of items in the order, and add a set amount of time to the order using the value in the difficulty setting.
        //************Also, take into account the number of current open orders?
        if (diffSetting.GenerateOrderDueDate)
        {
            int minutesToAdd = 0;

            int highestShippingTime = 0;

            for (int iItem = 0; iItem < items.Count; iItem++)
            {
                if (items[iItem].BaseShippingTime > highestShippingTime)
                    highestShippingTime = items[iItem].BaseShippingTime;
            }

            minutesToAdd += (int)(highestShippingTime * diffSetting.OrderTimeShippingTimeMultiplier);
            minutesToAdd += (int)(numberOfItems * diffSetting.OrderTimeAddedPerItem);

            dueDate = currentDate.AddMinutes(minutesToAdd * GameMaster.Instance.GameModeManager.Office.GameTimeSpeed);
        }
        else
            dueDate = null;

        #endregion

        #endregion

        Orders.Add(new Order(customer, items, currentDate, dueDate));

        CountOpen++;

		GameMaster.Instance.GUIManager.OrdersPanelScript.DisplayOrders ();
	    GameMaster.Instance.GUIManager.hudScript.orderNotifiation ();

        Debug.Log("*ORDER ITEMS:");
        foreach (OrderItem orderItem in Orders[Orders.Count - 1].Items)
            Debug.Log(orderItem.ToString());

        Debug.Log("Order value - base item costs: " + Orders[Orders.Count - 1].TotalValue().ToString());
        Debug.Log("Order value (with player markup): " + GameMaster.MarkupPrice(Orders[Orders.Count - 1].TotalValue(), GameMaster.Instance.Player.Business.GetTotalMarkup()).ToString());
    }

    public void CreateTutorialOrder()
    {
        Customer customer = GameMaster.Instance.CustomerManager.GenerateCustomer();
        DateTime dateReceived = GameMaster.Instance.GameDateTime;
        List<OrderItem> items = new List<OrderItem>();

        if (GameMaster.Instance.SupplierManager.Suppliers.Count > 1)
        {
            OrderItem item = new OrderItem(GameMaster.Instance.SupplierManager.Suppliers[0].Inventory.Items[0].ItemID, 5);
            items.Add(item);
        }

        Order newOrder = new Order(customer, items, dateReceived, null);

        Orders.Add(newOrder);
        CountOpen++;
    }

    public void CompleteOrder(int iOrderToComplete, List<OrderItem> orderItems, DateTime dateFilled, float markup, out float payment, out int score, out float penaltyMultiplier)
    {
        Orders[iOrderToComplete].CompleteOrder(orderItems, dateFilled, markup, out payment, out score, out penaltyMultiplier);

        GameMaster.Instance.Notifications.Add("Order for " + Orders[iOrderToComplete].Customer.FullName() + " completed!");

        Debug.Log("Total payment: " + payment.ToString());

        GameMaster.Instance.GUIManager.OrdersPanelScript.DisplayOrders();
    }

    public void CloseOrder(int iOrderToClose)
    {
        Orders[iOrderToClose].CloseOrder();

        GameMaster.Instance.Notifications.Add("Order for " + Orders[iOrderToClose].Customer.FullName() + " closed.");

        GameMaster.Instance.GUIManager.OrdersPanelScript.DisplayOrders();
    }

    private List<int> GetAvailableItemIDs(DifficultySO diffSetting)
    {
        List<int> itemIDsAvailable = new List<int>();
        List<int> itemIDsAll = new List<int>();

        List<SupplierAI> suppliers = GameMaster.Instance.SupplierManager.Suppliers;
        InventoryPlayer playerInventory = GameMaster.Instance.Player.Business.WarehouseInventory;

        int countAcceptedQualityItems = 0;

        //Loop through all items for all suppliers
        for (int iSupplier = 0; iSupplier < suppliers.Count; iSupplier++)
        {
            for (int iItem = 0; iItem < suppliers[iSupplier].Inventory.Items.Count; iItem++)
            {
                Item item = suppliers[iSupplier].Inventory.Items[iItem];

                if (!ExcludedItemSubcategories.Contains(item.Subcategory.EnumID) && !itemIDsAll.Contains(item.ItemID))
                {
                    itemIDsAll.Add(item.ItemID);

                    if ((int)item.Quality <= (int)diffSetting.MaxItemQuality)
                        countAcceptedQualityItems++;
                }
            }
        }

        //Loop through all player warehouse items
        for (int iPlayerItem = 0; iPlayerItem < playerInventory.Items.Count; iPlayerItem++)
        {
            OrderItem item = playerInventory.Items[iPlayerItem];

            if (!ExcludedItemSubcategories.Contains(item.Subcategory.EnumID) && !itemIDsAll.Contains(item.ItemID))
            {
                itemIDsAll.Add(item.ItemID);

                if ((int)item.Quality <= (int)diffSetting.MaxItemQuality)
                    countAcceptedQualityItems++;
            }
        }

        if (countAcceptedQualityItems > 0)
        {
            foreach (int itemID in itemIDsAll)
            {
                if ((int)GameMaster.Instance.ItemManager.Database.Items[itemID].Quality <= (int)diffSetting.MaxItemQuality || GameMaster.Roll(diffSetting.ChanceForAbnormalItemQuality))
                    itemIDsAvailable.Add(itemID);
            }

            if (itemIDsAvailable.Count < diffSetting.MaxOrderItems)
            {
                int countMakeupItems = diffSetting.MaxOrderItems - itemIDsAvailable.Count;
                List<int> itemIDsTemp = new List<int>();

                foreach (int itemID in itemIDsAll)
                {
                    if (!itemIDsAvailable.Contains(itemID))
                        itemIDsTemp.Add(itemID);
                }

                for (int c = 1; c <= countMakeupItems; c++)
                {
                    int indexTemp = UnityEngine.Random.Range(0, itemIDsTemp.Count);

                    itemIDsAvailable.Add(itemIDsTemp[indexTemp]);

                    itemIDsTemp.RemoveAt(indexTemp);
                }
            }
        }
        else
        {
            itemIDsAvailable = itemIDsAll; //*
        }

        return itemIDsAvailable;
    }

    #region Lists
    public List<Order> GetOpenOrders()
    {
        List<Order> orders = new List<Order>();

        for (int i = 0; i < Orders.Count; i++)
        {
            if (Orders[i].Open)
                orders.Add(Orders[i]);
        }

        return orders;
    }

    public List<Order> GetCompletedOrders()
    {
        List<Order> orders = new List<Order>();

        for (int i = 0; i < Orders.Count; i++)
        {
            if (Orders[i].Completed)
                orders.Add(Orders[i]);
        }

        return orders;
    }

    public List<Order> GetFailedOrders()
    {
        List<Order> orders = new List<Order>();

        for (int i = 0; i < Orders.Count; i++)
        {
            if (!Orders[i].Open && !Orders[i].Filled)
                orders.Add(Orders[i]);
        }

        return orders;
    }
    #endregion
}
