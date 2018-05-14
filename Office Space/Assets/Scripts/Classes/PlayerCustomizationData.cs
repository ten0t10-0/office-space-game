using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCustomizationData
{
    [SerializeField]
    public List<PlayerClothing> CurrentClothing { get; set; }
    [SerializeField]
    public List<PlayerClothing> UnlockedClothing { get; set; }

    //...

    public PlayerCustomizationData(List<int> defaultUnlockedClothingList)
    {
        CurrentClothing = new List<PlayerClothing>();

        UnlockedClothing = new List<PlayerClothing>();
        foreach (int i in defaultUnlockedClothingList)
        {
            UnlockedClothing.Add(new PlayerClothing(i));
        }
    }
}
