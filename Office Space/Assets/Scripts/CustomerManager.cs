using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Generate name using a random first name + <space> + surname
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
