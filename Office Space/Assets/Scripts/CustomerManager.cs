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
        DifficultySO diffSetting = GameMaster.Instance.GetDifficultySetting();

        string firstName, lastName;
        CustomerLevel level;

        int maxCustomerLevel = (int)diffSetting.MaxCustomerLevel;

        int iFirstName, iLastName, iLevel;

        iFirstName = Random.Range(0, firstNames.Count);
        iLastName = Random.Range(0, lastNames.Count);
        iLevel = Random.Range(1, maxCustomerLevel + 1);

        firstName = firstNames[iFirstName];
        lastName = lastNames[iLastName];
        level = (CustomerLevel)iLevel;

        return new Customer(firstName, lastName, level);
    }
}
