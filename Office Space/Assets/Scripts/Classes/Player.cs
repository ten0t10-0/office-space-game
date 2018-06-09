using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string Name { get; private set; }
    public SupplierPlayer Business { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public float PlayTime { get; set; }

    public CharacterCustomizationData CharacterCustomizationData;
    public OfficeCustomizationData OfficeCustomizationData;

    [SerializeField]
    public List<int> UnlockedClothing { get; set; }
    [SerializeField]
    public List<int> UnlockedOfficeItems { get; set; }

    #region <Constructors>

    /// <summary>
    /// Creates a NEW player. Default clothing unlocked and activated in customization data. Default Office items unlocked.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="businessName"></param>
    /// <param name="startingMoney"></param>
    /// <param name="maximumInventorySpace"></param>
    /// <param name="maximumShopInventorySpace"></param>
    public Player(string name, string businessName, float startingMoney, float maximumInventorySpace, float maximumShopInventorySpace)
    {
        Name = name;

        Business = new SupplierPlayer(businessName, startingMoney, maximumInventorySpace, maximumShopInventorySpace);

        Level = 1;
        Experience = 0;
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
    public void IncreaseExperience(int amount)
    {
        Experience += amount;

        if (Experience >= GetExperienceRequiredForNextLevel())
        {
            //*
        }
    }

    public int GetExperienceRequiredForNextLevel()
    {
        int expRequired = 0;

        //*

        return expRequired;
    }

    private int GetLevelExperience(int level)
    {
        int expBase = GameMaster.Instance.PlayerExperienceBase;

        //*

        return -1;
    }

    private void IncreaseLevel()
    {
        //*
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return string.Format("Name: {0}; Level: {1}; Experience: {2}; PlayTime: {3}", Name, Level.ToString(), Experience.ToString(), PlayTime.ToString());
    }
}
