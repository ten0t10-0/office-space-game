using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationScript : MonoBehaviour
{
    public Material MaterialBlank;
    [HideInInspector]
    public Material MaterialBody;

    [HideInInspector]
    public List<PlayerClothing> CurrentClothing;

    private void Awake()
    {
        MaterialBody = new Material(GameMaster.Instance.CustomizationManager.Player.MaterialBodyDefault);

        //Remove placeholder objects:
        for (int i = 0; i < GameMaster.Instance.CustomizationManager.Player.ClothingSlots.Count; i++)
        {
            UnsetObject(transform.Find(GameMaster.Instance.CustomizationManager.Player.ClothingSlots[i].PlaceholderNames[0]).gameObject);

            if (!GameMaster.Instance.CustomizationManager.Player.ClothingSlots[i].IsAccessory)
            {
                UnsetObject(transform.Find(GameMaster.Instance.CustomizationManager.Player.ClothingSlots[i].PlaceholderNames[1]).gameObject);
            }
        }
    }

    /// <summary>
    /// Removes all clothing from the character, then binds the specified list of clothing to the character.
    /// </summary>
    /// <param name="clothingList"></param>
    public void SetClothingByList(List<PlayerClothing> clothingList)
    {
        UnsetAllClothing();

        for (int i = 0; i < clothingList.Count; i++)
        {
            CurrentClothing.Add(clothingList[i]);
        }

        if (CurrentClothing.Count > 0)
        {
            List<PlayerClothing> tempClothing = new List<PlayerClothing>();
            foreach (PlayerClothing clothing in CurrentClothing)
            {
                tempClothing.Add(clothing);
            }

            CurrentClothing.Clear();

            for (int i = 0; i < tempClothing.Count; i++)
            {
                SetClothing(tempClothing[i]);
            }
        }
        else
        {
            SetObjectBody(GameMaster.Instance.CustomizationManager.Player.GetClothingSlotSO(ClothingSlot.Costume).Body);
        }
    }

    /// <summary>
    /// Sets the specified clothing item onto the character object and updates the CurrentClothing list.
    /// </summary>
    /// <param name="charClothing"></param>
    public void SetClothing(PlayerClothing charClothing)
    {
        if (charClothing.GetPlayerClothingSO().ClothingSlot.Slot == ClothingSlot.Costume)
            UnsetAllClothing(ClothingSlot.Head); //Remove all clothing EXCEPT for head accessory.

        SetObject(charClothing);

        int iClothing;

        if (IsClothingSlotUsed(charClothing.GetPlayerClothingSO().ClothingSlot.Slot, out iClothing))
            CurrentClothing.RemoveAt(iClothing);

        CurrentClothing.Add(charClothing);

        if (charClothing.GetPlayerClothingSO().ClothingSlot.Slot == ClothingSlot.Upper && !IsClothingSlotUsed(ClothingSlot.Lower))
        {
            SetObjectBody(GameMaster.Instance.CustomizationManager.Player.GetClothingSlotSO(ClothingSlot.Lower).Body);
        }
        else if (charClothing.GetPlayerClothingSO().ClothingSlot.Slot == ClothingSlot.Lower && !IsClothingSlotUsed(ClothingSlot.Upper))
        {
            SetObjectBody(GameMaster.Instance.CustomizationManager.Player.GetClothingSlotSO(ClothingSlot.Upper).Body);
        }
    }

    /// <summary>
    /// Removes the clothing item from the specified slot and updates the CurrentClothing list.
    /// </summary>
    /// <param name="clothingSlot"></param>
    public void UnsetClothing(PlayerClothingSlotSO clothingSlot)
    {
        int iClothing;

        if (IsClothingSlotUsed(clothingSlot.Slot, out iClothing))
        {
            UnsetObject(transform.Find(clothingSlot.PlaceholderNames[0]).gameObject);

            if (!clothingSlot.IsAccessory)
            {
                UnsetObject(transform.Find(clothingSlot.PlaceholderNames[1]).gameObject);
            }

            CurrentClothing.RemoveAt(iClothing);

            if (clothingSlot.Slot == ClothingSlot.Upper || clothingSlot.Slot == ClothingSlot.Lower || clothingSlot.Slot == ClothingSlot.Costume)
            {
                if (clothingSlot.Slot == ClothingSlot.Costume)
                {
                    SetObjectBody(clothingSlot.Body);
                }
                else if (!IsClothingSlotUsed(ClothingSlot.Lower) && !IsClothingSlotUsed(ClothingSlot.Upper))
                {
                    SetObjectBody(GameMaster.Instance.CustomizationManager.Player.GetClothingSlotSO(ClothingSlot.Costume).Body);
                }
                else
                {
                    SetObjectBody(clothingSlot.Body);
                }
            }
        }
        else
        { Debug.Log(string.Format("*Clothing slot '{0}' is not being used.", clothingSlot.Name)); }
    }

    /// <summary>
    /// Removes all clothing from the character object and updates the CurrentClothing list.
    /// </summary>
    private void UnsetAllClothing()
    {
        if (CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CurrentClothing.Count; i++)
            {
                UnsetClothing(CurrentClothing[i].GetPlayerClothingSO().ClothingSlot);
            }
        }
    }

    /// <summary>
    /// Removes all clothing from the character object, except for clothing currently occupying the specified slots, and updates the CurrentClothing list.
    /// </summary>
    private void UnsetAllClothing(params ClothingSlot[] slotsToExclude)
    {
        bool exclude = false;

        if (CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CurrentClothing.Count; i++)
            {
                foreach (ClothingSlot slot in slotsToExclude)
                {
                    if (CurrentClothing[i].GetPlayerClothingSO().ClothingSlot.Slot == slot)
                    {
                        exclude = true;
                        break;
                    }
                }

                if (!exclude)
                    UnsetClothing(CurrentClothing[i].GetPlayerClothingSO().ClothingSlot);
            }
        }
    }

    /// <summary>
    /// Sets/Enables the Mesh, Material and SkinnedMeshRenderer component of the GameObject found using the specified clothing item.
    /// </summary>
    /// <param name="charClothing"></param>
    private void SetObject(PlayerClothing charClothing)
    {
        PlayerClothingSO charClothingSO = charClothing.GetPlayerClothingSO();

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
                        newMaterial = new Material(charClothingSO.ClothingSlot.MaterialDefault)
                        { color = charClothing.GetColor() };
                    }
                    else
                    {
                        newMaterial = new Material(GameMaster.Instance.CustomizationManager.Player.CustomMaterials[charClothing.CustomMaterialID]);
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
                    if (bodyObject.GetComponent<Renderer>().sharedMaterial == MaterialBlank)
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
    private void SetObjectBody(PlayerBodySO charBody)
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
                if (bodyObject.GetComponent<Renderer>().sharedMaterial == MaterialBlank)
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
    /// Removes/disables all Mesh, Material and SkinnedMeshRenderer components of all GameObjects affected by clothes in the CurrentClothing list. (List is unchanged!)
    /// </summary>
    private void UnsetAllObjects()
    {
        if (CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CurrentClothing.Count; i++)
            {
                GameObject obj = transform.Find(CurrentClothing[i].GetPlayerClothingSO().ClothingSlot.PlaceholderNames[0]).gameObject;

                UnsetObject(obj);

                if (!CurrentClothing[i].GetPlayerClothingSO().ClothingSlot.IsAccessory)
                {
                    obj = transform.Find(CurrentClothing[i].GetPlayerClothingSO().ClothingSlot.PlaceholderNames[1]).gameObject;

                    UnsetObject(obj);
                }
            }
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

        if (CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CurrentClothing.Count; i++)
            {
                if (CurrentClothing[i].GetPlayerClothingSO().ClothingSlot.Slot == clothingSlot)
                {
                    clothingFound = true;
                    iClothing = i;

                    i = CurrentClothing.Count; //end
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

        if (CurrentClothing.Count > 0)
        {
            for (int i = 0; i < CurrentClothing.Count; i++)
            {
                if (CurrentClothing[i].GetPlayerClothingSO().ClothingSlot.Slot == clothingSlot)
                {
                    clothingFound = true;

                    i = CurrentClothing.Count; //end
                }
            }
        }

        return clothingFound;
    }
}
