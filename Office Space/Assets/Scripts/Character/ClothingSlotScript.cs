using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingSlotScript : MonoBehaviour
{
    public ClothingSlot ClothingSlot;
    public int ClothingIndex;
    public bool IsBodyObject;

    private CharacterCustomizationScript parentScript;

    private Material currentClothingMaterial = null;

    /// <summary>
    /// Sets a reference to the parent script and creates a copy of the clothing slot material to be used, if needed.
    /// </summary>
    /// <param name="parentScript"></param>
    public void Initialize(CharacterCustomizationScript parentScript)
    {
        this.parentScript = parentScript;

        if (!IsBodyObject)
        {
            currentClothingMaterial = new Material(GameMaster.Instance.CustomizationManager.Character.GetClothingSlotSO(ClothingSlot).MaterialDefault);
            currentClothingMaterial.name += " (copy)";

            GetComponent<Renderer>().sharedMaterial = currentClothingMaterial;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = parentScript.MaterialBody;
        }
    }

    public void Set(int clothingIndex)
    {
        CharacterClothingSO clothingSO = GameMaster.Instance.CustomizationManager.Character.Clothing[clothingIndex];

        if (!IsBodyObject)
        {
            GetComponent<SkinnedMeshRenderer>().sharedMesh = clothingSO.Meshes[0];
            GetComponent<SkinnedMeshRenderer>().enabled = true;

            ClothingIndex = clothingIndex;
        }
        else if (clothingSO.HasBodyMesh)
        {
            GetComponent<SkinnedMeshRenderer>().sharedMesh = clothingSO.Meshes[1];
            GetComponent<SkinnedMeshRenderer>().enabled = true;

            ClothingIndex = clothingIndex;
        }
    }

    /// <summary>
    /// Sets the ClothingIndex to -1 and removes/disables the Mesh, Material and SkinnedMeshRenderer component.
    /// </summary>
    public void Unset()
    {
        GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
        GetComponent<SkinnedMeshRenderer>().enabled = false;

        ClothingIndex = -1;
    }

    public bool IsSet
    {
        get { return (ClothingIndex > -1); }
    }

    private void OnDestroy()
    {
        if (currentClothingMaterial != null)
            Destroy(currentClothingMaterial);
    }
}
