using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Order
{
    public Customer Customer { get; set; }
    public List<InventoryItem> Items { get; set; }
    public DateTime DateReceived { get; private set; }
    public DateTime DateDue { get; set; }

    #region <Constructors>
    public Order (Customer customer, List<InventoryItem> items, DateTime dateReceived, DateTime dateDue) //*
    {
        Customer = customer;
        Items = items;
        DateReceived = dateReceived;
        DateDue = dateDue;
    }
    #endregion
}
