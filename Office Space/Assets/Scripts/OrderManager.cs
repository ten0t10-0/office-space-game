using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    public List<Order> OrdersOpen, OrdersClosed;

    public void GenerateOrder()
    {
        DateTime currentDate = GameMaster.Instance.GameDateTime;
        int difficulty = GameMaster.Instance.Difficulty;

        #region [CUSTOMER]
        Customer customer = GameMaster.Instance.CustomerManager.GenerateCustomer();
        #endregion

        #region [ORDER_ITEMS]
        List<InventoryItem> items = new List<InventoryItem>();
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

        OrdersOpen.Add(new Order(customer, items, currentDate, dueDate));
    }

    public void CloseOrder(int i)
    {
        DateTime filledDate = GameMaster.Instance.GameDateTime;
        Order filledOrder = OrdersOpen[i];

        filledOrder.CloseOrder(filledDate);

        OrdersOpen.RemoveAt(i);
        OrdersClosed.Add(filledOrder);
    }

    private void ClearAllOrders()
    {
        OrdersOpen.Clear();
        OrdersClosed.Clear();
    }
}
