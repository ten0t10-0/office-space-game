using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    #region <Constructors>
    public Customer(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        //...
    }

    //Players as customers:
    public Customer(string userName)
    {
        FirstName = userName;
        LastName = null;
        //...
    }
    #endregion

    #region <Methods>
    public string FullName()
    {
        if (LastName != null)
            return FirstName + ' ' + LastName;
        else
            return FirstName;
    }
    #endregion
}
