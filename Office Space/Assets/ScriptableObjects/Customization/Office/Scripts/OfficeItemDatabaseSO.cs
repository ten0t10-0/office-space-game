﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Office Item Database", menuName = "Office Customization/Database")]
public class OfficeItemDatabaseSO : ScriptableObject
{
    public Material MaterialWallsDefault;
    public Material MaterialFloorDefault;
    public Material MaterialCeilingDefault;
    public Material MaterialInvalidPlacement;
    public Material MaterialHighlighted;

    public GameObject LightLampDefault;

    public int OfficeItemLayer = 8;

    public float ObjectPlacementDistance = 7f;

    [HideInInspector]
    public Material MaterialWallsCurrent;
    [HideInInspector]
    public Material MaterialFloorCurrent;
    [HideInInspector]
    public Material MaterialCeilingCurrent;
    [HideInInspector]
    public Material MaterialWallsShopCurrent;

    [HideInInspector]
    public int SelectedObjectIndex = -1;

    public string OfficeParentName;
    public string RoomParentName;
    public string ObjectsParentName;
    public string ShopParentName;
    public string ShopRoomParentName;
    //public string ShopObjectsParentName;

    public int MaxNumberOfObjects = 15;

    public List<OfficeItemSO> Items;
    public List<OfficeItemTypeSO> Types;

    private Transform officeRoomTransform;
    private Transform officeObjectTransform;
    private Transform shopRoomTransform;
    //private Transform shopObjectTransform;

    public List<string> EssentialObjectNames;

    [HideInInspector]
    public Dictionary<string, Material[]> MaterialEssentialObjectsDefault;

    [HideInInspector]
    public List<GameObject> CurrentObjects;

    public bool ObjectSelected
    {
        get { return SelectedObjectIndex > -1; }
    }

    /// <summary>
    /// (CALL ONCE!) Sets up a reference to the correct parent object for instantiating office items and creates walls, floor and ceiling materials.
    /// </summary>
    public void Initialize()
    {
        MaterialWallsCurrent = new Material(MaterialWallsDefault);
        MaterialFloorCurrent = new Material(MaterialFloorDefault);
        MaterialCeilingCurrent = new Material(MaterialCeilingDefault);
        MaterialWallsShopCurrent = new Material(MaterialWallsDefault);

        MaterialWallsCurrent.name += " (Office)";
        MaterialWallsShopCurrent.name += " (Shop)";

        officeRoomTransform = GameObject.Find(OfficeParentName).transform.Find(RoomParentName);
        officeObjectTransform = GameObject.Find(OfficeParentName).transform.Find(ObjectsParentName);
        shopRoomTransform = GameObject.Find(ShopParentName).transform.Find(ShopRoomParentName);
        //shopObjectTransform = GameObject.Find(ShopParentName).transform.Find(ShopObjectParentName);

        MaterialEssentialObjectsDefault = new Dictionary<string, Material[]>();

        CurrentObjects = new List<GameObject>();

        if (EssentialObjectNames.Count > 0)
        {
            for (int i = 0; i < EssentialObjectNames.Count; i++)
            {
                GameObject essentialObject = GameObject.Find(EssentialObjectNames[i]);

                essentialObject.GetComponent<OfficeObjectScript>().Essential = true;
                essentialObject.GetComponent<OfficeObjectScript>().ObjectIndex = CurrentObjects.Count;

                MaterialEssentialObjectsDefault.Add(essentialObject.name, essentialObject.GetComponent<Renderer>().sharedMaterials);

                CurrentObjects.Add(essentialObject);
            }
        }

        Transform wallsTransform = officeRoomTransform.Find("Walls");

        for (int i = 0; i < wallsTransform.childCount; i++)
        {
            wallsTransform.GetChild(i).gameObject.GetComponent<Renderer>().sharedMaterial = MaterialWallsCurrent;
        }

        wallsTransform = shopRoomTransform.Find("Walls");

        for (int i = 0; i < wallsTransform.childCount; i++)
        {
            wallsTransform.GetChild(i).gameObject.GetComponent<Renderer>().sharedMaterial = MaterialWallsShopCurrent;
        }
    }

    /// <summary>
    /// Sets up the office by using the specified customization data.
    /// </summary>
    /// <param name="customizationData"></param>
    public void SetUpOffice(OfficeCustomizationData data)
    {
        RemoveAllOfficeObjects();

        MaterialWallsCurrent.color = data.GetWallsColor();
        MaterialFloorCurrent.color = data.GetFloorColor();
        MaterialCeilingCurrent.color = data.GetCeilingColor();

        if (data.OfficeItems.Count > 0)
        {
            for (int i = 0; i < data.OfficeItems.Count; i++)
            {
                OfficeItem officeItem = data.OfficeItems[i];
                GameObject newOfficeObject = null;

                if (officeItem.ItemID > -1)
                {
                    int iObject;

                    InitializeOfficeObject(officeItem.ItemID, out iObject);

                    newOfficeObject = CurrentObjects[iObject];
                }
                else
                {
                    foreach (GameObject obj in CurrentObjects)
                    {
                        if (obj.name == officeItem.ObjectName)
                        {
                            newOfficeObject = obj;
                            break;
                        }
                    }
                }

                newOfficeObject.transform.position = officeItem.GetPosition();
                newOfficeObject.transform.rotation = officeItem.GetRotation();
            }

            int iCurrentObjectsStart = 0;

            foreach (OfficeObjectDependency dependency in data.Dependencies)
            {
                for (int iCurrentObjects = iCurrentObjectsStart; iCurrentObjects < CurrentObjects.Count; iCurrentObjects++)
                {
                    OfficeObjectScript objScript = CurrentObjects[iCurrentObjects].GetComponent<OfficeObjectScript>();

                    if (objScript.ObjectIndex == dependency.ObjectIndexChild)
                    {
                        objScript.SetParent(dependency.ObjectIndexParent);

                        iCurrentObjectsStart++;
                        iCurrentObjects = CurrentObjects.Count; //break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Instantiates a new office object with DEFAULT position and rotation, and sets its held values for office item ID and object list index.
    /// </summary>
    /// <param name="officeItemId"></param>
    /// <returns></returns>
    public void InitializeOfficeObject(int officeItemId, out int objectIndex)
    {
        if (CurrentObjects.Count <= MaxNumberOfObjects)
        {
            GameObject officeObject = Instantiate(Items[officeItemId].Object, officeObjectTransform);

            objectIndex = CurrentObjects.Count;

            officeObject.GetComponent<OfficeObjectScript>().Initialize(officeItemId, objectIndex);

            CurrentObjects.Add(officeObject);
        }
        else
        {
            //TEMP:
            objectIndex = -1;
            Debug.Log("*Maximum number of items placed.");
        }
    }

    public void SelectObject(int objectIndex)
    {
        CurrentObjects[objectIndex].GetComponent<OfficeObjectScript>().Select();
        SelectedObjectIndex = objectIndex;
    }

    public void PlaceObject(GameObject parentObj)
    {
        if (parentObj != null && parentObj.GetComponent<OfficeObjectScript>() != null)
        {
            CurrentObjects[SelectedObjectIndex].GetComponent<OfficeObjectScript>().SetParent(parentObj.GetComponent<OfficeObjectScript>().ObjectIndex);
        }
        else
        {
            CurrentObjects[SelectedObjectIndex].GetComponent<OfficeObjectScript>().ParentIndex = -1;
            CurrentObjects[SelectedObjectIndex].transform.parent = officeObjectTransform;
        }
        
        CurrentObjects[SelectedObjectIndex].GetComponent<OfficeObjectScript>().Deselect();
        SelectedObjectIndex = -1;
    }

    /// <summary>
    /// (Call after user selects to remove an object from their office?)
    /// </summary>
    /// <param name="objectIndex"></param>
    public void RemoveOfficeObject(int objectIndex)
    {
        if (!CurrentObjects[objectIndex].GetComponent<OfficeObjectScript>().Essential)
        {
            Destroy(CurrentObjects[objectIndex]);
            CurrentObjects.RemoveAt(objectIndex);

            UpdateObjectIndexes();
        }
        else
            Debug.Log("Cannot remove essential office items!");
    }

    /// <summary>
    /// Destroys all currently instantiated office objects and clears the currentObjects list.
    /// </summary>
    public void RemoveAllOfficeObjects()
    {
        List<GameObject> essentialObjects = new List<GameObject>();

        if (CurrentObjects.Count > 0)
        {
            for (int i = 0; i < CurrentObjects.Count; i++)
            {
                if (!CurrentObjects[i].GetComponent<OfficeObjectScript>().Essential)
                    Destroy(CurrentObjects[i]);
                else
                    essentialObjects.Add(CurrentObjects[i]);
            }

            CurrentObjects.Clear();

            if (essentialObjects.Count > 0)
            {
                for (int i = 0; i < essentialObjects.Count; i++)
                {
                    CurrentObjects.Add(essentialObjects[i]);
                }
            }
        }
    }

    /// <summary>
    /// Returns save data for current office state.
    /// </summary>
    /// <returns></returns>
    public OfficeCustomizationData GetCustomizationData()
    {
        OfficeCustomizationData data = new OfficeCustomizationData(MaterialWallsCurrent.color, MaterialWallsShopCurrent.color, MaterialFloorCurrent.color, MaterialCeilingCurrent.color);

        UnsetAllObjectParents();

        if (CurrentObjects.Count > 0)
        {
            OfficeItem officeItem;

            foreach (GameObject obj in CurrentObjects)
            {
                OfficeObjectScript objScript = obj.GetComponent<OfficeObjectScript>();

                if (objScript.OfficeItemID > -1)
                {
                    officeItem = new OfficeItem(objScript.OfficeItemID, obj.transform.position, obj.transform.rotation);
                }
                else
                {
                    officeItem = new OfficeItem(obj.name, obj.transform.position, obj.transform.rotation);
                }

                data.OfficeItems.Add(officeItem);

                if (objScript.ParentIndex != -1)
                {
                    data.Dependencies.Add(new OfficeObjectDependency(objScript.ObjectIndex, objScript.ParentIndex));
                }
            }
        }

        ResetAllObjectParents();

        return data;
    }

    public void UnsetAllObjectParents()
    {
        foreach (GameObject obj in CurrentObjects)
        {
            obj.transform.parent = officeObjectTransform;
        }
    }

    public void ResetAllObjectParents()
    {
        foreach (GameObject obj in CurrentObjects)
        {
            OfficeObjectScript objScript = obj.GetComponent<OfficeObjectScript>();

            if (objScript.ParentIndex != -1)
            {
                objScript.SetParent(objScript.ParentIndex);
            }
            else
            {
                obj.transform.parent = officeObjectTransform;
            }
        }
    }

    public void ResetRoomColors()
    {
        MaterialWallsCurrent.color = MaterialWallsDefault.color;
        MaterialFloorCurrent.color = MaterialFloorDefault.color;
        MaterialCeilingCurrent.color = MaterialCeilingDefault.color;
    }

    public float GetValue()
    {
        float value = 0;

        foreach (GameObject obj in CurrentObjects)
        {
            OfficeObjectScript objScript = obj.GetComponent<OfficeObjectScript>();

            if (objScript != null)
            {
                if (!objScript.Essential)
                    value += Items[objScript.OfficeItemID].Price;
            }
            else
            {
                Debug.Log("*INVALID ITEM IN OFFICE ITEMS*");
            }
        }

        return value;
    }

    private void UpdateObjectIndexes()
    {
        for (int i = 0; i < CurrentObjects.Count; i++)
        {
            int oldIndex = CurrentObjects[i].GetComponent<OfficeObjectScript>().ObjectIndex;

            CurrentObjects[i].GetComponent<OfficeObjectScript>().ObjectIndex = i;

            for (int x = i + 1; x < CurrentObjects.Count; x++)
            {
                if (CurrentObjects[i].GetComponent<OfficeObjectScript>().ParentIndex == oldIndex)
                {
                    CurrentObjects[i].GetComponent<OfficeObjectScript>().ParentIndex = i;
                }
            }
        }
    }
}
