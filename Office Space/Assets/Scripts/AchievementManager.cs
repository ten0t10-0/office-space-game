using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType { PlayerMoney, PlayerLevel, PlayerPlayTime, OrdersCompleted, OrdersFailed, UpgradeCount }

public class AchievementManager : MonoBehaviour
{
    public List<AchievementSO> Achievements;

    public void CheckAllAchievements()
    {
        CheckAchievementsByType(AchievementType.OrdersCompleted);
        CheckAchievementsByType(AchievementType.OrdersFailed);
        CheckAchievementsByType(AchievementType.PlayerLevel);
        CheckAchievementsByType(AchievementType.PlayerMoney);
        CheckAchievementsByType(AchievementType.PlayerPlayTime);
        CheckAchievementsByType(AchievementType.UpgradeCount);
    }

    public List<int> GetAchievementIDsByType(AchievementType achievementType)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < Achievements.Count; i++)
        {
            if (Achievements[i].AchievementType == achievementType)
            {
                result.Add(i);
            }
        }

        return result;
    }

    public void CheckAchievementsByType(AchievementType achievementType)
    {
        List<int> achievementList = GetAchievementIDsByType(achievementType);

        float? achievementField = GetAchievementField(achievementType);

        if (achievementField.HasValue)
        {
            foreach (int achievementID in achievementList)
            {
                if (achievementField.Value >= Achievements[achievementID].TargetValue)
                {
                    GameMaster.Instance.Player.UnlockAchievement(achievementID);
                }
            }
        }
        else
        { Debug.Log(string.Format("No logic set for achievement type '{0}'!", achievementType.ToString())); }
    }

    private float? GetAchievementField(AchievementType achievementType)
    {
        float? genericField = null;

        switch (achievementType)
        {
            case AchievementType.OrdersCompleted:
                genericField = GameMaster.Instance.OrderManager.GetCompletedOrders().Count;
                break;

            case AchievementType.OrdersFailed:
                genericField = GameMaster.Instance.OrderManager.GetFailedOrders().Count;
                break;

            case AchievementType.PlayerLevel:
                genericField = GameMaster.Instance.Player.Level;
                break;

            case AchievementType.PlayerMoney:
                genericField = GameMaster.Instance.Player.Business.Money;
                break;

            case AchievementType.PlayerPlayTime:
                genericField = GameMaster.Instance.Player.PlayTime;
                break;
        }

        return genericField;
    }
}
