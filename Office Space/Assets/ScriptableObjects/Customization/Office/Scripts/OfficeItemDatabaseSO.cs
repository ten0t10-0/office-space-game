using System.Collections;
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
    public int SelectedObjectIndex = -1;

    public string OfficeParentName;
    public string RoomParentName;
    public string ObjectsParentName;

    public int MaxNumberOfObjects = 15;

    public List<OfficeItemSO> Items;
    public List<OfficeItemTypeSO> Types;

    public List<int> DefaultOfficeItemIndexes;

    private Transform officeRoomTransform;
    private Transform officeObjectTransform;

    private List<GameObject> currentObjects;

    /// <summary>
    /// Sets up a reference to the correct parent object for instantiating office items and creates walls, floor and ceiling materials.
    /// </summary>
    public void Initialize()
    {
        MaterialWallsCurrent = new Material(MaterialWallsDefault);
        MaterialFloorCurrent = new Material(MaterialFloorDefault);
        MaterialCeilingCurrent = new Material(MaterialCeilingDefault);

        officeRoomTransform = GameObject.Find(OfficeParentName).transform.Find(RoomParentName);
        officeObjectTransform = GameObject.Find(OfficeParentName).transform.Find(ObjectsParentName);

        currentObjects = new List<GameObject>();

        Transform wallsTransform = officeRoomTransform.Find("Walls");

        for (int i = 0; i < wallsTransform.childCount; i++)
        {
            wallsTransform.GetChild(i).gameObject.GetComponent<Renderer>().sharedMaterial = MaterialWallsCurrent;
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
            foreach(OfficeItem officeItem in data.OfficeItems)
            {
                int iObject;

                InitializeOfficeObject(officeItem.ItemID, out iObject);

                GameObject newOfficeObject = currentObjects[iObject];
                newOfficeObject.transform.position = officeItem.GetPosition();
                newOfficeObject.transform.rotation = officeItem.GetRotation();
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
        if (currentObjects.Count <= MaxNumberOfObjects)
        {
            GameObject officeObject = Instantiate(Items[officeItemId].Object, officeObjectTransform);

            objectIndex = currentObjects.Count;

            officeObject.GetComponent<OfficeObjectScript>().Initialize(officeItemId, objectIndex);

            currentObjects.Add(officeObject);
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
        currentObjects[objectIndex].GetComponent<OfficeObjectScript>().Select();
        SelectedObjectIndex = objectIndex;
    }

    public void PlaceObject()
    {
        currentObjects[SelectedObjectIndex].GetComponent<OfficeObjectScript>().Deselect();
        SelectedObjectIndex = -1;
    }

    public GameObject GetOfficeObject(int objectIndex)
    {
        return currentObjects[objectIndex];
    }

    /// <summary>
    /// (Call after user selects to remove an object from their office?)
    /// </summary>
    /// <param name="objectIndex"></param>
    public void RemoveOfficeObject(GameObject obj)
    {
        int objectIndex = obj.GetComponent<OfficeObjectScript>().ObjectIndex;

        Destroy(currentObjects[objectIndex]);
        currentObjects.RemoveAt(objectIndex);

        UpdateObjectIndexes();
    }

    /// <summary>
    /// Destroys all currently instantiated office objects and clears the currentObjects list.
    /// </summary>
    public void RemoveAllOfficeObjects()
    {
        if (currentObjects.Count > 0)
        {
            for (int i = 0; i < currentObjects.Count; i++)
            {
                Destroy(currentObjects[i]);
            }

            currentObjects.Clear();
        }
    }

    /// <summary>
    /// Returns save data for current office state.
    /// </summary>
    /// <returns></returns>
    public OfficeCustomizationData GetCustomizationData()
    {
        OfficeCustomizationData data = new OfficeCustomizationData(MaterialWallsCurrent.color, MaterialFloorCurrent.color, MaterialCeilingCurrent.color);

        if (currentObjects.Count > 0)
        {
            OfficeItem officeItem;

            for (int i = 0; i < currentObjects.Count; i++)
            {
                officeItem = new OfficeItem(currentObjects[i].GetComponent<OfficeObjectScript>().OfficeItemID, currentObjects[i].transform.position, currentObjects[i].transform.rotation);

                data.OfficeItems.Add(officeItem);
            }
        }

        return data;
    }

    public void ResetRoomColors()
    {
        MaterialWallsCurrent.color = MaterialWallsDefault.color;
        MaterialFloorCurrent.color = MaterialFloorDefault.color;
        MaterialCeilingCurrent.color = MaterialCeilingDefault.color;
    }

    private void UpdateObjectIndexes()
    {
        for (int i = 0; i < currentObjects.Count; i++)
        {
            currentObjects[i].GetComponent<OfficeObjectScript>().ObjectIndex = i;
        }
    }
}
