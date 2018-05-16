using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterClothing
{
    public int ClothingID { get; set; }
    public int CustomMaterialID { get; set; }

    public float ColorR { get; set; }
    public float ColorG { get; set; }
    public float ColorB { get; set; }
    public float ColorA { get; set; }

    #region <Constructors>
    /// <summary>
    /// Create a reference to a clothing item in the DB. Default material with custom color used.
    /// </summary>
    /// <param name="clothingId"></param>
    /// <param name="color"></param>
    public CharacterClothing(int clothingId, Color color)
    {
        ClothingID = clothingId;
        UpdateColorData(color);

        CustomMaterialID = -1;
    }

    /// <summary>
    /// Create a reference to a clothing item in the DB. Default material used.
    /// </summary>
    /// <param name="clothingId"></param>
    public CharacterClothing(int clothingId)
    {
        ClothingID = clothingId;
        UpdateColorData(GetClothingSO().ClothingSlot.MaterialDefault.color);

        CustomMaterialID = -1;
    }

    /// <summary>
    /// Create a reference to a clothing item in the DB. Custom material used.
    /// </summary>
    /// <param name="clothingId"></param>
    /// <param name="materialId"></param>
    public CharacterClothing(int clothingId, int materialId)
    {
        ClothingID = clothingId;
        CustomMaterialID = materialId;

        UpdateColorData(GameMaster.Instance.CustomizationManager.Character.CustomMaterials[materialId].color);
    }
    #endregion

    #region <Methods>
    public CharacterClothingSO GetClothingSO()
    {
        return GameMaster.Instance.CustomizationManager.Character.Clothing[ClothingID];
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

    public bool HasCustomMaterial()
    {
        return CustomMaterialID > -1;
    }
    #endregion
}
