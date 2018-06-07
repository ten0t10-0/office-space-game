﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Office Item", menuName = "Office Customization/Item")]
public class OfficeItemSO : ScriptableObject
{
    public string Name;
    public OfficeItemTypeSO Type;
    public GameObject Object;
}