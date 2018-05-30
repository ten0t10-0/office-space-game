using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string Name { get; private set; }
    public int TotalScore { get; private set; }
    public SupplierPlayer Business { get; private set; }
    public float PlayTime { get; set; }

    public CharacterCustomizationData CharacterCustomizationData;
    public OfficeCustomizationData OfficeCustomizationData;

    [SerializeField]
    public List<int> UnlockedClothing { get; set; }
    [SerializeField]
    public List<int> UnlockedOfficeItems { get; set; }

    #region <Constructors>

    //Creates a new player. Default clothing unlocked & equipped. Default Office items unlocked.
    public Player(string name, string businessName, float startingMoney, float maximumInventorySpace, float maximumShopInventorySpace)
    {
        Name = name;

        Business = new SupplierPlayer(businessName, startingMoney, maximumInventorySpace, maximumShopInventorySpace);

        TotalScore = 0;
        PlayTime = 0f;

        CharacterCustomizationData = new CharacterCustomizationData(GameMaster.Instance.CustomizationManager.Character.MaterialBodyDefault.color);
        UnlockedClothing = new List<int>();

        foreach (int i in GameMaster.Instance.CustomizationManager.Character.DefaultClothingIndexes)
        {
            UnlockedClothing.Add(i);

            CharacterCustomizationData.CurrentClothing.Add(new CharacterClothing(i));
        }

        OfficeCustomizationData = new OfficeCustomizationData(GameMaster.Instance.CustomizationManager.Office.MaterialWallsDefault.color, GameMaster.Instance.CustomizationManager.Office.MaterialFloorDefault.color, GameMaster.Instance.CustomizationManager.Office.MaterialCeilingDefault.color);
        UnlockedOfficeItems = new List<int>();

        foreach (int i in GameMaster.Instance.CustomizationManager.Office.DefaultOfficeItemIndexes)
        {
            UnlockedOfficeItems.Add(i);
        }
    }
    #endregion

    #region <Methods>

    /// <summary>
    /// Returns a Customer object representing the player.
    /// </summary>
    /// <returns></returns>
    public Customer ToCustomer()
    {
        return new Customer(this.Name);
    }

    public void IncreaseScore(int increment)
    {
        TotalScore += increment;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return string.Format("Name: {0}; Total Score {1}; PlayTime: {2}", Name, TotalScore.ToString(), PlayTime.ToString());
    }
}
