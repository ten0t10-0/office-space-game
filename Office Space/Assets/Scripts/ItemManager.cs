using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ItemCategory { ComputerComponent, Computer, Gaming,  } //Add...
//public enum ItemSubcategory { Motherboard, CPU, PowerSupply, GraphicsCard}
public enum ItemQuality { None, Low, Medium, High }

public class ItemManager : MonoBehaviour
{
    public ItemDatabaseSO Database;

    private const int INDEX_DEFAULT = 0;

    #region Queries
    public ItemSO GetItemSO(int itemId)
    {
        return Database.Items[itemId];
    }

    public int GetItemID(string itemName)
    {
        int itemId = INDEX_DEFAULT;

        itemName = itemName.ToLower();

        for (int i = 0; i < Database.Items.Count; i++)
        {
            string itemNameTemp = Database.Items[i].Name.ToLower();
            string itemNameCurrent = "";

            foreach (char c in itemNameTemp)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    itemNameCurrent += c.ToString();
            }

            if (itemNameCurrent != itemName)
            {
                string[] words = itemName.Split(' ');
                bool found = true;

                foreach (string word in words)
                {
                    if (!itemNameCurrent.Contains(word))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    itemId = i;
                }
            }
            else
            {
                itemId = i;
                i = Database.Items.Count; //end
            }
        }

        return itemId;
    }
    #endregion
}
