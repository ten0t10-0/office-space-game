using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string Name { get; private set; }
    public float Money { get; set; }
    public Supplier Business { get; private set; }
    public float PlayTime { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }

    #region <Constructors>
    //New Player
    public Player(string name, float startingMoney, string businessName, float maximumInventorySpace)
    {
        Name = name;
        Money = startingMoney;

        Business = new Supplier(businessName, maximumInventorySpace);

        PlayTime = 0f;
        Level = 1;
        Experience = 0;
    }

    //Existing Player
    public Player(string name, float money, Supplier business, float playTime, int level, int experience)
    {
        Name = name;
        Money = money;
        Business = business;
        PlayTime = playTime;
        Level = level;
        Experience = experience;
    }

    //Existing Player
    public Player(string name, float money, float playTime, int level, int experience)
    {
        Name = name;
        Money = money;
        PlayTime = playTime;
        Level = level;
        Experience = experience;
    }
    #endregion
}
