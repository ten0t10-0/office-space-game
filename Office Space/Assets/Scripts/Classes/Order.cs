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
    }
    #endregion

    #region <Methods>

    /// <summary>
    /// (CALLED BY ORDERMANAGER) Sets this order as completed and closed.
    /// </summary>
    /// <param name="items">The list of items to be used to fill the order.</param>
    /// <param name="dateFilled">The date the order was successfully completed.</param>
    /// <param name="paymentTotal">The resulting payment (base cost).</param>
    public void CompleteOrder(List<OrderItem> items, DateTime dateFilled, float markup, out float paymentTotal, out int score, out float penaltyMultiplier)
    {
        paymentTotal = 0;
        score = 0;
        penaltyMultiplier = 1;

        if (Open && !Filled)
        {
            Filled = true;
            DateFilled = dateFilled;

            score = CalculateScore(items, out paymentTotal, out penaltyMultiplier);

            paymentTotal = GameMaster.MarkupPrice(paymentTotal, markup);

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

    private int CalculateScore(List<OrderItem> items, out float paymentTotal, out float penaltyMultiplier)
    {
        int score = 0;
        paymentTotal = 0;
        penaltyMultiplier = 1;

        if (Filled)
        {
            int quantityTotal = 0;
            int quantityExcess = 0;

            //*** Calculate score (based on time):
            if (DateDue.HasValue)
            {
                TimeSpan timeSpanCompleted = DateDue.Value.Subtract(DateFilled.Value);
                TimeSpan timeSpanMax = DateDue.Value.Subtract(DateReceived);

                Debug.Log("TimeSpan completed: " + timeSpanCompleted.ToString());
                Debug.Log("TimeSpan max: " + timeSpanMax.ToString());

                float rate = (float)(timeSpanCompleted.TotalMilliseconds / timeSpanMax.TotalMilliseconds);

                if (rate >= 0.75f) //Bonus
                {
                    score = 200;
                    Debug.Log("Time bonus: x2");
                }
                else if (rate >= 0.5f)
                {
                    score = 150;
                    Debug.Log("Time bonus: x1.5");
                }
                else
                {
                    score = 100;
                    Debug.Log("Time bonus: <none>");
                }
            }
            else
            {
                score = 100; //*
                Debug.Log("Time bonus: <none>");
            }

            //*** Calculate penalty (based on accuracy/correctness of items):
            for (int iItems = 0; iItems < Items.Count; iItems++)
            {
                int itemIDRequired = Items[iItems].ItemID;
                int quantityRequired = Items[iItems].Quantity;

                for (int i = 0; i < items.Count; i++)
                {
                    int itemIDGiven = items[i].ItemID;
                    int quantityGiven = items[i].Quantity;

                    if (itemIDGiven == itemIDRequired)
                    {
                        if (quantityGiven <= quantityRequired)
                        {
                            paymentTotal += items[i].TotalValue();
                            quantityTotal += quantityGiven;
                        }
                        else
                        {
                            paymentTotal += items[i].UnitCost * quantityRequired; //only pay for number of items requested in order.
                            quantityTotal += quantityRequired;

                            quantityExcess += quantityGiven - quantityRequired;
                        }

                        items.RemoveAt(i);
                        i = items.Count; //end
                    }
                }
            }

            Debug.Log("Items correct: " + quantityTotal.ToString() + "/" + TotalQuantity());
            Debug.Log("Items in excess: " + quantityExcess.ToString());

            penaltyMultiplier = (float)quantityTotal / TotalQuantity(); //*

            score = (int)(score * penaltyMultiplier);
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
    /// Returns true if the order is closed AND filled.
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

        return string.Format("Customer: {0}; DateReceived: {1}; DateDue: {2}; Filled?: {3}; DateFilled: {4}; Open: {5}; Total Value: {6}; Total Quantities: {7}", Customer.FullName(), DateReceived.ToString(), dateDueString, Filled.ToString(), dateFilledString, Open.ToString(), TotalValue().ToString(), TotalQuantity().ToString());
    }
}
