using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Office Item Database", menuName = "Office Customization/Database")]
public class OfficeItemDatabaseSO : ScriptableObject
{
    public Material MaterialWallsDefault;
    public Material MaterialFloorDefault;
    public Material MaterialCeilingDefault;

    [HideInInspector]
    public Material MaterialWallsCurrent;
    [HideInInspector]
    public Material MaterialFloorCurrent;
    [HideInInspector]
    public Material MaterialCeilingCurrent;

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

        for (int i = 0; i < officeRoomTransform.Find("Walls").childCount; i++)
        {
            officeRoomTransform.Find("Walls").GetChild(i).gameObject.GetComponent<Renderer>().sharedMaterial = MaterialWallsCurrent;
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
                GameObject newOfficeObject = InitializeOfficeObject(officeItem.ItemID);

                newOfficeObject.transform.position = officeItem.GetPosition();
                newOfficeObject.transform.rotation = officeItem.GetRotation();
            }
        }
    }

    /// <summary>
    /// (Call from UI?) Instantiates a new office object with DEFAULT position and rotation, and sets its held values for office item ID and object list index.
    /// </summary>
    /// <param name="officeItemId"></param>
    /// <returns></returns>
    public GameObject InitializeOfficeObject(int officeItemId)
    {
        if (currentObjects.Count <= MaxNumberOfObjects)
        {
            GameObject officeObject = Instantiate(Items[officeItemId].Object, officeObjectTransform);

            officeObject.GetComponent<OfficeObjectScript>().OfficeItemID = officeItemId;
            officeObject.GetComponent<OfficeObjectScript>().ObjectIndex = currentObjects.Count;

            currentObjects.Add(officeObject);

            return officeObject;
        }
        else
        {
            //TEMP:
            Debug.Log("*Maximum number of items placed.");

            return null;
        }
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
