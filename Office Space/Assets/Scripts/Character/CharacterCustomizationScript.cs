using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationScript : MonoBehaviour
{
    [HideInInspector]
    public Material MaterialBody;

    /// <summary>
    /// Key = Clothing Slot; Value = childIndex
    /// </summary>
    private Dictionary<ClothingSlot, int> ClothingObjects;
    /// <summary>
    /// Key = Clothing Slot; Value = childIndex
    /// </summary>
    private Dictionary<ClothingSlot, int> BodyObjects;

    private void Awake()
    {
        ClothingObjects = new Dictionary<ClothingSlot, int>();
        BodyObjects = new Dictionary<ClothingSlot, int>();

        //Get & use a copy of the default body material (and color)
        MaterialBody = new Material(GameMaster.Instance.CustomizationManager.Character.MaterialBodyDefault);
        MaterialBody.name += " (copy)";

        //Set up dictionaries & initialize all Clothing & Body placeholder objects
        for (int i = 0; i < transform.childCount; i++)
        {
            ClothingSlotScript clothingSlotScript = GetClothingSlotScript(i);

            if (clothingSlotScript != null)
            {
                clothingSlotScript.Initialize(this);

                if (!clothingSlotScript.IsBodyObject)
                {
                    ClothingObjects.Add(clothingSlotScript.ClothingSlot, i);
                }
                else
                {
                    BodyObjects.Add(clothingSlotScript.ClothingSlot, i);
                }
            }
        }

        //Remove all clothing (in this case, will remove/clear placeholder objects)
        UnsetAllClothing();
    }

    public void SetClothing(int clothingId)
    {
        CharacterClothingSO clothingSO = GameMaster.Instance.CustomizationManager.Character.Clothing[clothingId];

        GetClothingSlotScript(ClothingObjects[clothingSO.ClothingSlot.Slot]).Set(clothingId);

        if (clothingSO.HasBodyMesh)
        {
            GetClothingSlotScript(BodyObjects[clothingSO.ClothingSlot.Slot]).Set(clothingId);
        }

        switch (clothingSO.ClothingSlot.Slot)
        {
            case ClothingSlot.Costume:
                {
                    if (GetClothingSlotScript(ClothingObjects[ClothingSlot.Upper]).IsSet)
                        UnsetClothing(ClothingSlot.Upper);

                    if (GetClothingSlotScript(ClothingObjects[ClothingSlot.Lower]).IsSet)
                        UnsetClothing(ClothingSlot.Lower);

                    if (GetClothingSlotScript(ClothingObjects[ClothingSlot.LeftArm]).IsSet)
                        UnsetClothing(ClothingSlot.LeftArm);

                    if (GetClothingSlotScript(ClothingObjects[ClothingSlot.RightArm]).IsSet)
                        UnsetClothing(ClothingSlot.RightArm);

                    break;
                }

            case ClothingSlot.Upper:
            case ClothingSlot.Lower:
            case ClothingSlot.LeftArm:
            case ClothingSlot.RightArm:
                {
                    if (GetClothingSlotScript(ClothingObjects[ClothingSlot.Costume]).IsSet)
                        UnsetClothing(ClothingSlot.Costume);

                    break;
                }

            case ClothingSlot.Head:
                {
                    break;
                }
        }

        SetUpBodyObjects();
    }

    public void UnsetClothing(ClothingSlot slot)
    {
        GetClothingSlotScript(ClothingObjects[slot]).Unset();

        if (BodyObjects.ContainsKey(slot))
        {
            GetClothingSlotScript(BodyObjects[slot]).Unset();

            SetUpBodyObjects();
        }
    }

    /// <summary>
    /// Sets up the characters appearance according to the specified customization data.
    /// </summary>
    /// <param name="clothingList"></param>
    public void SetAppearanceByData(CharacterCustomizationData customizationData)
    {
        Dictionary<ClothingSlot, CharacterClothing> clothingDictionary = customizationData.GetDictionary();

        MaterialBody.color = customizationData.GetBodyColor();

        UnsetAllClothing();

        foreach (ClothingSlot slot in clothingDictionary.Keys)
        {
            SetClothing(clothingDictionary[slot].ClothingID);

            UpdateClothingColor(slot, clothingDictionary[slot].GetColor());
        }

        //ReloadCharacterAppearance();
    }

    /// <summary>
    /// Sets the character's body (skin) color to the specified color.
    /// </summary>
    /// <param name="newBodyColor"></param>
    public void UpdateBodyColor(Color newBodyColor)
    {
        MaterialBody.color = newBodyColor;
    }

    /// <summary>
    /// Sets the color of clothing in the specified clothing slot to the specified color.
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="newClothingColor"></param>
    public void UpdateClothingColor(ClothingSlot slot, Color newClothingColor)
    {
        transform.GetChild(ClothingObjects[slot]).gameObject.GetComponent<Renderer>().sharedMaterial.color = newClothingColor;
    }

    public CharacterCustomizationData GetCustomizationData()
    {
        CharacterCustomizationData data = new CharacterCustomizationData(MaterialBody.color);

        Dictionary<ClothingSlot, CharacterClothing> dictionary = new Dictionary<ClothingSlot, CharacterClothing>();

        foreach (ClothingSlot slot in ClothingObjects.Keys)
        {
            if (IsClothingSlotUsed(slot))
            {
                GameObject clothingObj = transform.GetChild(ClothingObjects[slot]).gameObject;

                CharacterClothing characterClothing = new CharacterClothing(clothingObj.GetComponent<ClothingSlotScript>().ClothingIndex, clothingObj.GetComponent<Renderer>().sharedMaterial.color);

                dictionary.Add(slot, characterClothing);
            }
        }

        data.SetUpLists(dictionary);

        //TEMP:
        Debug.Log("GENERATE CUSTOMIZATION DATA:");
        Debug.Log("Body Color: " + data.GetBodyColor().ToString());
        for (int i = 0; i < data.CurrentClothing_Keys.Count; i++)
        {
            Debug.Log("*" + data.CurrentClothing_Keys[i].ToString() + "* - Clothing ID: " + data.CurrentClothing_Values[i].ClothingID.ToString() + "; Color: " + data.CurrentClothing_Values[i].GetColor().ToString());
        }

        return data;
    }

    /// <summary>
    /// Checks if the specified clothing slot is currently filled/in use.
    /// </summary>
    /// <param name="clothingSlot"></param>
    /// <returns></returns>
    public bool IsClothingSlotUsed(ClothingSlot clothingSlot)
    {
        return GetClothingSlotScript(ClothingObjects[clothingSlot]).IsSet;
    }

    /// <summary>
    /// Returns the Index/ID of the clothing on the specified slot. A value of -1 being returned indicates there is no clothing.
    /// </summary>
    /// <param name="clothingSlot"></param>
    /// <returns></returns>
    public int GetClothingIndexBySlot(ClothingSlot clothingSlot)
    {
        return GetClothingSlotScript(ClothingObjects[clothingSlot]).ClothingIndex;
    }

    public void RandomizeAppearance()
    {
        CharacterCustomizationDatabaseSO db = GameMaster.Instance.CustomizationManager.Character;

        UnsetAllClothing();

        List<int> clothingToApply = new List<int>();

        bool setOutfit = GameMaster.Roll(0.15f);
        bool setHead = GameMaster.Roll(0.15f);

        bool setArmLeft = false;
        bool setArmRight = false;

        if (!setOutfit)
        {
            clothingToApply.Add(db.GetRandomClothingBySlot(ClothingSlot.Lower));
            clothingToApply.Add(db.GetRandomClothingBySlot(ClothingSlot.Upper));

            setArmLeft = GameMaster.Roll(0.05f);
            setArmRight = GameMaster.Roll(0.05f);

            if (setArmLeft)
            {
                clothingToApply.Add(db.GetRandomClothingBySlot(ClothingSlot.LeftArm));
            }
            if (setArmRight)
            {
                clothingToApply.Add(db.GetRandomClothingBySlot(ClothingSlot.RightArm));
            }
        }
        else
        {
            clothingToApply.Add(db.GetRandomClothingBySlot(ClothingSlot.Costume));
        }

        if (setHead)
        {
            clothingToApply.Add(db.GetRandomClothingBySlot(ClothingSlot.Head));
        }

        foreach (int id in clothingToApply)
        {
            SetClothing(id);
            Debug.Log(id);
        }

        foreach (ClothingSlot slot in ClothingObjects.Keys)
        {
            if (IsClothingSlotUsed(slot))
            {
                Color newColor = new Color
                {
                    r = Random.Range(0f, 1f),
                    g = Random.Range(0f, 1f),
                    b = Random.Range(0f, 1f)
                };

                UpdateClothingColor(slot, newColor);
            }
        }

        if (db.SkinColors.Count > 0)
        {
            UpdateBodyColor(db.SkinColors[Random.Range(0, db.SkinColors.Count)]);
        }
    }

    public void UnsetAllClothing()
    {
        foreach (CharacterClothingSlotSO slot in GameMaster.Instance.CustomizationManager.Character.ClothingSlots)
        {
            UnsetClothing(slot.Slot);
        }
    }

    private void SetUpBodyObjects()
    {
        bool isSlotUsedCostume = IsClothingSlotUsed(ClothingSlot.Costume);
        bool isSlotUsedUpper = IsClothingSlotUsed(ClothingSlot.Upper);
        bool isSlotUsedLower = IsClothingSlotUsed(ClothingSlot.Lower);

        if (isSlotUsedCostume)
        {
            UnsetBody(ClothingSlot.Upper);
            UnsetBody(ClothingSlot.Lower);
        }
        else
        {
            UnsetBody(ClothingSlot.Costume);

            if (!isSlotUsedUpper)
                SetBody(ClothingSlot.Upper);

            //if (!isSlotUsedLower)
            //    SetBody(ClothingSlot.Lower);

            if (!isSlotUsedLower)
                SetClothing(GameMaster.Instance.CustomizationManager.Character.GetDefaultClothingBySlot(ClothingSlot.Lower).Value);
        }
    }

    private void SetBody(ClothingSlot slot)
    {
        GameObject bodyObj = transform.GetChild(BodyObjects[slot]).gameObject;

        bodyObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = GameMaster.Instance.CustomizationManager.Character.GetClothingSlotSO(slot).BodyMesh;
        bodyObj.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    private void UnsetBody(ClothingSlot slot)
    {
        GameObject bodyObj = transform.GetChild(BodyObjects[slot]).gameObject;

        bodyObj.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
        bodyObj.GetComponent<SkinnedMeshRenderer>().enabled = false;
    }

    private ClothingSlotScript GetClothingSlotScript(int childIndex)
    {
        return transform.GetChild(childIndex).gameObject.GetComponent<ClothingSlotScript>();
    }

    private void OnDestroy()
    {
        Destroy(MaterialBody);
    }
}
