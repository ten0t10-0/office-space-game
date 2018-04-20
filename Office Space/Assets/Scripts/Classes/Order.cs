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

    #region <Calculated Properties>

    /// <summary>
    /// Returns a float containing the total cost of this order.
    /// </summary>
    /// <returns></returns>
    public float Cost()
    {
        float cost = 0f;

        foreach (InventoryItem item in Items)
        {
            cost += item.TotalValue();
        }

        return cost;
    }
    #endregion

    #region <Constructors>
    public Order (Customer customer, List<InventoryItem> items, DateTime dateReceived, DateTime? dateDue) //*
    {
        Customer = customer;
        Items = items;
        DateReceived = dateReceived;
        DateDue = dateDue;

        Filled = false;
        DateFilled = null;
    }
    #endregion

    #region <Methods>

    /// <summary>
    /// Sets this order as completed.
    /// </summary>
    /// <param name="dateFilled">The date the order was successfully completed.</param>
    public void SetFilled(DateTime dateFilled)
    {
        if (!Filled)
        {
            Filled = true;
            DateFilled = dateFilled;
        }
    }
    #endregion
}
