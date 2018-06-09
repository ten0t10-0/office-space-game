using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[System.Serializable]
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
    public float GameTimeSpeed;

    public float ChanceNextOrder;

    public bool TutorialMode;
    public bool DayEnd;

    public int DayEndCurrent;

    public NotificationList Notifications;

    public GameData()
    { }

    public GameData(Player player, List<SupplierAI> suppliers, List<Order> orders, int difficulty, DateTime gameDateTime, float gameTimeSpeed, float chanceNextOrder, bool tutorialMode, bool dayEnd, int dayEndCurrent, NotificationList notifications)
    {
        Player = player;

        Suppliers = suppliers;

        Orders = orders;

        Difficulty = difficulty;

        GameDateTime = gameDateTime;
        GameTimeSpeed = gameTimeSpeed;

        ChanceNextOrder = chanceNextOrder;

        TutorialMode = tutorialMode;
        DayEnd = dayEnd;

        DayEndCurrent = dayEndCurrent;

        Notifications = notifications;
    }
}
