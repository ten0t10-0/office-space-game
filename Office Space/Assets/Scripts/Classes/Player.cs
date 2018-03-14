using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Money { get; set; }
    public Supplier Business { get; private set; }
    public float PlayTime { get; set; }

    #region <Constructors>
    //New Player
    public Player(string name, float startingMoney, string businessName, float maximumInventorySpace)
    {
        Name = name;
        Money = startingMoney;
        PlayTime = 0f;

        Business = new Supplier(businessName, maximumInventorySpace);
    }

    //Existing Player
    public Player(string name, float money, Supplier business)
    {
        Name = name;
        Money = money;

        Business = business;
    }

    //Existing Player
    public Player(string name, float money, float playTime)
    {
        Name = name;
        Money = money;
        PlayTime = playTime;
    }

    //Existing Player
    public Player(string name, float money)
    {
        Name = name;
        Money = money;
    }
    #endregion
}
