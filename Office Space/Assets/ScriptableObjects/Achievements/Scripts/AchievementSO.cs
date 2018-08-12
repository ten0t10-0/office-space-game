using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievements/Achievement")]
public class AchievementSO : ScriptableObject
{
    public string Name;
    public AchievementType AchievementType;
    public int TargetValue;
}
