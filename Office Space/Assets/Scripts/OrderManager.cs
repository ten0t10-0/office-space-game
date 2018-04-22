using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    public List<Order> OrdersOpen, OrdersFilled, OrdersFailed;

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

    /// <summary>
    /// Removes the specified order from the Open Orders list and adds it to either the Filled Orders or Failed Orders list.
    /// </summary>
    /// <param name="orderToCloseId">The ID/index of the order in the Open Orders list.</param>
    /// <param name="orderFilled">Was the order successful?</param>
    public void CloseOrder(int orderToCloseId, bool orderFilled)
    {
        Order orderToClose = OrdersOpen[orderToCloseId];
        OrdersOpen.RemoveAt(orderToCloseId);

        if (orderFilled)
        {
            DateTime filledDate = GameMaster.Instance.GameDateTime;
            orderToClose.SetFilled(filledDate);

            OrdersFilled.Add(orderToClose);
        }
        else
        {
            OrdersFailed.Add(orderToClose);
        }
    }

    /// <summary>
    /// Removes all orders from the Open, Filled and Failed Orders lists.
    /// </summary>
    private void ClearAllOrders()
    {
        OrdersOpen.Clear();
        OrdersFilled.Clear();
        OrdersFailed.Clear();
    }
}
