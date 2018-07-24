using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OfficeCustomizationData
{
    [SerializeField]
    public List<OfficeItem> OfficeItems { get; set; }

    [SerializeField]
    public List<OfficeObjectDependency> Dependencies { get; set; }

    public float ColorWallsR { get; set; }
    public float ColorWallsG { get; set; }
    public float ColorWallsB { get; set; }
    public float ColorWallsA { get; set; }

    public float ColorWallsShopR { get; set; }
    public float ColorWallsShopG { get; set; }
    public float ColorWallsShopB { get; set; }
    public float ColorWallsShopA { get; set; }

    public float ColorFloorR { get; set; }
    public float ColorFloorG { get; set; }
    public float ColorFloorB { get; set; }
    public float ColorFloorA { get; set; }

    public float ColorCeilingR { get; set; }
    public float ColorCeilingG { get; set; }
    public float ColorCeilingB { get; set; }
    public float ColorCeilingA { get; set; }

    #region <Constructors>
    public OfficeCustomizationData(Color colorWalls, Color colorWallsShop, Color colorFloor, Color colorCeiling)
    {
        OfficeItems = new List<OfficeItem>();
        Dependencies = new List<OfficeObjectDependency>();

        UpdateWallsColorData(colorWalls);
        UpdateFloorColorData(colorFloor);
        UpdateCeilingColorData(colorCeiling);
    }
    #endregion

    #region <Methods>
    public void UpdateWallsColorData(Color colorWalls)
    {
        ColorWallsR = colorWalls.r;
        ColorWallsG = colorWalls.g;
        ColorWallsB = colorWalls.b;
        ColorWallsA = colorWalls.a;
    }

    public void UpdateWallsShopColorData(Color colorWalls)
    {
        ColorWallsShopR = colorWalls.r;
        ColorWallsShopG = colorWalls.g;
        ColorWallsShopB = colorWalls.b;
        ColorWallsShopA = colorWalls.a;
    }

    public void UpdateFloorColorData(Color colorFloor)
    {
        ColorFloorR = colorFloor.r;
        ColorFloorG = colorFloor.g;
        ColorFloorB = colorFloor.b;
        ColorFloorA = colorFloor.a;
    }

    public void UpdateCeilingColorData(Color colorCeiling)
    {
        ColorCeilingR = colorCeiling.r;
        ColorCeilingG = colorCeiling.g;
        ColorCeilingB = colorCeiling.b;
        ColorCeilingA = colorCeiling.a;
    }

    public Color GetWallsColor()
    {
        return new Color(ColorWallsR, ColorWallsG, ColorWallsB, ColorWallsA);
    }

    public Color GetWallsShopColor()
    {
        return new Color(ColorWallsShopR, ColorWallsShopG, ColorWallsShopB, ColorWallsShopA);
    }

    public Color GetFloorColor()
    {
        return new Color(ColorFloorR, ColorFloorG, ColorFloorB, ColorFloorA);
    }

    public Color GetCeilingColor()
    {
        return new Color(ColorCeilingR, ColorCeilingG, ColorCeilingB, ColorCeilingA);
    }
    #endregion
}
