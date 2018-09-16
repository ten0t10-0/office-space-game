using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class SaveData
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    [SerializeField]
    public GameData GameData { get; set; }
}
