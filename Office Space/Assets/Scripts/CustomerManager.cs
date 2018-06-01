using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerLevel { None, Low, Medium, High }

public class CustomerManager : MonoBehaviour
{
    #region <PRESET NAMES LISTS>
    public List<string> firstNames = new List<string>()
        {
            "John", "Jane"
        };
    public List<string> lastNames = new List<string>()
        {
            "Doe"
        };
    #endregion

    /// <summary>
    /// Returns a randomly generated Customer object.
    /// </summary>
    /// <returns></returns>
    public Customer GenerateCustomer()
    {
        string firstName, lastName;
        int iFirstName, iLastName;

        iFirstName = Random.Range(0, firstNames.Count);
        iLastName = Random.Range(0, lastNames.Count);

        firstName = firstNames[iFirstName];
        lastName = lastNames[iLastName];

        return new Customer(firstName, lastName);
    }
}
