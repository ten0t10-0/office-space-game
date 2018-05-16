using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingSlot { Costume, Upper, Lower, Head, LeftArm, RightArm }

public enum OfficeItemCategory { Furniture, Decorations, Electronics, Miscellaneous }

public class CustomizationManager : MonoBehaviour
{
    public CharacterCustomizationDatabaseSO Character;
    public OfficeItemDatabaseSO Office;

    private void Awake()
    {
        Office.Initialize();
    }
}
