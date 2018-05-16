using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterCustomizationData
{
    [SerializeField]
    public List<CharacterClothing> CurrentClothing { get; set; }

    public float BodyColorR { get; set; }
    public float BodyColorG { get; set; }
    public float BodyColorB { get; set; }
    public float BodyColorA { get; set; }

    #region <Constructors>
    public CharacterCustomizationData(Color bodyColor)
    {
        CurrentClothing = new List<CharacterClothing>();
        UpdateBodyColorInfo(bodyColor);
    }
    #endregion

    #region <Methods>
    public void UpdateBodyColorInfo(Color bodyColor)
    {
        BodyColorR = bodyColor.r;
        BodyColorG = bodyColor.g;
        BodyColorB = bodyColor.b;
        BodyColorA = bodyColor.a;
    }

    public Color GetBodyColor()
    {
        return new Color(BodyColorR, BodyColorG, BodyColorB, BodyColorA);
    }
    #endregion
}
