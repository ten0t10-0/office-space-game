using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Office Item Type", menuName = "Office Customization/Item Type")]
public class OfficeItemTypeSO : ScriptableObject
{
    public string Name;
    public OfficeItemCategory Category;
}
