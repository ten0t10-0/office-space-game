using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Order
{
    public Customer Customer { get; set; }
    [SerializeField]
    public List<OrderItem> Items { get; set; }
    public DateTime DateReceived { get; private set; }
    public DateTime? DateDue { get; set; } //null due date for tutorial level?
    public bool Filled { get; set; }
    public DateTime? DateFilled { get; set; }
    public bool Open { get; set; }
    public int Score { get; private set; }

    #region <Constructors>
    public Order (Customer customer, List<OrderItem> items, DateTime dateReceived, DateTime? dateDue) //*
    {
        Customer = customer;
        Items = items;
        DateReceived = dateReceived;
        DateDue = dateDue;

        Filled = false;
        DateFilled = null;
        Open = true;

        Score = 0;
    }
    #endregion

    #region <Methods>

    /// <summary>
    /// (CALLED BY ORDERMANAGER) Sets this order as completed and closed.
    /// </summary>
    /// <param name="items">The list of items to be used to fill the order.</param>
    /// <param name="dateFilled">The date the order was successfully completed.</param>
    /// <param name="paymentTotal">The resulting payment (base cost).</param>
    public void CompleteOrder(List<OrderItem> items, DateTime dateFilled, out float paymentTotal)
    {
        paymentTotal = 0;

        if (Open && !Filled)
        {
            Filled = true;
            DateFilled = dateFilled;

            Score = CalculateScore(items, out paymentTotal);

            CloseOrder();
        }
    }

    /// <summary>
    /// Sets this order as closed.
    /// </summary>
    public void CloseOrder()
    {
        if (Open)
        {
            Open = false;
        }
    }

    /// <summary>
    /// Returns a TimeSpan containing the time remaining for this order. Returns null if the order is already filled or has no Due Date.
    /// </summary>
    /// <returns></returns>
    public TimeSpan? GetTimeRemaining()
    {
        if (!Filled && DateDue.HasValue)
            return DateDue.Value.Subtract(GameMaster.Instance.GameDateTime);
        else
            return null;
    }

    private int CalculateScore(List<OrderItem> items, out float paymentTotal)
    {
        int score = 0;
        paymentTotal = 0;

        if (Filled && DateDue.HasValue)
        {
            int quantityTotal = 0;
            float penaltyPercent;

            //*** Calculate score (based on time):
            TimeSpan timeSpanCompleted = DateDue.Value.Subtract(DateFilled.Value);
            TimeSpan timeSpanMax = DateDue.Value.Subtract(DateReceived);

            float rate = (float)(timeSpanCompleted.TotalMilliseconds / timeSpanMax.TotalMilliseconds);

            if (rate >= 0.75f) //Bonus
                score = 200;
            else if (rate >= 0.5f)
                score = 150;
            else
                score = 100;

            //*** Calculate penalty (based on accuracy/correctness of items):
            for (int iItems = 0; iItems < Items.Count; iItems++)
            {
                int itemIDRequired = Items[iItems].ItemID;
                int quantityRequired = Items[iItems].Quantity;

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ItemID == itemIDRequired)
                    {
                        paymentTotal += items[i].TotalValue();
                        quantityTotal += items[i].Quantity;

                        items.RemoveAt(i);
                        i = items.Count; //end
                    }
                }
            }

            penaltyPercent = quantityTotal / TotalQuantity();

            score *= (int)penaltyPercent;
        }

        return score;
    }

    /// <summary>
    /// Returns a float containing the total cost of this order.
    /// </summary>
    /// <returns></returns>
    public float TotalValue()
    {
        float cost = 0f;

        foreach (OrderItem item in Items)
        {
            cost += item.TotalValue();
        }

        return cost;
    }

    /// <summary>
    /// Returns the total quantities of all items.
    /// </summary>
    /// <returns></returns>
    public int TotalQuantity()
    {
        int qty = 0;

        foreach (OrderItem item in Items)
        {
            qty += item.Quantity;
        }

        return qty;
    }

    /// <summary>
    /// Returns true if the order is closed AND completed.
    /// </summary>
    public bool Completed
    {
        get { return (!Open && Filled); }
    }
    #endregion

    public override string ToString()
    {
        string dateDueString, dateFilledString;

        if (DateDue.HasValue)
            dateDueString = DateDue.Value.ToString();
        else
            dateDueString = "<No due date>";

        if (DateFilled.HasValue)
            dateFilledString = DateFilled.Value.ToString();
        else
            dateFilledString = "<Not completed>";

        return string.Format("Customer: {0}; DateReceived: {1}; DateDue: {2}; Filled?: {3}; DateFilled: {4}; Open: {5}; Score: {6}; Total Value: {7}; Total Quantities: {8}", Customer.FullName(), DateReceived.ToString(), dateDueString, Filled.ToString(), dateFilledString, Open.ToString(), Score.ToString(), TotalValue().ToString(), TotalQuantity().ToString());
    }
}
