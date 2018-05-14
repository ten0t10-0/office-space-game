using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingSlot { Costume, Upper, Lower, Head, LeftArm, RightArm }

[CreateAssetMenu(fileName = "New Player Customization DB", menuName = "Player Customization/Player Customization DB")]
public class PlayerClothingDatabaseSO : ScriptableObject
{
    public Material BodyMaterialDefault;
    [HideInInspector]
    public Material BodyMaterialCurrent;

    public List<PlayerClothingSO> Clothing;
    public List<PlayerClothingSlotSO> ClothingSlots;

    [HideInInspector]
    public GameObject PlayerObject;
    [HideInInspector]
    public Player Player;

    private bool isPlayerUnset;

    public List<int> DefaultClothingIndexes;

    public void SetPlayer(GameObject playerObject, ref Player player)
    {
        PlayerObject = playerObject;
        Player = player;

        if (player.CustomizationData.CurrentClothing.Count > 0)
        {
            SetClothingByList(player.CustomizationData.CurrentClothing);
        }
        else
            UnsetAllClothing();
    }

    public void ResetAppearance()
    {
        Player.CustomizationData.CurrentClothing = new List<PlayerClothing>();

        foreach (int i in DefaultClothingIndexes)
            Player.CustomizationData.CurrentClothing.Add(new PlayerClothing(i));

        //Default clothing
        SetClothingByList(Player.CustomizationData.CurrentClothing);
    }

    /// <summary>
    /// Sets the specified clothing object on the player.
    /// </summary>
    /// <param name="playerClothing"></param>
    public void SetClothing(PlayerClothing playerClothing)
    {
        PlayerClothingSO playerClothingSO = playerClothing.GetPlayerClothingSO();

        if (!isPlayerUnset && playerClothingSO.ClothingSlot.Slot == ClothingSlot.Costume)
            UnsetAllClothing();

        GameObject clothingObject = PlayerObject.transform.Find(playerClothingSO.ClothingSlot.PlaceholderNames[0]).gameObject;

        if (clothingObject != null)
        {
            SkinnedMeshRenderer clothingObjectSMR = clothingObject.GetComponent<SkinnedMeshRenderer>();
            Renderer clothingObjectRenderer = clothingObject.GetComponent<Renderer>();

            //Set Meshes:
            if (playerClothingSO.Meshes[0] != null)
                clothingObjectSMR.sharedMesh = playerClothingSO.Meshes[0];

            //Set Materials (if needed):
            if (clothingObjectRenderer.sharedMaterial == BodyMaterialDefault)
                clothingObject.GetComponent<Renderer>().sharedMaterial = playerClothingSO.ClothingSlot.MaterialDefault;

            //Enable SkinnedMeshRenderer components (if needed)
            if (!clothingObjectSMR.enabled)
                clothingObjectSMR.enabled = true;
        }
        else
        { Debug.Log(string.Format("*Failed to set Clothing object for '{0}': No object found.", playerClothingSO.Name)); }

        if (!playerClothingSO.ClothingSlot.IsAccessory)
        {
            GameObject bodyObject = PlayerObject.transform.Find(playerClothingSO.ClothingSlot.PlaceholderNames[1]).gameObject;

            if (bodyObject != null)
            {
                SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
                Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();

                //Set Meshes:
                if (playerClothingSO.Meshes[1] != null)
                    bodyObjectSMR.sharedMesh = playerClothingSO.Meshes[1];

                //Set Materials (if needed):
                //if (bodyObject.GetComponent<Renderer>().material == BodyMaterialDefault)
                //    bodyObject.GetComponent<Renderer>().material = BodyMaterialDefault;

                //Enable SkinnedMeshRenderer components (if needed)
                if (!bodyObjectSMR.enabled)
                    bodyObjectSMR.enabled = true;
            }
            else
            { Debug.Log(string.Format("Failed to set Body object for '{0}': No object found.", playerClothingSO.Name)); }
        }

        AddPlayerClothingData(playerClothing);

        isPlayerUnset = false;
    }


    /// <summary>
    /// Sets the specified clothing object on the specified npc object.
    /// </summary>
    /// <param name="customizationData"></param>
    /// <param name="npcObject"></param>
    public void SetClothingNPC(PlayerClothing customizationData, GameObject npcObject)
    {
        PlayerClothingSO npcClothing = customizationData.GetPlayerClothingSO();

        if (npcClothing.ClothingSlot.Slot == ClothingSlot.Costume)
            UnsetAllClothingNPC(npcObject);

        GameObject clothingObject = npcObject.transform.Find(npcClothing.ClothingSlot.PlaceholderNames[0]).gameObject;

        if (clothingObject != null)
        {
            SkinnedMeshRenderer clothingObjectSMR = clothingObject.GetComponent<SkinnedMeshRenderer>();
            Renderer clothingObjectRenderer = clothingObject.GetComponent<Renderer>();

            //Set Meshes:
            if (npcClothing.Meshes[0] != null)
                clothingObjectSMR.sharedMesh = npcClothing.Meshes[0];

            //Set Materials (if needed):
            if (clothingObjectRenderer.sharedMaterial == BodyMaterialDefault)
                clothingObject.GetComponent<Renderer>().sharedMaterial = npcClothing.ClothingSlot.MaterialDefault;

            //Enable SkinnedMeshRenderer components (if needed)
            if (!clothingObjectSMR.enabled)
                clothingObjectSMR.enabled = true;
        }
        else
        { Debug.Log(string.Format("*Failed to set Clothing object for '{0}': No object found.", npcClothing.Name)); }

        if (!npcClothing.ClothingSlot.IsAccessory)
        {
            GameObject bodyObject = npcObject.transform.Find(npcClothing.ClothingSlot.PlaceholderNames[1]).gameObject;

            if (bodyObject != null)
            {
                SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
                Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();

                //Set Meshes:
                if (npcClothing.Meshes[1] != null)
                    bodyObjectSMR.sharedMesh = npcClothing.Meshes[1];

                //Set Materials (if needed):
                //if (bodyObject.GetComponent<Renderer>().material == BodyMaterialDefault)
                //    bodyObject.GetComponent<Renderer>().material = BodyMaterialDefault;

                //Enable SkinnedMeshRenderer components (if needed)
                if (!bodyObjectSMR.enabled)
                    bodyObjectSMR.enabled = true;
            }
            else
            { Debug.Log(string.Format("Failed to set Body object for '{0}': No object found.", npcClothing.Name)); }
        }
    }

    public void SetClothingByList(List<PlayerClothing> data)
    {
        UnsetAllClothing();

        for (int i = 0; i < data.Count; i++)
        {
            SetClothing(data[i]);
        }
    }

    public void SetClothingByListNPC(List<PlayerClothing> data, GameObject npcObject)
    {
        UnsetAllClothingNPC(npcObject);

        for (int i = 0; i < data.Count; i++)
        {
            SetClothingNPC(data[i], npcObject);
        }
    }

    /// <summary>
    /// Removes/disables all Clothing Meshes, Materials and SkinnedMeshRenderer components.
    /// </summary>
    public void UnsetAllClothing()
    {
        foreach (PlayerClothingSlotSO slot in ClothingSlots)
        {
            UnsetClothing(slot);
        }

        isPlayerUnset = true;
    }

    public void UnsetAllClothingNPC(GameObject npcObject)
    {
        foreach (PlayerClothingSlotSO slot in ClothingSlots)
        {
            UnsetClothingNPC(slot, npcObject);
        }
    }

    /// <summary>
    /// Removes/disables the Mesh, Material and SkinnedMeshRenderer component of the current clothing item in the specified slot.
    /// </summary>
    /// <param name="objectName">The name of the object.</param>
    public void UnsetClothing(PlayerClothingSlotSO clothingSlot)
    {
        UnsetObject(PlayerObject.transform.Find(clothingSlot.PlaceholderNames[0]).gameObject);

        if (!clothingSlot.IsAccessory)
        {
            UnsetObject(PlayerObject.transform.Find(clothingSlot.PlaceholderNames[1]).gameObject);
        }

        RemovePlayerClothingData(clothingSlot);

        if (clothingSlot.Slot == ClothingSlot.Upper || clothingSlot.Slot == ClothingSlot.Lower)
        {
            GameObject bodyObject = PlayerObject.transform.Find(clothingSlot.PlaceholderNames[1]).gameObject;

            if (bodyObject != null)
            {
                SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
                Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();

                //Set Meshes:
                if (clothingSlot.NoClothingMesh != null)
                    bodyObjectSMR.sharedMesh = clothingSlot.NoClothingMesh;

                //Set Materials (if needed):
                //if (bodyObject.GetComponent<Renderer>().material == BodyMaterialDefault)
                //    bodyObject.GetComponent<Renderer>().material = BodyMaterialDefault;

                //Enable SkinnedMeshRenderer components (if needed)
                if (!bodyObjectSMR.enabled)
                    bodyObjectSMR.enabled = true;
            }
            else
            { Debug.Log(string.Format("Failed to set Body object for '{0}': No object found.", bodyObject.name)); }
        }

        if (clothingSlot.Slot == ClothingSlot.Costume && Player.CustomizationData.CurrentClothing.Count == 0)
        {
            GameObject bodyObject = PlayerObject.transform.Find(clothingSlot.PlaceholderNames[1]).gameObject;

            if (bodyObject != null)
            {
                SkinnedMeshRenderer bodyObjectSMR = bodyObject.GetComponent<SkinnedMeshRenderer>();
                Renderer bodyObjectRenderer = bodyObject.GetComponent<Renderer>();

                //Set Meshes:
                if (clothingSlot.NoClothingMesh != null)
                    bodyObjectSMR.sharedMesh = clothingSlot.NoClothingMesh;

                //Set Materials (if needed):
                //if (bodyObject.GetComponent<Renderer>().material == BodyMaterialDefault)
                //    bodyObject.GetComponent<Renderer>().material = BodyMaterialDefault;

                //Enable SkinnedMeshRenderer components (if needed)
                if (!bodyObjectSMR.enabled)
                    bodyObjectSMR.enabled = true;
            }
            else
            { Debug.Log(string.Format("Failed to set Body object for '{0}': No object found.", bodyObject.name)); }
        }
    }

    public void UnsetClothingNPC(PlayerClothingSlotSO clothingSlot, GameObject npcObject)
    {
        UnsetObject(npcObject.transform.Find(clothingSlot.PlaceholderNames[0]).gameObject);

        if (!clothingSlot.IsAccessory)
        {
            UnsetObject(npcObject.transform.Find(clothingSlot.PlaceholderNames[1]).gameObject);
        }
    }

    /// <summary>
    /// Checks if the specified clothing slot is already used by the player.
    /// </summary>
    /// <param name="clothingSlot"></param>
    /// <returns></returns>
    private bool IsClothingSlotUsed(PlayerClothingSlotSO clothingSlot, out int iCurrentClothing)
    {
        bool slotUsed = false;
        iCurrentClothing = -1;

        if (Player.CustomizationData.CurrentClothing.Count > 0)
        {
            for (int i = 0; i < Player.CustomizationData.CurrentClothing.Count; i++)
            {
                if (Player.CustomizationData.CurrentClothing[i].GetPlayerClothingSO() == clothingSlot)
                {
                    slotUsed = true;

                    iCurrentClothing = i;

                    i = Player.CustomizationData.CurrentClothing.Count; //end
                }
            }
        }

        return slotUsed;
    }

    private void AddPlayerClothingData(PlayerClothing playerClothing)
    {
        RemovePlayerClothingData(playerClothing.GetPlayerClothingSO().ClothingSlot);

        Player.CustomizationData.CurrentClothing.Add(playerClothing);
    }

    private void RemovePlayerClothingData(PlayerClothingSlotSO clothingSlotSO)
    {
        int iClothing;

        if (IsClothingSlotUsed(clothingSlotSO, out iClothing))
        {
            Player.CustomizationData.CurrentClothing.RemoveAt(iClothing);
        }
    }

    /// <summary>
    /// Removes/disables the Mesh, Material and SkinnedMeshRenderer componenet of the specified GameObject.
    /// </summary>
    /// <param name="obj"></param>
    private void UnsetObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            obj.GetComponent<SkinnedMeshRenderer>().enabled = false;
            obj.GetComponent<Renderer>().sharedMaterial = BodyMaterialDefault;
        }
        else
            Debug.Log(string.Format("*Failed to unset object: '{0}' not found.", obj.name));
    }
}
