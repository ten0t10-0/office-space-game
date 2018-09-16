using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Office Item", menuName = "Office Customization/Item")]
public class OfficeItemSO : ScriptableObject
{
    public string Name;
    public float Price;
    public bool Essential;
    public int LevelRequirement;
    public OfficeItemPosition Placement;
    public OfficeItemCategory Category;
    public GameObject Object;
}
