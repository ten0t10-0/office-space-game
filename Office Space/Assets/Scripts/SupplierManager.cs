using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplierManager : MonoBehaviour
{
    public int InitNumberOfSuppliers = 5;
    public int MinNumberOfItemsPerSupplier = 10;
    public int MaxNumberOfItemsPerSupplier = 25;
    public int MaxGenerationsPerItem = 3;
    public List<ItemSubcategory> ExcludedItemSubcategories = new List<ItemSubcategory>
        {
            ItemSubcategory.Nothing
        };
    public int LowestDiscount = 5;
    public int HighestDiscount = 50;

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

    #region Methods
    
    /// <summary>
    /// (WIP?) Generates the specified number of Suppliers and stores them in the Suppliers list.
    /// </summary>
    /// <param name="supplierCount">The number of Suppliers to generate.</param>
    /// <param name="message">String to store the result message.</param>
    public void GenerateSuppliers(int supplierCount, out string result)
    {
        string currentGeneratedName;
        List<float> discounts = new List<float>();
        float currentDiscount;

        result = GameMaster.MSG_ERR_DEFAULT;

        Suppliers = new List<SupplierAI>();

        namesPreRemaining = new List<string>();

        for (int i = 0; i < namesPre.Count; i++)
        { namesPreRemaining.Add(namesPre[i]); }

        //<Generate discounts>
        float discountHighest = (float)HighestDiscount / 100;
        float discountLowest = (float)LowestDiscount / 100;
        float discountInterval = (float)System.Math.Round((discountHighest - discountLowest) / (supplierCount - 1), 2);

        discounts.Add(discountLowest);
        for (int i = 0; i < supplierCount - 2; i++)
        {
            discounts.Add(discountLowest + (discountInterval * (i + 1)));
        }
        discounts.Add(discountHighest);

        //* TEMP:
        foreach (float discount in discounts)
        {
            Debug.Log(discount.ToString());
        }

        for (int c = 1; c <= supplierCount; c++)
        {
            currentGeneratedName = GenerateName();

            int iCurrentDiscount = Random.Range(0, discounts.Count);
            currentDiscount = discounts[iCurrentDiscount];
            discounts.RemoveAt(iCurrentDiscount);

            if (currentGeneratedName != "")
                Suppliers.Add(new SupplierAI(currentGeneratedName, currentDiscount));
            else
            {
                c = supplierCount + 1; //set sentinel value to end the FOR loop
                result = "Partial Success - " + Suppliers.Count.ToString() + " out of " + supplierCount.ToString() + " suppliers were generated!";
            }
        }

        if (Suppliers.Count == supplierCount)
            result = "Success - All " + supplierCount.ToString() + " suppliers were generated!";

        GameMaster.Instance.Log(result);
    }

    /// <summary>
    /// (WIP) Populates all Suppliers' inventories with new randomized items.
    /// </summary>
    public void PopulateSupplierInventories()
    {
        string result = GameMaster.MSG_ERR_DEFAULT;
        int numberOfItemsGenerated = 0;

        //1. Clear items:
        ClearAllSupplierInventoryItems();

        //2. Generate items:
        #region <FIELDS>
        ItemDatabaseSO itemDatabase = GameMaster.Instance.ItemManager.Database; //reference to the item database

        List<int> itemIDs = new List<int>();                                    //All item ID's in the database, excluding those of the specified excluded subcategories

        Dictionary<int, int> itemGenerations = new Dictionary<int, int>();      //Number of current generations per item (KEY = itemID, VALUE = number of generations)

        int[] numberOfItemsPerSupplier = new int[Suppliers.Count];              //Holds the number of items to be generated for each supplier. Parallel to 'Suppliers' list.
        #endregion

        //2.1 Populate List of item IDs for items to be used, excluding those with excluded subcategories:
        for (int i = 0; i < itemDatabase.Items.Count; i++)
        {
            bool exclude = false;

            foreach (ItemSubcategory excludedSubcategory in ExcludedItemSubcategories)
            {
                if (itemDatabase.Items[i].Subcategory.EnumID == excludedSubcategory)
                {
                    exclude = true;
                    break;
                }
            }

            if (!exclude)
                itemIDs.Add(i);
        }

        //2.2 Setup item generation count dictionary:
        for (int i = 0; i < itemIDs.Count; i++)
        {
            itemGenerations.Add(itemIDs[i], 0);
        }

        //2.3 Generate number of items to be added for each supplier:
        for (int iSupplier = 0; iSupplier < Suppliers.Count; iSupplier++)
        {
            numberOfItemsPerSupplier[iSupplier] = Random.Range(MinNumberOfItemsPerSupplier, MaxNumberOfItemsPerSupplier + 1);
        }

        //2.4 Generate items for each supplier
        for (int iSupplier = 0; iSupplier < Suppliers.Count; iSupplier++) //For each supplier...
        {
            //Copy 'itemIDs' list for temporary use so that the same item doesnt get generated for this supplier:
            List<int> itemIDsAvailableForSupplier = new List<int>();
            if (itemIDs.Count > 0)
            {
                foreach (int itemId in itemIDs)
                    itemIDsAvailableForSupplier.Add(itemId);
            }

            for (int itemNumber = 1; itemNumber <= numberOfItemsPerSupplier[iSupplier]; itemNumber++) //Perform item generation x times, where x = the number of items to be generated for this supplier...
            {
                if (itemIDsAvailableForSupplier.Count > 0) //if there are still items available to be generated...
                {
                    int iItem = Random.Range(0, itemIDsAvailableForSupplier.Count - 1); //index of the itemID in 'itemIDsAvailableForSupplier' list

                    int itemIDToBeAdded = itemIDsAvailableForSupplier[iItem]; //actual itemID

                    itemIDsAvailableForSupplier.RemoveAt(iItem); //Remove item from temp list so that it doesn't get generated again.

                    //Increment item generation counter for this generated item:
                    itemGenerations[itemIDToBeAdded] += 1;

                    //If this item has now been generated max number of times, remove it from the MASTER itemID list so that it doesn't get generated again for any supplier:
                    if (itemGenerations[itemIDToBeAdded] == MaxGenerationsPerItem)
                    {
                        int iItemInMaster = -1;

                        for (int i = 0; i < itemIDs.Count; i++)
                        {
                            if (itemIDs[i] == itemIDToBeAdded)
                            {
                                iItemInMaster = i;

                                i = itemIDs.Count; //end
                            }
                        }

                        itemIDs.RemoveAt(iItemInMaster);
                    }

                    //Finally, add the item to the supplier's inventory:
                    Suppliers[iSupplier].Inventory.AddItem(new Item(itemIDToBeAdded), out result);

                    //Increase master item count:
                    numberOfItemsGenerated += 1;
                }
            }
        }

        //TEMP:
        Debug.Log("--SUPPLIER INVENTORY GENERATION RESULTS--");
        Debug.Log(string.Format("* Number of items generated in total: {0}", numberOfItemsGenerated.ToString()));
        foreach (int key in itemGenerations.Keys)
        {
            Debug.Log(string.Format("- {0} x '{1}' generated!", itemGenerations[key].ToString(), itemDatabase.Items[key].Name));
        }
        Debug.Log("*<OK!>");
    }

    /// <summary>
    /// Returns a list of item ID's of items that are only held by the specified supplier.
    /// </summary>
    /// <param name="iSupplier"></param>
    /// <returns></returns>
    public int[] GetSupplierUniqueItems(int iSupplier)
    {
        //First, add all of this supplier's item IDs to the list:
        List<int> itemIDs = new List<int>();
        for (int iItem = 0; iItem < Suppliers[iSupplier].Inventory.Items.Count; iItem++)
        {
            itemIDs.Add(Suppliers[iSupplier].Inventory.Items[iItem].ItemID);
        }

        for (int iOtherSupplier = 0; iOtherSupplier < Suppliers.Count; iOtherSupplier++) //for each supplier...
        {
            if (iOtherSupplier != iSupplier)
            {
                for (int iItemOtherSupplier = 0; iItemOtherSupplier < Suppliers[iOtherSupplier].Inventory.Items.Count; iItemOtherSupplier++) //for each item in the supplier's inventory...
                {
                    int itemIdOtherSupplier = Suppliers[iOtherSupplier].Inventory.Items[iItemOtherSupplier].ItemID; //store the item's ID...

                    for (int iItemId = 0; iItemId < itemIDs.Count; iItemId++) //loop through current supplier's item ID list...
                    {
                        int itemId = itemIDs[iItemId]; //store item ID...

                        if (itemId == itemIdOtherSupplier) //if item ID is equal to the item ID of the other supplier's item AND the list of indexes to remove does NOT already contain the current item ID's index...
                            itemIDs[iItemId] = -1;
                    }
                }
            }
        }

        List<int> itemIDsUnique = new List<int>();

        foreach (int itemId in itemIDs)
        {
            if (itemId != -1)
                itemIDsUnique.Add(itemId);
        }


        return itemIDsUnique.ToArray();
    }

    /// <summary>
    /// Clears all Suppliers' inventories.
    /// </summary>
    private void ClearAllSupplierInventoryItems()
    {
        for (int i = 0; i < Suppliers.Count; i++)
        { Suppliers[i].Inventory.ClearInventory(); }
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
