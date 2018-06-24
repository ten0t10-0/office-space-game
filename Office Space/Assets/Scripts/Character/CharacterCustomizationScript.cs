using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationScript : MonoBehaviour
{
    public Material MaterialBlank;
    [HideInInspector]
    public Material MaterialBody;

    [HideInInspector]
    public CharacterCustomizationData CustomizationData;

    private void Awake()
    {
        //Get & use default body material (and color)
        MaterialBody = new Material(GameMaster.Instance.CustomizationManager.Character.MaterialBodyDefault);

        //Remove placeholder objects:
        for (int i = 0; i < GameMaster.Instance.CustomizationManager.Character.ClothingSlots.Count; i++)
        {
            UnsetObject(transform.Find(GameMaster.Instance.CustomizationManager.Character.ClothingSlots[i].PlaceholderNames[0]).gameObject);

            if (!GameMaster.Instance.CustomizationManager.Character.ClothingSlots[i].IsAccessory)
            {
                UnsetObject(transform.Find(GameMaster.Instance.CustomizationManager.Character.ClothingSlots[i].PlaceholderNames[1]).gameObject);
            }
        }
    }

    /// <summary>
    /// Replaces the customization data with specified customization data, and reloads the character.
    /// </summary>
    /// <param name="clothingList"></param>
    public void SetAppearanceByData(CharacterCustomizationData customizationData)
    {
        CustomizationData = customizationData;

        ReloadCharacterAppearance();
    }

    /// <summary>
    /// Binds clothing to the character based on the CurrentClothing list in the customization data.
    /// </summary>
    public void ReloadCharacterAppearance()
    {
        MaterialBody.color = CustomizationData.GetBodyColor();

        if (CustomizationData.CurrentClothing.Count > 0)
        {
            UnsetAllClothing();

            bool isSlotUsedCostume = IsClothingSlotUsed(ClothingSlot.Costume);
            bool isSlotUsedUpper = IsClothingSlotUsed(ClothingSlot.Upper);
            bool isSlotUsedLower = IsClothingSlotUsed(ClothingSlot.Lower);

            if (!isSlotUsedCostume && !isSlotUsedUpper && !isSlotUsedLower)
            {
                SetObjectBody(GameMaster.Instance.CustomizationManager.Character.GetClothingSlotSO(ClothingSlot.Costume).Body);
            }
            else if (!isSlotUsedCostume)
            {
                if (isSlotUsedLower && !isSlotUsedUpper)
                {
                    SetObjectBody(GameMaster.Instance.CustomizationManager.Character.GetClothingSlotSO(ClothingSlot.Upper).Body);
                }
                else if (isSlotUsedUpper && !isSlotUsedLower)
                {
                    SetObjectBody(GameMaster.Instance.CustomizationManager.Character.GetClothingSlotSO(ClothingSlot.Lower).Body);
                }
            }

            for (int i = 0; i < CustomizationData.CurrentClothing.Count; i++)
            {
                ClothingSlot clothingSlot = CustomizationData.CurrentClothing[i].GetClothingSO().ClothingSlot.Slot;

                if (!isSlotUsedCostume)
                {
                    SetObject(CustomizationData.CurrentClothing[i]);
                }
                else
                {
                    if (clothingSlot == ClothingSlot.Costume || clothingSlot == ClothingSlot.Head)
                        SetObject(CustomizationData.CurrentClothing[i]);
                }
            }
        }
        else
        {
            SetObjectBody(GameMaster.Instance.CustomizationManager.Character.GetClothingSlotSO(ClothingSlot.Costume).Body);
        }
    }

    /// <summary>
    /// Adds the specified clothing item onto the list and refreshes the character.
    /// </summary>
    /// <param name="charClothing"></param>
    public void AddClothing(CharacterClothing charClothing)
    {
        AddClothingToList(charClothing);

        ReloadCharacterAppearance();
    }

    /// <summary>
    /// Removes the clothing item in the specified slot from the list and refreshes the character.
    /// </summary>
    /// <param name="clothingSlot"></param>
    public void RemoveClothing(CharacterClothingSlotSO clothingSlot)
    {
        RemoveClothingFromList(clothingSlot);

        ReloadCharacterAppearance();
    }

    /// <summary>
    /// Updates the character's body (skin) color to the specified color and updates customization data.
    /// </summary>
    /// <param name="newBodyColor"></param>
    public void UpdateBodyColor(Color newBodyColor)
    {
        MaterialBody.color = newBodyColor;

        CustomizationData.UpdateBodyColorInfo(newBodyColor);
    }

    private void AddClothingToList(CharacterClothing clothing)
    {
        int iClothing;

        if (IsClothingSlotUsed(clothing.GetClothingSO().ClothingSlot.Slot, out iClothing))
            CustomizationData.CurrentClothing.RemoveAt(iClothing);

        CustomizationData.CurrentClothing.Add(clothing);
    }

    private void RemoveClothingFromList(CharacterClothingSlotSO clothingSlot)
    {
        int iClothing;

        if (IsClothingSlotUsed(clothingSlot.Slot, out iClothing))
        {
            CustomizationData.CurrentClothing.RemoveAt(iClothing);
        }
        else
        {
            Debug.Log(string.Format("*Clothing slot '{0}' not in use.", clothingSlot.Name));
        }
    }

    /// <summary>
    /// Removes all clothing from all placeholder objects according to the list.
    /// </summary>
    private void UnsetAllClothing()
    {
        if (CustomizationData.CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CustomizationData.CurrentClothing.Count; i++)
            {
                UnsetClothing(CustomizationData.CurrentClothing[i].GetClothingSO().ClothingSlot);
            }
        }
    }

    /// <summary>
    /// Sets/Enables the Mesh, Material and SkinnedMeshRenderer component of the GameObject found using the specified clothing item.
    /// </summary>
    /// <param name="charClothing"></param>
    private void SetObject(CharacterClothing charClothing)
    {
        CharacterClothingSO charClothingSO = charClothing.GetClothingSO();

        GameObject clothingObject = null;

        if (charClothingSO.ClothingSlot.PlaceholderNames[0] != "")
        {
            clothingObject = transform.Find(charClothingSO.ClothingSlot.PlaceholderNames[0]).gameObject;

            if (clothingObject != null)
            {
                SkinnedMeshRenderer clothingObjectSMR = clothingObject.GetComponent<SkinnedMeshRenderer>();
                Renderer clothingObjectRenderer = clothingObject.GetComponent<Renderer>();

                //Set Mesh:
                clothingObjectSMR.sharedMesh = charClothingSO.Meshes[0];

                //Set Material (if needed):
                if (clothingObjectRenderer.sharedMaterial == MaterialBlank)
                {
                    Material newMaterial;

                    if (!charClothing.HasCustomMaterial()) //if no custom material
                    {
                        newMaterial = new Material(charClothingSO.ClothingSlot.MaterialDefault) { color = charClothing.GetColor() };
                    }
                    else
                    {
                        newMaterial = new Material(GameMaster.Instance.CustomizationManager.Character.CustomMaterials[charClothing.CustomMaterialID]);
                    }

                    clothingObject.GetComponent<Renderer>().sharedMaterial = newMaterial;
                }

                //Enable SkinnedMeshRenderer component (if needed)
                if (!clothingObjectSMR.enabled)
                    clothingObjectSMR.enabled = true;
            }
            else
            { Debug.Log(string.Format("*Failed to set Clothing object for '{0}': No placeholder object found.", charClothingSO.Name)); }
        }

        if (!charClothingSO.ClothingSlot.IsAccessory)
        {
            GameObject bodyObject = null;

            if (charClothingSO.ClothingSlot.PlaceholderNames[1] != "")
            {
                bodyObject = transform.Find(charClothingSO.ClothingSlot.PlaceholderNames[1]).gameObject;

                if (bodyObject != null)
                {
                    SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
                    Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();

                    //Set Mesh:
                    bodyObjectSMR.sharedMesh = charClothingSO.Meshes[1];

                    //Set Material (if needed):
                    if (bodyObjectRenderer.sharedMaterial == MaterialBlank)
                    {
                        bodyObject.GetComponent<Renderer>().sharedMaterial = MaterialBody;
                    }

                    //Enable SkinnedMeshRenderer component (if needed)
                    if (!bodyObjectSMR.enabled)
                        bodyObjectSMR.enabled = true;
                }
                else
                { Debug.Log(string.Format("*Failed to set Body object for '{0}': No placeholder object found.", charClothingSO.Name)); }
            }
        }
    }

    /// <summary>
    /// Sets/Enables the Mesh, Material and SkinnedMeshRenderer component of the GameObject found using the specified body item.
    /// </summary>
    /// <param name="charClothing"></param>
    private void SetObjectBody(CharacterBodySO charBody)
    {
        GameObject bodyObject = null;

        if (charBody.ClothingSlot.PlaceholderNames[1] != "")
        {
            bodyObject = transform.Find(charBody.ClothingSlot.PlaceholderNames[1]).gameObject;

            if (bodyObject != null)
            {
                SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
                Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();

                //Set Mesh:
                bodyObjectSMR.sharedMesh = charBody.Mesh;

                //Set Material (if needed):
                if (bodyObjectRenderer.sharedMaterial == MaterialBlank)
                {
                    bodyObject.GetComponent<Renderer>().sharedMaterial = MaterialBody;
                }

                //Enable SkinnedMeshRenderer component (if needed)
                if (!bodyObjectSMR.enabled)
                    bodyObjectSMR.enabled = true;
            }
            else
            { Debug.Log(string.Format("*Failed to set Body '{0}': No placeholder object found.", charBody.Name)); }
        }
    }

    /// <summary>
    /// Unsets the clothing placeholder object and, if applicable, the body placeholder object of the specified clothing slot.
    /// </summary>
    /// <param name="clothing"></param>
    private void UnsetClothing(CharacterClothingSlotSO clothingSlot)
    {
        UnsetObject(transform.Find(clothingSlot.PlaceholderNames[0]).gameObject);

        if (!clothingSlot.IsAccessory)
        {
            UnsetObject(transform.Find(clothingSlot.PlaceholderNames[1]).gameObject);
        }
    }

    /// <summary>
    /// Removes/disables the Mesh, Material and SkinnedMeshRenderer component of the specified GameObject.
    /// </summary>
    /// <param name="obj"></param>
    private void UnsetObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            obj.GetComponent<SkinnedMeshRenderer>().enabled = false;
            obj.GetComponent<Renderer>().sharedMaterial = MaterialBlank;
        }
        else
            Debug.Log(string.Format("*Failed to unset object: '{0}' not found.", obj.name));
    }

    /// <summary>
    /// Checks if the specified clothing slot is currently filled/in use.
    /// </summary>
    /// <param name="clothingSlot">The clothing slot to use in the search.</param>
    /// <param name="iClothing">The index of the found clothing item in the CurrentClothing list.</param>
    /// <returns></returns>
    private bool IsClothingSlotUsed(ClothingSlot clothingSlot, out int iClothing)
    {
        bool clothingFound = false;
        iClothing = -1;

        if (CustomizationData.CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CustomizationData.CurrentClothing.Count; i++)
            {
                if (CustomizationData.CurrentClothing[i].GetClothingSO().ClothingSlot.Slot == clothingSlot)
                {
                    clothingFound = true;
                    iClothing = i;

                    i = CustomizationData.CurrentClothing.Count; //end
                }
            }
        }

        return clothingFound;
    }

    /// <summary>
    /// Checks if the specified clothing slot is currently filled/in use.
    /// </summary>
    /// <param name="clothingSlot">The clothing slot to use in the search.</param>
    /// <returns></returns>
    private bool IsClothingSlotUsed(ClothingSlot clothingSlot)
    {
        bool clothingFound = false;

        if (CustomizationData.CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CustomizationData.CurrentClothing.Count; i++)
            {
                if (CustomizationData.CurrentClothing[i].GetClothingSO().ClothingSlot.Slot == clothingSlot)
                {
                    clothingFound = true;

                    i = CustomizationData.CurrentClothing.Count; //end
                }
            }
        }

        return clothingFound;
    }
}
