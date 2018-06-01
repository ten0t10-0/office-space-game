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

    public GameData()
    { }

    public GameData(Player player, List<SupplierAI> suppliers, List<Order> orders, int difficulty, DateTime gameDateTime, float gameTimeSpeed, float chanceNextOrder)
    {
        Suppliers = suppliers;

        Player = player;

        Orders = orders;

        Difficulty = difficulty;

        GameDateTime = gameDateTime;
        GameTimeSpeed = gameTimeSpeed;
    }
}
