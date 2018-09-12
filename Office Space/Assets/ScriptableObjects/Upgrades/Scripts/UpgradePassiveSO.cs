using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Permanent upgrades

[CreateAssetMenu(fileName = "New Passive Upgrade", menuName = "Upgrades/Passive")]
public class UpgradePassiveSO : ScriptableObject
{
    public string Name;
    public string Description;
    public UpgradeField UpgradeField;
    public float Value;
    public float Cost;
}
