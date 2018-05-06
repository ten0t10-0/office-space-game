using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum ItemCategory { Special, Electronics, Furniture } //Add...
[SerializeField]
public enum ItemQuality { Low, Medium, High }

public class ItemManager : MonoBehaviour
{
    public ItemDatabaseSO Database;

    private const string err = "**[!]**";

    private void Awake()
    {
        ValidateDatabase();
    }

    private void ValidateDatabase()
    {
        bool valid = true;

        int iCategory, iType, iItem;

        for (iCategory = 0; iCategory < Database.Categories.Length; iCategory ++)
        {
            for (iType = 0; iType < Database.Categories[iCategory].Types.Length; iType++)
            {
                for (iItem = 0; iItem < Database.Categories[iCategory].Types[iType].Items.Length; iItem++)
                {
                    if (Database.Categories[iCategory].Types[iType].Items[iItem].Category != (ItemCategory)iCategory)
                    {
                        Debug.Log(string.Format(err + " Item Category in {0} > {1} > {2} is invalid!", Database.Categories[iCategory].name, Database.Categories[iCategory].Types[iType].name, Database.Categories[iCategory].Types[iType].Items[iItem].name));
                        valid = false;
                    }
                }

                if (iItem == 3)
                {
                    if (Database.Categories[iCategory].Types[iType].Items[0].Quality != ItemQuality.Low || Database.Categories[iCategory].Types[iType].Items[1].Quality != ItemQuality.Medium || Database.Categories[iCategory].Types[iType].Items[2].Quality != ItemQuality.High)
                    {
                        valid = false;
                        Debug.Log(string.Format(err + " Invalid Item Quality(s) in {0} > {1}!", Database.Categories[iCategory].name, Database.Categories[iCategory].Types[iType].name));
                    }
                }
                else
                {
                    valid = false;
                    Debug.Log(string.Format(err + " More than 3 Items in {0} > {1}!", Database.Categories[iCategory].name, Database.Categories[iCategory].Types[iType].name));
                }
            }
        }

        if (valid)
            Debug.Log("***ITEMS VALIDATED***");
    }
}
