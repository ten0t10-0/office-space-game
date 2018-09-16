using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingSlot { Costume, Upper, Lower, Head, LeftArm, RightArm }

public enum OfficeItemCategory { Chairs, Lights, Tables, Miscellaneous, Stationery }
public enum OfficeItemPosition { Floor, Wall, Ceiling, None }

public class CustomizationManager : MonoBehaviour
{
    public CharacterCustomizationDatabaseSO Character;
    public OfficeItemDatabaseSO Office;
}
