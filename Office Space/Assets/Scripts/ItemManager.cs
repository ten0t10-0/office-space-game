using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { Special, Electronics, Furniture } //Add...
public enum ItemQuality { None, Low, Medium, High }

public class ItemManager : MonoBehaviour
{
    public ItemDatabaseSO Database;

    private string itemTypeLast, itemDescriptionLast;

    private const int INDEX_DEFAULT = 0;

    private void Awake()
    {
        //ValidateItemDatabase();
    }

    #region Queries
    public ItemSO FetchItem(int itemId)
    {
        return Database.Items[itemId];
    }

    public int GetItemID(string itemType, string itemDescription)
    {
        int itemId = INDEX_DEFAULT;

        bool itemTypeFound = false;

        itemType = itemType.ToLower();
        itemDescription = itemDescription.ToLower();

        for (int i = 0; i < Database.Items.Count; i++)
        {
            if (!itemTypeFound && Database.Items[i].Type.Name.ToLower() == itemType)
            {
                itemTypeFound = true;

                itemTypeLast = itemType;

                itemId = i;
            }

            if (itemTypeFound && Database.Items[i].Description.ToLower() == itemDescription)
            {
                itemId = i;

                itemDescriptionLast = itemDescription;

                i = Database.Items.Count; //end
            }
        }

        return itemId;
    }
    #endregion

    private void ValidateItemDatabase()
    {
        List<ItemTypeSO> typesFound = new List<ItemTypeSO>();
        List<int> typesItemCount = new List<int>();
        List<ItemQuality[]> typesItemQualities = new List<ItemQuality[]>();

        for (int i = 0; i < Database.Items.Count; i++)
        {
            //Pull Item Type:
            ItemTypeSO typeFound = Database.Items[i].Type;

            //If Item Type not in list, add it, and add/set its item count in parallel list to 1;
            if (!typesFound.Contains(typeFound))
            {
                typesFound.Add(typeFound);
                typesItemCount.Add(1);
            }
            //If Item Type already in list, increment its item count by 1;
            else
            {
                typesItemCount[i]++;
            }
        }
    }
}
