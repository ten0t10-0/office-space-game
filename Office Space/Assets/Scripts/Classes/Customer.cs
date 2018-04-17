using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
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
    #endregion
}
