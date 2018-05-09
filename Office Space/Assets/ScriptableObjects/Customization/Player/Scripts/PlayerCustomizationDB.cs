using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Customization DB", menuName = "Player Customization/Player Customization DB")]
public class PlayerCustomizationDB : ScriptableObject
{
    public Material BodyMaterialDefault;
    [HideInInspector]
    public Material BodyMaterialCurrent;

    public List<PlayerClothing> Clothing;
    public List<PlayerAccessory> Accessories;

    public List<PlayerClothingCategory> ClothingCategories;
    public List<PlayerAccessoryCategory> AccessoryCategories;

    [HideInInspector]
    public GameObject PlayerObject;

    //TEMP?:
    private Mesh bodyMesh;
    private string bodyPlaceholderName = "B_FULL";

    //TEMP ENUMS?:
    private enum ClothingCategory { Costume, Upper, Lower }
    private enum AccessoryCategory { Head, ArmLeft, ArmRight }

    private bool isPlayerUnset;

    public void SetPlayerObject(GameObject playerObject)
    {
        PlayerObject = playerObject;

        bodyMesh = PlayerObject.transform.Find(bodyPlaceholderName).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        ResetAppearance();
    }

    public void ResetAppearance()
    {
        UnsetAppearance();

        //TEST: Default Upper & Lower clothing
        SetClothing(Clothing[(int)ClothingCategory.Upper]);
        SetClothing(Clothing[(int)ClothingCategory.Lower]);

        //TEST: Default costume/Onez
        //SetClothing(Clothing[(int)ClothingCategory.Costume]);

        //TEST: No clothing
        //Nudify();
    }

    public void SetAccessory(PlayerAccessory playerAccessory)
    {
        //*

        //isPlayerUnset = false;
    }

    /// <summary>
    /// Sets the specified clothing object on the player.
    /// </summary>
    /// <param name="playerClothing"></param>
    public void SetClothing(PlayerClothing playerClothing)
    {
        if (!isPlayerUnset && playerClothing.Category == ClothingCategories[(int)ClothingCategory.Costume])
            UnsetAppearance();

        GameObject bodyObject = PlayerObject.transform.Find(playerClothing.Category.BodyPlaceholderName).gameObject;
        GameObject clothingObject = PlayerObject.transform.Find(playerClothing.Category.ClothingPlaceholderName).gameObject;

        if (bodyObject != null && clothingObject != null)
        {
            SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer clothingObjectSMR = clothingObject.GetComponent<SkinnedMeshRenderer>();

            //Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();
            Renderer clothingObjectRenderer = clothingObject.GetComponent<Renderer>();

            //Set Meshes:
            bodyObjectSMR.sharedMesh = playerClothing.MeshGroup.BodyMesh;
            if (playerClothing.MeshGroup.ClothingMesh != null)
                clothingObjectSMR.sharedMesh = playerClothing.MeshGroup.ClothingMesh;

            //Set Materials (if needed):
            //if (bodyObject.GetComponent<Renderer>().material == BodyMaterialDefault)
            //    bodyObject.GetComponent<Renderer>().material = BodyMaterialDefault;
            if (clothingObjectRenderer.sharedMaterial == BodyMaterialDefault)
                clothingObject.GetComponent<Renderer>().sharedMaterial = playerClothing.Category.ClothingMaterialDefault;

            //Enable SkinnedMeshRenderer components (if needed)
            if (!bodyObjectSMR.enabled)
                bodyObjectSMR.enabled = true;
            if (!clothingObjectSMR.enabled)
                clothingObjectSMR.enabled = true;

            isPlayerUnset = false;
        }
        else
        {
            Debug.Log(string.Format("Failed to set clothing for '{0}': Object(s) not found.", playerClothing.name));
        }
    }

    /// <summary>
    /// Removes/disables all Clothing and Accessory Meshes, Materials and SkinnedMeshRenderer components.
    /// </summary>
    public void UnsetAppearance()
    {
        UnsetAccessories();
        UnsetClothing();

        isPlayerUnset = true;
    }

    /// <summary>
    /// Removes/disables all Clothing Meshes, Materials and SkinnedMeshRenderer components.
    /// </summary>
    public void UnsetClothing()
    {
        foreach (PlayerClothingCategory pcCategory in ClothingCategories)
        {
            DisableObject(pcCategory.ClothingPlaceholderName);
            DisableObject(pcCategory.BodyPlaceholderName);
        }
    }
    /// <summary>
    /// Removes/disables all Accessory Meshes, Materials and SkinnedMeshRenderer components.
    /// </summary>
    public void UnsetAccessories()
    {
        foreach (PlayerAccessoryCategory paCategory in AccessoryCategories)
        {
            DisableObject(paCategory.ClothingPlaceholderName);
        }
    }

    /// <summary>
    /// Removes/disables the object's Mesh, Material and SkinnedMeshRenderer component.
    /// </summary>
    /// <param name="objectName">The name of the object.</param>
    private void DisableObject(string objectName)
    {
        GameObject obj = PlayerObject.transform.Find(objectName).gameObject;

        if (obj != null)
        {
            obj.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            obj.GetComponent<SkinnedMeshRenderer>().enabled = false;
            obj.GetComponent<Renderer>().sharedMaterial = BodyMaterialDefault;
        }
        else
            Debug.Log(string.Format("*Failed to disable object: '{0}' not found.", objectName));
    }

    private void Nudify()
    {
        UnsetAppearance();

        PlayerObject.transform.Find(bodyPlaceholderName).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = bodyMesh;
        PlayerObject.transform.Find(bodyPlaceholderName).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }
}
