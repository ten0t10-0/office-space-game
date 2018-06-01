using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Order> Orders;

    public int maxOrdersSaved = 50;

    /// <summary>
    /// (WIP) Generates an Order based on the difficulty specified in the GameMaster instance.
    /// </summary>
    public void GenerateOrder()
    {
        DateTime currentDate = GameMaster.Instance.GameDateTime;
        DifficultySO diffSetting = GameMaster.Instance.GetDifficultySettings();
        List<SupplierAI> suppliers = GameMaster.Instance.SupplierManager.Suppliers;

        #region [CUSTOMER]
        Customer customer = GameMaster.Instance.CustomerManager.GenerateCustomer();
        #endregion

        #region [ORDER ITEMS]
        //<algorithm> (check current suppliers' inventories to ensure only items that are possible to buy get generated; make use of difficulty to determine item requirements such as number of Items and each of their quality & quantity values).*
        List<OrderItem> items = new List<OrderItem>();
        int numberOfItems = UnityEngine.Random.Range(1, diffSetting.MaxOrderItems + 1);

        List<int> itemIDsAvailable = new List<int>();

        for (int iSupplier = 0; iSupplier < suppliers.Count; iSupplier++)
        {
            for (int iItem = 0; iItem < suppliers[iSupplier].Inventory.Items.Count; iItem++)
            {
                itemIDsAvailable.Add(suppliers[iSupplier].Inventory.Items[iItem].ItemID);
            }
        }

        for (int c = 1; c <= numberOfItems; c++)
        {
            int itemId = itemIDsAvailable[UnityEngine.Random.Range(0, itemIDsAvailable.Count)];
            int quantity = 1; //***
            items.Add(new OrderItem(itemId, quantity));
        }
        #endregion

        #region [DUE DATE]
        DateTime? dueDate;
        //<algorithm> (make use of difficulty to determine the order due date; take into account number of items in order - add time to due date).*

        #region (Example)
        float hoursToAdd = 1;

        if (diffSetting.GenerateOrderDueDate)
        {
            //...
            dueDate = currentDate.AddHours(hoursToAdd);
        }
        else
            dueDate = null;

        #endregion

        #endregion

        if (Orders.Count == maxOrdersSaved)
        {
            Orders.RemoveAt(0);
        }

        Orders.Add(new Order(customer, items, currentDate, dueDate));
    }

    public void CompleteOrder(int iOrderToComplete, List<OrderItem> orderItems, DateTime dateFilled, out float payment)
    {
        Orders[iOrderToComplete].CompleteOrder(orderItems, dateFilled, out payment);
    }

    public void CloseOrder(int iOrderToClose)
    {
        Orders[iOrderToClose].CloseOrder();
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
