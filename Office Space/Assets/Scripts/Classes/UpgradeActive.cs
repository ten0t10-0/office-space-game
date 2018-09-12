using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[System.Serializable]
public class UpgradeActive
{
    public int UpgradeID { get; set; }
    public int TimeRemaining { get; set; }

    public UpgradeActive(int id)
    {
        UpgradeID = id;

        TimeRemaining = SO.Days * 1440;
    }

    public UpgradeActiveSO SO
    {
        get
        {
            return GameMaster.Instance.UpgradeManager.Upgrades_Active[UpgradeID];
        }
    }
}
