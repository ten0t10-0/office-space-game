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
	public Dictionary<ClothingSlot, int> ClothingObjects;
    /// <summary>
    /// Key = Clothing Slot; Value = childIndex
    /// </summary>
    private Dictionary<ClothingSlot, int> BodyObjects;

    [HideInInspector]
    public Transform Transform_Hip;
    [HideInInspector]
    public Transform Transform_Head;
    [HideInInspector]
    public Transform Transform_HandL;
    [HideInInspector]
    public Transform Transform_HandR;

    private void Awake()
    {
        GameObject objHandL, objHandR;

        ClothingObjects = new Dictionary<ClothingSlot, int>();
        BodyObjects = new Dictionary<ClothingSlot, int>();

        Transform_Hip = transform.Find("Player").Find("ROOT").Find("Hip_CONT").Find("Hip");
        Transform_Head = Transform_Hip.Find("Spine").Find("Chest").Find("Neck").Find("Head").gameObject.transform;

        objHandL = new GameObject(name + "_Hand-L");
        objHandR = new GameObject(name + "_Hand-R");

        objHandL.transform.parent = Transform_Hip.Find("Spine").Find("Chest").Find("Collar_L").Find("Arm1_L").Find("Arm2_L").gameObject.transform;
        objHandR.transform.parent = Transform_Hip.Find("Spine").Find("Chest").Find("Collar_R").Find("Arm1_R").Find("Arm2_R").gameObject.transform;

        objHandL.transform.position = objHandL.transform.parent.position + (objHandL.transform.parent.right * -1 * GameMaster.Instance.CustomizationManager.Character.CharacterHandObjOffset);
        objHandR.transform.position = objHandR.transform.parent.position + (objHandR.transform.parent.right * -1 * GameMaster.Instance.CustomizationManager.Character.CharacterHandObjOffset);

        Transform_HandL = objHandL.transform;
        Transform_HandR = objHandR.transform;

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

    public void SetAccessory(int accessoryId)
    {
        if (!IsAccessoryEquipped(accessoryId))
        {
            if (GetAccessoryObjects().Count < GameMaster.Instance.CustomizationManager.Character.MaxAccessories)
            {
                Transform headTransform = transform.Find("Player").Find("ROOT").Find("Hip_CONT").Find("Hip").Find("Spine").Find("Chest").Find("Neck").Find("Head").gameObject.transform;

                GameObject newAccObj = Instantiate(GameMaster.Instance.CustomizationManager.Character.Accessories[accessoryId].GameObject, transform);
                newAccObj.transform.parent = headTransform;

                newAccObj.GetComponent<AccessoryScript>().Initialize(accessoryId);
            }
            else
                Debug.Log("*Maximum number of accessories reached!");
        }
    }

    public void UnsetAccessory(int accessoryId)
    {
        GameObject obj = null;

        IsAccessoryEquipped(accessoryId, out obj);

        if (obj != null)
        {
            Destroy(obj);
        }
    }

    public bool IsAccessoryEquipped(int accessoryId, out GameObject obj)
    {
        bool equipped = false;

        List<GameObject> accObjs = GetAccessoryObjects();

        obj = null;

        for (int i = 0; i < accObjs.Count; i++)
        {
            if (accObjs[i].GetComponent<AccessoryScript>().AccessoryIndex == accessoryId)
            {
                equipped = true;
                obj = accObjs[i];

                i = accObjs.Count; //break;
            }
        }

        return equipped;
    }

    public bool IsAccessoryEquipped(int accessoryId)
    {
        bool equipped = false;

        List<GameObject> accObjs = GetAccessoryObjects();

        for (int i = 0; i < accObjs.Count; i++)
        {
            if (accObjs[i].GetComponent<AccessoryScript>().AccessoryIndex == accessoryId)
            {
                equipped = true;

                i = accObjs.Count; //break;
            }
        }

        return equipped;
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
        UnsetAllAccessories();

        foreach (ClothingSlot slot in clothingDictionary.Keys)
        {
            SetClothing(clothingDictionary[slot].ClothingID);

            UpdateClothingColor(slot, clothingDictionary[slot].GetColor());
        }

        foreach (CharacterAccessory accessory in customizationData.CurrentAccessories)
        {
            SetAccessory(accessory.AccessoryID);

            UpdateAccessoryColor(accessory.AccessoryID, accessory.GetColor());
        }
    }

    public void SetAccessoriesByPreset(CharacterAccessoryPresetSO preset)
    {
        UnsetAllAccessories();

        foreach (int accessoryId in preset.AccessoryIDList)
        {
            SetAccessory(accessoryId);
        }
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
        ClothingSlotScript clothingSlotScript = transform.GetChild(ClothingObjects[slot]).gameObject.GetComponent<ClothingSlotScript>();

        if (!clothingSlotScript.HasCustomMaterial)
            clothingSlotScript.gameObject.GetComponent<Renderer>().sharedMaterial.color = newClothingColor;
        else
            Debug.Log("***Cannot change color of clothing object with custom material!");
    }

    public void UpdateAccessoryColor(int accessoryId, Color newColor)
    {
        List<GameObject> accObjs = GetAccessoryObjects();

        if (accObjs.Count > 0)
        {
            for (int i = 0; i < accObjs.Count; i++)
            {
                if (accObjs[i].GetComponent<AccessoryScript>().AccessoryIndex == accessoryId)
                {
                    accObjs[i].GetComponent<Renderer>().sharedMaterial.color = newColor;
                }
            }
        }
        else
            Debug.Log("*No Accessory objects found!");
    }

    public CharacterCustomizationData GetCustomizationData()
    {
        CharacterCustomizationData data = new CharacterCustomizationData(MaterialBody.color);

        Dictionary<ClothingSlot, CharacterClothing> dictionary = new Dictionary<ClothingSlot, CharacterClothing>();

        List<GameObject> accessoryObjects = GetAccessoryObjects();

        foreach (ClothingSlot slot in ClothingObjects.Keys)
        {
            if (IsClothingSlotUsed(slot))
            {
                GameObject clothingObj = transform.GetChild(ClothingObjects[slot]).gameObject;

                CharacterClothing characterClothing = new CharacterClothing(clothingObj.GetComponent<ClothingSlotScript>().ClothingIndex, clothingObj.GetComponent<Renderer>().sharedMaterial.color);

                dictionary.Add(slot, characterClothing);
            }
        }

        foreach (GameObject accObj in accessoryObjects)
        {
            CharacterAccessory charAcc = new CharacterAccessory(accObj.GetComponent<AccessoryScript>().AccessoryIndex, accObj.GetComponent<Renderer>().sharedMaterial.color);

            data.CurrentAccessories.Add(charAcc);
        }

        data.SetUpLists(dictionary);

        //TEMP:
        Debug.Log("GENERATE CUSTOMIZATION DATA:");
        Debug.Log("Body Color: " + data.GetBodyColor().ToString());
        for (int i = 0; i < data.CurrentClothing_Keys.Count; i++)
        {
            Debug.Log("*" + data.CurrentClothing_Keys[i].ToString() + "* - Clothing ID: " + data.CurrentClothing_Values[i].ClothingID.ToString() + "; Color: " + data.CurrentClothing_Values[i].GetColor().ToString());
        }
        for (int i = 0; i < data.CurrentAccessories.Count; i++)
        {
            Debug.Log("*" + "ACC ID: " + data.CurrentAccessories[i].AccessoryID.ToString() + "; ACC Color: " + data.CurrentAccessories[i].GetColor().ToString());
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

        bool setOutfit = GameMaster.Roll(0.075f);
        bool setHead = GameMaster.Roll(0.05f);

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
            //Debug.Log(id);
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

    public void UnsetAllAccessories()
    {
        List<GameObject> accObj = GetAccessoryObjects();

        if (accObj.Count > 0)
        {
            for (int i = 0; i < accObj.Count; i++)
            {
                Destroy(accObj[i]);
            }
        }
    }

    public List<GameObject> GetAccessoryObjects()
    {
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < Transform_Head.childCount; i++)
        {
            GameObject child = Transform_Head.GetChild(i).gameObject;

            if (child.GetComponent<AccessoryScript>() != null)
                objects.Add(child);
        }

        return objects;
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
