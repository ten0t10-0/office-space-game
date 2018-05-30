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
        int difficulty = GameMaster.Instance.Difficulty;

        #region [CUSTOMER]
        Customer customer = GameMaster.Instance.CustomerManager.GenerateCustomer();
        #endregion

        #region [ORDER_ITEMS]
        List<OrderItem> items = new List<OrderItem>();
        //<algorithm> (check current suppliers' inventories to ensure only items that are possible to buy get generated; make use of difficulty to determine item requirements such as number of Items and each of their quality & quantity values).*
        #endregion

        #region [DUE DATE]
        DateTime? dueDate;
        //<algorithm> (make use of difficulty to determine the order due date; take into account number of items in order - add time to due date).*

        #region (Example)
        float hoursToAdd = 1;

        if (difficulty != 0)
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

    //public void CompleteOrder(int iOrderToComplete, List<OrderItem> orderItems)
    //{
        
    //}

    public void CloseOrder(int iOrderToClose)
    {

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
