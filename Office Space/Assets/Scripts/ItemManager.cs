using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { Special, Components, Hardware, Gaming, Computers, Merchandise  } //Add...
public enum ItemSubcategory { Nothing, CPU, GPU, MotherBoard, Ram, Console, Desktop, Laptop, PCGame, ConsoleGame,
	Keyboard, Mouse,figurines,Tshirts,  } //Add...
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

    public ItemCategorySO GetCategorySO(ItemCategory enumId, out int index)
    {
        ItemCategorySO categorySO = null;
        index = -1;

        for (int i = 0; i < Database.Categories.Count; i++)
        {
            if (Database.Categories[i].EnumID == enumId)
            {
                categorySO = Database.Categories[i];
                index = i;

                i = Database.Categories.Count; //end
            }
        }

        return categorySO;
    }

    public ItemSubcategorySO GetSubcategorySO(ItemSubcategory enumId, out int index)
    {
        ItemSubcategorySO subcategorySO = null;
        index = -1;

        for (int i = 0; i < Database.Subcategories.Count; i++)
        {
            if (Database.Subcategories[i].EnumID == enumId)
            {
                subcategorySO = Database.Subcategories[i];
                index = i;

                i = Database.Subcategories.Count; //end
            }
        }

        return subcategorySO;
    }

    /// <summary>
    /// Searches through all items for the specified word(s) and returns the found item's ID.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
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
