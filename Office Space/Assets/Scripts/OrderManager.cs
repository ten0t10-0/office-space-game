using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    public List<Order> Orders;

    private int difficulty;

    public void GenerateOrder(int difficulty)
    {
        this.difficulty = difficulty;

        Customer customer = GameMaster.Instance.CustomerManager.GenerateCustomer();
        List<InventoryItem> items = GenerateOrderItemList();
        DateTime dueDate = GenerateOrderDueDate();

        Orders.Add(new Order(customer, items, GameMaster.Instance.GameDateTime, dueDate));
    }

    private List<InventoryItem> GenerateOrderItemList()
    {
        List<InventoryItem> items = new List<InventoryItem>();

        //<algorithm> (check current suppliers' inventories to ensure only items that are possible to buy get generated; make use of difficulty to determine item requirements such as number of Items and each of their quality & quantity values).*

        return items;
    }

    private DateTime GenerateOrderDueDate()
    {
        DateTime date = GameMaster.Instance.GameDateTime.AddHours(1); //*TEMP

        //<algorithm> (make use of difficulty to determine the order due date; maybe add hours to current date or add day(s) for larger orders).*

        return date;
    }

    private void RemoveOrder(int i)
    {
        Orders.RemoveAt(i);
    }
}
