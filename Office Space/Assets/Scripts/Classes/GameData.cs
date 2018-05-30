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

    [SerializeField]
    public DateTime GameDateTime;
    public float GameTimeSpeed;

    //Difficulty?

    public GameData()
    { }

    public GameData(Player player, List<SupplierAI> suppliers, List<Order> orders, DateTime gameDateTime, float gameTimeSpeed)
    {
        Suppliers = suppliers;

        Player = player;

        Orders = orders;

        GameDateTime = gameDateTime;
        GameTimeSpeed = gameTimeSpeed;
    }
}
