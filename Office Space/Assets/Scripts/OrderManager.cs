using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Order> Orders;

    public int MaxOrdersSaved = 50;

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

        List<int> itemIDsAvailable = GetAvailableItemIDs();

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
            minutesToAdd += (int)(numberOfItems * diffSetting.OrderTimeSecondsAddedPerItem);

            dueDate = currentDate.AddMinutes(minutesToAdd);
        }
        else
            dueDate = null;

        #endregion

        #endregion

        if (Orders.Count == MaxOrdersSaved)
        {
            Orders.RemoveAt(0);
        }

        Orders.Add(new Order(customer, items, currentDate, dueDate));
    }

    public void CompleteOrder(int iOrderToComplete, List<OrderItem> orderItems, DateTime dateFilled, out float payment, out int score, out float penaltyMultiplier)
    {
        Orders[iOrderToComplete].CompleteOrder(orderItems, dateFilled, out payment, out score, out penaltyMultiplier);
    }

    public void CloseOrder(int iOrderToClose)
    {
        Orders[iOrderToClose].CloseOrder();

        //*TEMP:
        Debug.Log("*ORDER CLOSED");
    }

    private List<int> GetAvailableItemIDs()
    {
        List<int> itemIDsAvailable = new List<int>();

        List<SupplierAI> suppliers = GameMaster.Instance.SupplierManager.Suppliers;
        InventoryPlayer playerInventory = GameMaster.Instance.Player.Business.WarehouseInventory;

        //Loop through all items for all suppliers
        for (int iSupplier = 0; iSupplier < suppliers.Count; iSupplier++)
        {
            for (int iItem = 0; iItem < suppliers[iSupplier].Inventory.Items.Count; iItem++)
            {
                bool exclude = false;

                Item item = suppliers[iSupplier].Inventory.Items[iItem];

                foreach (ItemSubcategory excludedSubcategory in ExcludedItemSubcategories)
                {
                    if (item.Subcategory.EnumID == excludedSubcategory)
                    {
                        exclude = true;
                        break;
                    }
                }

                if (!exclude)
                    itemIDsAvailable.Add(item.ItemID);
            }
        }

        //Loop through all player warehouse items
        for (int iPlayerItem = 0; iPlayerItem < playerInventory.Items.Count; iPlayerItem++)
        {
            bool exclude = false;

            OrderItem item = playerInventory.Items[iPlayerItem];

            foreach (ItemSubcategory excludedSubcategory in ExcludedItemSubcategories)
            {
                if (item.Subcategory.EnumID == excludedSubcategory)
                {
                    exclude = true;
                    break;
                }
            }

            if (!exclude)
                itemIDsAvailable.Add(item.ItemID);
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
            if (!Orders[i].Completed)
                orders.Add(Orders[i]);
        }

        return orders;
    }
    #endregion
}
