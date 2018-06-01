using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CustomerLevel Level { get; set; } //* not used

    #region <Constructors>
    public Customer(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        Level = CustomerLevel.None;
    }

    public Customer(string firstName, string lastName, CustomerLevel level)
    {
        FirstName = firstName;
        LastName = lastName;
        Level = level;
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

    public override string ToString()
    {
        return string.Format("First Name: {0}; Last Name: {1}; Level: <{2}>", FirstName, LastName, Level.ToString());
    }
}
