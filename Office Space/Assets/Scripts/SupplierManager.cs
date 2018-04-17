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
    public List<Supplier> Suppliers;

    //private RangeAttribute markupPercent = new RangeAttribute(0.00f, 4.00f); //Up to 400%
    //private RangeAttribute buyPriceMult = new RangeAttribute(0.25f, 3.00f);
    //private RangeAttribute conditionPercent = new RangeAttribute(0.25f, 1.00f);

    #region Methods
    public void GenerateSuppliers(int count, out string message)
    {
        Suppliers = new List<Supplier>();

        string currentGeneratedName;

        message = GameMaster.MSG_ERR_DEFAULT;

        namesPreRemaining = new List<string>();

        for (int i = 0; i < namesPre.Count; i++)
        { namesPreRemaining.Add(namesPre[i]); }

        for (int c = 1; c <= count; c++)
        {
            currentGeneratedName = GenerateName();

            if (currentGeneratedName != "")
                Suppliers.Add(new Supplier(currentGeneratedName));
            else
            {
                c = count + 1; //set sentinel value to end the FOR loop
                message = "Partial Success - " + Suppliers.Count.ToString() + " out of " + count.ToString() + " suppliers were generated!";
            }
        }

        if (Suppliers.Count == count)
            message = "Success - All " + count.ToString() + " suppliers were generated!";
    }

    //Generate name using a random pre-name + <space> + post-name
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

    private string GetRandomUniqueName()
    {
        int random = Random.Range(0, namesUnique.Count);
        return namesUnique[random];
    }
    #endregion
}
