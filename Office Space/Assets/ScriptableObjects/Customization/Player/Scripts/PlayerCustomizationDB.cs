using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Customization DB", menuName = "Player Customization DB")]
public class PlayerCustomizationDB : ScriptableObject
{
    [HideInInspector]
    public GameObject ClothingPlaceholderUpper;
    [HideInInspector]
    public GameObject ClothingPlaceholderLower;
    [HideInInspector]
    public GameObject ClothingPlaceholderCostume;
    [HideInInspector]
    public GameObject ClothingPlaceholderHead;
    [HideInInspector]
    public GameObject ClothingPlaceholderArmL;
    [HideInInspector]
    public GameObject ClothingPlaceholderArmR;

    [HideInInspector]
    public GameObject Body;
    [HideInInspector]
    public GameObject BodyPlaceholderUpper;
    [HideInInspector]
    public GameObject BodyPlaceholderLower;

    public Material DefaultUpperClothingMaterial;
    public Material DefaultLowerClothingMaterial;
    public Material DefaultCostumeClothingMaterial;
    public Material DefaultHeadClothingMaterial;
    public Material DefaultArmClothingMaterial;

    public List<PlayerMeshGroup> UpperMeshGroups;
    public List<PlayerMeshGroup> LowerMeshGroups;
    public List<PlayerMeshGroup> CostumeMeshGroups;
    public List<Mesh> HeadMeshes;
    public List<Mesh> ArmLMeshes;
    public List<Mesh> ArmRMeshes;

    private GameObject playerObject;

    public void SetPlayerObject(GameObject playerObject)
    {
        this.playerObject = playerObject;

        bool success = GetPlaceholders();

        Debug.Log("PLAYER INSTANTIATION SUCCESSFUL: " + success.ToString());

        ResetClothing();
    }

    public void ResetClothing()
    {
        DisablePlaceholder(Body);

        DisablePlaceholder(ClothingPlaceholderCostume);
        DisablePlaceholder(ClothingPlaceholderArmL);
        DisablePlaceholder(ClothingPlaceholderArmR);
        DisablePlaceholder(ClothingPlaceholderHead);

        BodyPlaceholderUpper.GetComponent<SkinnedMeshRenderer>().sharedMesh = UpperMeshGroups[0].Body;
        ClothingPlaceholderUpper.GetComponent<SkinnedMeshRenderer>().sharedMesh = UpperMeshGroups[0].Clothing;
        ClothingPlaceholderUpper.GetComponent<Renderer>().material = DefaultUpperClothingMaterial;

        BodyPlaceholderLower.GetComponent<SkinnedMeshRenderer>().sharedMesh = LowerMeshGroups[0].Body;
        ClothingPlaceholderLower.GetComponent<SkinnedMeshRenderer>().sharedMesh = LowerMeshGroups[0].Clothing;
        ClothingPlaceholderLower.GetComponent<Renderer>().material = DefaultLowerClothingMaterial;
    }

    private void DisablePlaceholder(GameObject placeholder)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = placeholder.GetComponent<SkinnedMeshRenderer>();

        skinnedMeshRenderer.sharedMesh = null;
        skinnedMeshRenderer.enabled = false;
    }

    private bool GetPlaceholders()
    {
        ClothingPlaceholderUpper = playerObject.transform.Find("C_Upper").gameObject;
        ClothingPlaceholderLower = playerObject.transform.Find("C_Lower").gameObject;
        ClothingPlaceholderCostume = playerObject.transform.Find("C_Costume").gameObject;
        ClothingPlaceholderHead = playerObject.transform.Find("C_Head").gameObject;
        ClothingPlaceholderArmL = playerObject.transform.Find("C_ArmL").gameObject;
        ClothingPlaceholderArmR = playerObject.transform.Find("C_ArmR").gameObject;

        Body = playerObject.transform.Find("B_FULL").gameObject;
        BodyPlaceholderUpper = playerObject.transform.Find("B_Upper").gameObject;
        BodyPlaceholderLower = playerObject.transform.Find("B_Lower").gameObject;

        return ValidatePlaceholders();
    }

    private bool ValidatePlaceholders()
    {
        if (ClothingPlaceholderUpper != null && ClothingPlaceholderLower != null && ClothingPlaceholderHead != null & ClothingPlaceholderCostume != null && ClothingPlaceholderArmR != null && ClothingPlaceholderArmL != null && Body != null && BodyPlaceholderUpper != null & BodyPlaceholderLower != null)
            return true;
        else
            return false;
    }
}
