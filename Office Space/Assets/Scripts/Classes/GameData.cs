using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class GameData
{
    [SerializeField]
    public Player Player;

    [SerializeField]
    public List<SupplierAI> Suppliers;

    [SerializeField]
    public List<Order> Orders;
    public int OrdersCountOpen, OrdersCountCompleted, OrdersCountFailed, OrdersCountCompletedToday, OrdersCountFailedToday;

    public int Difficulty;

    [SerializeField]
    public DateTime GameDateTime;
    [SerializeField]
    public DayOfWeek DayDebt;
    public int WeekCurrent;

    public float ChanceNextOrder;

    public bool SleepMode;
    public bool DayEnd;

    public int DayEndCurrent;

    public NotificationList Notifications;

    public GameMode GameMode;

    public GameData()
    { }
}
