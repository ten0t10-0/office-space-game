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
    public bool HasLifeLine { get; set; }

    public CharacterCustomizationData CharacterCustomizationData;
    public OfficeCustomizationData OfficeCustomizationData;

    [SerializeField]
    public List<int> UnlockedClothing { get; set; }
    [SerializeField]
    public List<int> PurchasedClothing { get; set; }
    [SerializeField]
    public List<int> UnlockedOfficeItems { get; set; }

    [SerializeField]
    public List<int> UnlockedAchievements { get; set; }

    [SerializeField]
    public List<int> UnlockedUpgradesPassive { get; set; }
    [SerializeField]
    public List<UpgradeActive> CurrentUpgradesActive { get; set; }

    #region <Constructors>

    /// <summary>
    /// Creates a NEW player. Default clothing unlocked and activated in customization data. Default Office items unlocked.
    /// </summary>
    public Player(string name, string businessName, int initialLevel, float startingMoney, float initialMarkup, float maximumInventorySpace, int shopItemSlotCount)
    {
        Name = name;

        Business = new SupplierPlayer(businessName, startingMoney, initialMarkup, maximumInventorySpace, shopItemSlotCount);

        if (initialLevel < 1)
        {
            initialLevel = 1;
        } 
        Level = initialLevel;
        Experience = GetLevelExperience(initialLevel);

        PlayTime = 0f;

        HasLifeLine = true;

        CharacterCustomizationData = new CharacterCustomizationData(GameMaster.Instance.CustomizationManager.Character.MaterialBodyDefault.color);
        UnlockedClothing = new List<int>();
        PurchasedClothing = new List<int>();

        OfficeCustomizationData = new OfficeCustomizationData(GameMaster.Instance.CustomizationManager.Office.MaterialWallsDefault.color, GameMaster.Instance.CustomizationManager.Office.MaterialWallsDefault.color, GameMaster.Instance.CustomizationManager.Office.MaterialFloorDefault.color, GameMaster.Instance.CustomizationManager.Office.MaterialCeilingDefault.color);
        UnlockedOfficeItems = new List<int>();

        GetLevelUnlocks();

        foreach (int i in GameMaster.Instance.CustomizationManager.Character.DefaultClothingIndexes)
        {
            PurchasedClothing.Add(i);

            CharacterCustomizationData.AddClothingData(new CharacterClothing(i));
        }

        UnlockedAchievements = new List<int>();

        UnlockedUpgradesPassive = new List<int>();
        CurrentUpgradesActive = new List<UpgradeActive>();

        ////TEMP:
        //for (int i = 1; i <= 20; i++)
        //    Debug.Log("Level " + i + ": " + GetLevelExperience(i));
    }
    #endregion

    #region <Methods>
    public void UnlockAchievement(int achievementID)
    {
        if (!UnlockedAchievements.Contains(achievementID))
        {
            UnlockedAchievements.Add(achievementID);

			GameMaster.Instance.GUIManager.achiev.addAcheivment(GameMaster.Instance.AchievementManager.Achievements [achievementID].Name);
            GameMaster.Instance.Notifications.Add(string.Format("Achievement unlocked: '{0}'", GameMaster.Instance.AchievementManager.Achievements[achievementID].Name));
        }
    }

    public void IncreaseExperience(int amount)
    {
        Experience += amount;

        while (Experience >= GetLevelExperience(Level + 1))
        {
            IncreaseLevel();
        }
    }

	public int GetLevelExperience(float level)
    {
        float expBase = GameMaster.Instance.PlayerExperienceBase;

        return (int)((expBase * level) * (1 + (level / 2) - 0.5) - expBase);
    }

    private void IncreaseLevel()
    {
        Level++;

        GetLevelUnlocks();

        GameMaster.Instance.AchievementManager.CheckAchievementsByType(AchievementType.PlayerLevel);

        GameMaster.Instance.CheckDifficulty();

        if (Level >= GameMaster.Instance.ShopModeLevelRequirement)
        {
            GameMaster.Instance.ShopUnlocked = true;
            GameMaster.Instance.Notifications.Add("SHOP UNLOCKED!");
        }
    }

    private void GetLevelUnlocks()
    {
        for (int i = 0; i < GameMaster.Instance.CustomizationManager.Character.Clothing.Count; i++)
        {
            CharacterClothingSO clothing = GameMaster.Instance.CustomizationManager.Character.Clothing[i];

            if (!clothing.Special && clothing.LevelRequirement <= Level)
            {
                if (!UnlockedClothing.Contains(i))
                {
                    UnlockedClothing.Add(i);

                    GameMaster.Instance.Notifications.Add(string.Format("Clothing unlocked: '{0}'", clothing.Name));
                }
            }
        }

        for (int i = 0; i < GameMaster.Instance.CustomizationManager.Office.Items.Count; i++)
        {
            if (GameMaster.Instance.CustomizationManager.Office.Items[i].LevelRequirement <= Level)
            {
                if (!UnlockedOfficeItems.Contains(i))
                {
                    UnlockedOfficeItems.Add(i);

                    GameMaster.Instance.Notifications.Add(string.Format("Office item unlocked: '{0}'", GameMaster.Instance.CustomizationManager.Office.Items[i].Name));
                }
            }
        }
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return string.Format("Name: {0}; Level: {1}; Experience: {2}; PlayTime: {3}", Name, Level.ToString(), Experience.ToString(), PlayTime.ToString());
    }
}
