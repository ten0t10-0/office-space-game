using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeField { PlayerMarkup, CustomerTolerance, PlayerInventorySpace }

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradePassiveSO> Upgrades_Passive;
    public List<UpgradeActiveSO> Upgrades_Active;

    public void PurchaseActiveUpgrade(int id)
    {
        UpgradeActive ua = new UpgradeActive(id);

        GameMaster.Instance.Player.Business.DecreaseMoney(ua.SO.Cost);
        GameMaster.Instance.Player.CurrentUpgradesActive.Add(ua);
    }

    public void PurchasePassiveUpgrade(int id)
    {
        string result;

        UpgradePassiveSO upSO = GameMaster.Instance.UpgradeManager.Upgrades_Passive[id];

        GameMaster.Instance.Player.Business.DecreaseMoney(upSO.Cost);
        GameMaster.Instance.Player.UnlockedUpgradesPassive.Add(id);

        switch (upSO.UpgradeField)
        {
            case UpgradeField.PlayerInventorySpace:
                GameMaster.Instance.Player.Business.WarehouseInventory.IncreaseMaximumSpace(upSO.Value, out result); break;

            case UpgradeField.PlayerMarkup:
                GameMaster.Instance.Player.Business.MarkupPercentage += upSO.Value; break;
        }
    }

    public void UpdateActiveUpgrades()
    {
        List<UpgradeActive> upgrades = GameMaster.Instance.Player.CurrentUpgradesActive;

        if (upgrades.Count > 0)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].TimeRemaining -= (1 * GameMaster.Instance.GameModeManager.Office.GameTimeSpeed);

                Debug.Log("Time remaining: " + upgrades[i].TimeRemaining.ToString());

                if (upgrades[i].TimeRemaining == 0)
                {
                    EndActiveUpgrade(i);
                    i--;
                }
            }
        }
    }

    public void EndActiveUpgrade(int playerCurrentUpgradeID)
    {
        UpgradeActiveSO uaSO = GameMaster.Instance.Player.CurrentUpgradesActive[playerCurrentUpgradeID].SO;

        GameMaster.Instance.Player.CurrentUpgradesActive.RemoveAt(playerCurrentUpgradeID);

        GameMaster.Instance.Notifications.Add(string.Format("Active upgrade '{0}' has ended!", uaSO.Name));
    }
}
