using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Order
{
    public Customer Customer { get; set; }
    public List<InventoryItem> Items { get; set; }
    public DateTime DateReceived { get; private set; }
    public DateTime? DateDue { get; set; } //null due date for tutorial level?
    public bool Filled { get; set; }
    public DateTime? DateFilled { get; set; }

    #region <Constructors>
    public Order (Customer customer, List<InventoryItem> items, DateTime dateReceived, DateTime? dateDue) //*
    {
        Customer = customer;
        Items = items;
        DateReceived = dateReceived;
        DateDue = dateDue;

        Filled = false;
    }
    #endregion

    #region <Methods>
    public void CloseOrder(DateTime dateFilled)
    {
        if (!Filled)
        {
            Filled = true;
            DateFilled = dateFilled;
        }
    }
    #endregion
}
