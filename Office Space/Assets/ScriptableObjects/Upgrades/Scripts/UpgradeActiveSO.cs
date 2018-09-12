using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Temporary upgrades

[CreateAssetMenu(fileName = "New Active Upgrade", menuName = "Upgrades/Active")]
public class UpgradeActiveSO : ScriptableObject
{
    public string Name;
    public string Description;
    public UpgradeField UpgradeField;
    public float Value;
    public float Cost;
    public int Days;
}
