﻿using System.Collections;
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

    public int Difficulty;

    [SerializeField]
    public DateTime GameDateTime;

    public float ChanceNextOrder;

    public bool SleepMode;
    public bool DayEnd;

    public int DayEndCurrent;

    public NotificationList Notifications;

    public GameMode GameMode;

    public GameData()
    { }

    public GameData(Player player, List<SupplierAI> suppliers, List<Order> orders, int difficulty, DateTime gameDateTime, float chanceNextOrder, bool sleepMode, bool dayEnd, int dayEndCurrent, NotificationList notifications, GameMode gameMode)
    {
        Player = player;

        Suppliers = suppliers;

        Orders = orders;

        Difficulty = difficulty;

        GameDateTime = gameDateTime;

        ChanceNextOrder = chanceNextOrder;

        SleepMode = sleepMode;
        DayEnd = dayEnd;

        DayEndCurrent = dayEndCurrent;

        Notifications = notifications;

        GameMode = gameMode;
    }
}
