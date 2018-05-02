using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplierManager : MonoBehaviour
{
    #region <PRESET NAMES LISTS>
    public List<string> namesPre = new List<string>()
        {
            "Galaxy", "Hello", "Illuminati", "FizzBuzz", "Perfect"
        };
    public List<string> namesPost = new List<string>()
        {
            "Suppliers", "Inc", "Ltd", "(Pty) Ltd"
        };
    public List<string> namesUnique = new List<string>()
        {
            "Bonus Supplier"
        };

    private List<string> namesPreRemaining;
    #endregion

    [HideInInspector]
    public List<SupplierAI> Suppliers;

    //private RangeAttribute markupPercent = new RangeAttribute(0.00f, 4.00f); //Up to 400%

    #region Methods
    
    /// <summary>
    /// (WIP?) Generates the specified number of Suppliers and stores them in the Suppliers list.
    /// </summary>
    /// <param name="count">The number of Suppliers to generate.</param>
    /// <param name="message">String to store the result message.</param>
    public void GenerateSuppliers(int count)
    {
        Suppliers = new List<SupplierAI>();

        string currentGeneratedName;

        string message = GameMaster.MSG_ERR_DEFAULT;

        namesPreRemaining = new List<string>();

        for (int i = 0; i < namesPre.Count; i++)
        { namesPreRemaining.Add(namesPre[i]); }

        for (int c = 1; c <= count; c++)
        {
            currentGeneratedName = GenerateName();

            if (currentGeneratedName != "")
                Suppliers.Add(new SupplierAI(currentGeneratedName));
            else
            {
                c = count + 1; //set sentinel value to end the FOR loop
                message = "Partial Success - " + Suppliers.Count.ToString() + " out of " + count.ToString() + " suppliers were generated!";
            }
        }

        if (Suppliers.Count == count)
            message = "Success - All " + count.ToString() + " suppliers were generated!";

        GameMaster.Instance.Log(message);
    }

    /// <summary>
    /// (WIP) Populates all Suppliers' inventories with new randomized items.
    /// </summary>
    public void PopulateSupplierInventories()
    {
        //1. Clear items:
        ClearAllSupplierInventoryItems();

        //2. Generate items:
        //*
    }

    /// <summary>
    /// Clears all Suppliers' inventories.
    /// </summary>
    private void ClearAllSupplierInventoryItems()
    {
        for (int i = 0; i < Suppliers.Count; i++)
        { Suppliers[i].Inventory.Clear(); }
    }

    /// <summary>
    /// Returns a string containing a randomly generated Supplier name.
    /// </summary>
    /// <returns></returns>
    private string GenerateName()
    {
        string generatedName = "";

        int iPreName, iPostName;

        if (namesPreRemaining.Count != 0)
        {
            iPreName = Random.Range(0, namesPreRemaining.Count);
            iPostName = Random.Range(0, namesPost.Count);

            generatedName = namesPreRemaining[iPreName] + ' ' + namesPost[iPostName];

            namesPreRemaining.RemoveAt(iPreName);
        }

        return generatedName;
    }

    /// <summary>
    /// Returns a string containing a random unique Supplier name.
    /// </summary>
    /// <returns></returns>
    private string GetRandomUniqueName()
    {
        int random = Random.Range(0, namesUnique.Count);
        return namesUnique[random];
    }
    #endregion
}
