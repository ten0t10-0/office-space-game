using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string Name { get; private set; }
    public SupplierPlayer Business { get; private set; }
    public float PlayTime { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }

    #region <Constructors>
    //New Player
    public Player(string name, string businessName, float startingMoney, float maximumInventorySpace)
    {
        Name = name;

        Business = new SupplierPlayer(businessName, startingMoney, maximumInventorySpace);

        PlayTime = 0f;
        Level = 1;
        Experience = 0;
    }

    ////Existing Player
    //public Player(string name, float money, SupplierPlayer business, float playTime, int level, int experience)
    //{
    //    Name = name;
    //    Money = money;
    //    Business = business;
    //    PlayTime = playTime;
    //    Level = level;
    //    Experience = experience;
    //}
    //
    ////Existing Player
    //public Player(string name, float money, float playTime, int level, int experience)
    //{
    //    Name = name;
    //    Money = money;
    //    PlayTime = playTime;
    //    Level = level;
    //    Experience = experience;
    //}
    #endregion

    #region <Methods>

    /// <summary>
    /// Returns a Customer object representing the player.
    /// </summary>
    /// <returns></returns>
    public Customer ToCustomer()
    {
        return new Customer(this.Name);
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return string.Format("Name: {0}; PlayTime: {1}; Level: {2}; Experience: {3}", Name, PlayTime.ToString(), Level.ToString(), Experience.ToString());
    }
}
