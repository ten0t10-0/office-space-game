using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplierManager : MonoBehaviour
{
    #region <PRESET NAMES LISTS>
    public List<string> tempPreNames = new List<string>()
        {
            "Galaxy", "Hello", "Illuminati", "FizzBuzz", "Perfect"
        };

    public List<string> tempPostNames = new List<string>()
        {
            "Suppliers", "Inc", "Ltd", "(Pty) Ltd"
        };

    public List<string> tempUniqueNames = new List<string>()
        {
            "Test Supplier"
        };
    #endregion

    private class MinMax
    {
        public float MinimumValue { get; set; }
        public float MaximumValue { get; set; }

        public MinMax(float min, float max)
        {
            MinimumValue = min;
            MaximumValue = max;
        }
    }

    private MinMax markupPercent = new MinMax(0.00f, 4.00f); //Up to 400%
    private MinMax buyPriceMult = new MinMax(0.25f, 3.00f);
    private MinMax conditionPercent = new MinMax(0.25f, 1.00f);

    public List<Supplier> GenerateSuppliers(int count)
    {
        List<Supplier> suppliers = new List<Supplier>(count);

        for (int i = 1; i <= count; i++)
        {
            suppliers.Add(new Supplier(GenerateSupplierName()));
        }

        return suppliers;
    }

    public string GenerateSupplierName()
    {
        string generatedName = "";

        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                {
                    random = Random.Range(0, tempPreNames.Count);
                    generatedName += tempPreNames[random] + " ";

                    random = Random.Range(0, tempPostNames.Count);
                    generatedName += tempPostNames[random];

                    break;
                }
            case 1:
                {
                    if (tempUniqueNames.Count != 0)
                    {
                        random = Random.Range(0, tempUniqueNames.Count);
                        generatedName = tempUniqueNames[random];

                        tempUniqueNames.RemoveAt(random);
                    }
                    else
                        generatedName = GenerateSupplierName();

                    break;
                }
        }

        return generatedName;
    }
}
