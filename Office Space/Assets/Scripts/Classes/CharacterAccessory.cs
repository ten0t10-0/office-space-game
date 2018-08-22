using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAccessory
{
    public int AccessoryID { get; set; }

    public float ColorR { get; set; }
    public float ColorG { get; set; }
    public float ColorB { get; set; }
    public float ColorA { get; set; }

    #region <Constructors>
    /// <summary>
    /// Create a reference to a clothing item in the DB. Default material with custom color used.
    /// </summary>
    /// <param name="accessoryId"></param>
    /// <param name="color"></param>
    public CharacterAccessory(int accessoryId, Color color)
    {
        AccessoryID = accessoryId;
        UpdateColorData(color);
    }

    /// <summary>
    /// Create a reference to a clothing item in the DB. Default material used.
    /// </summary>
    /// <param name="accessoryId"></param>
    public CharacterAccessory(int accessoryId)
    {
        AccessoryID = accessoryId;
        UpdateColorData(GetAccessorySO().GameObject.GetComponent<Renderer>().sharedMaterial.color);
    }
    #endregion

    #region <Methods>
    public CharacterAccessorySO GetAccessorySO()
    {
        return GameMaster.Instance.CustomizationManager.Character.Accessories[AccessoryID];
    }

    public Color GetColor()
    {
        return new Color(ColorR, ColorG, ColorB, ColorA);
    }

    public void UpdateColorData(Color color)
    {
        ColorR = color.r;
        ColorG = color.g;
        ColorB = color.b;
        ColorA = color.a;
    }
    #endregion
}
