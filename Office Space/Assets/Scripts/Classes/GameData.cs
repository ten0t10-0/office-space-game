using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField]
    public Player Player;

    [SerializeField]
    public List<SupplierAI> Suppliers;

    [SerializeField]
    public List<Order> OrdersOpen, OrdersFilled, OrdersFailed;

    public GameData(Player player, List<SupplierAI> suppliers, List<Order> ordersOpen, List<Order> ordersFilled, List<Order> ordersFailed)
    {
        Suppliers = suppliers;

        Player = player;

        OrdersOpen = ordersOpen;
        OrdersFilled = ordersFilled;
        OrdersFailed = ordersFailed;
    }
}
