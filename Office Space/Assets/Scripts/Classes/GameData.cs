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
    public List<Order> OrdersOpen, OrdersFilled, OrdersFailed;

    [SerializeField]
    public DateTime GameDateTime;
    public float GameTimeSpeed;

    //Difficulty?

    public GameData()
    { }

    public GameData(Player player, List<SupplierAI> suppliers, List<Order> ordersOpen, List<Order> ordersFilled, List<Order> ordersFailed, DateTime gameDateTime, float gameTimeSpeed)
    {
        Suppliers = suppliers;

        Player = player;

        OrdersOpen = ordersOpen;
        OrdersFilled = ordersFilled;
        OrdersFailed = ordersFailed;

        GameDateTime = gameDateTime;
        GameTimeSpeed = gameTimeSpeed;
    }
}
