using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryScript : MonoBehaviour
{
    [HideInInspector]
    public int AccessoryIndex = -1;

    private Material currentMaterial = null;

    public void Initialize(int accessoryIndex)
    {
        AccessoryIndex = accessoryIndex;

        currentMaterial = new Material(GameMaster.Instance.CustomizationManager.Character.Accessories[AccessoryIndex].GameObject.GetComponent<Renderer>().sharedMaterial);
        currentMaterial.name += " (copy)";

        GetComponent<Renderer>().sharedMaterial = currentMaterial;
    }

    private void OnDestroy()
    {
        if (currentMaterial != null)
        {
            Destroy(currentMaterial);
            currentMaterial = null;
        }
    }
}
