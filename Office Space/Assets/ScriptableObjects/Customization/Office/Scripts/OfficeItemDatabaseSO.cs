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

    public float ObjectInteractDistance = 2f;

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

    private Transform officeRoomTransform;
    private Transform officeObjectTransform;
    private Transform shopRoomTransform;
    //private Transform shopObjectTransform;

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

        OfficeObjectScript[] objs = FindObjectsOfType<OfficeObjectScript>();

        for (int i = 0; i < objs.Length; i++)
        {
            Debug.Log("* " + objs[i].gameObject.name);
            objs[i].Essential = true;
            objs[i].ObjectIndex = CurrentObjects.Count;

            Renderer[] renderers = objs[i].gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                MaterialEssentialObjectsDefault.Add(renderer.gameObject.name, renderer.sharedMaterials);
            }

            CurrentObjects.Add(objs[i].gameObject);
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

    public void SelectObject(GameObject obj)
    {
        int id = obj.GetComponent<OfficeObjectScript>().ObjectIndex;
        SelectObject(id);
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
        GameObject obj = CurrentObjects[objectIndex];

        if (!obj.GetComponent<OfficeObjectScript>().Essential)
        {
            RaycastHit hit;

            List<Transform> children = new List<Transform>();

            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;

                if (child.GetComponent<OfficeObjectScript>() != null)
                {
                    children.Add(child.transform);

                    child.GetComponent<OfficeObjectScript>().ParentIndex = -1;
                    child.GetComponent<OfficeObjectScript>().Deselect();
                    child.transform.parent = officeObjectTransform;

                    i--;
                }
            }

            obj.layer = 2;
            Destroy(obj);
            CurrentObjects.RemoveAt(objectIndex);

            UpdateObjectIndexes();

            SelectedObjectIndex = -1;

            foreach (Transform child in children)
            {
                if (Physics.Raycast(child.position, Vector3.down, out hit))
                {
                    child.position = hit.point;

                    OfficeObjectScript objScript = hit.collider.gameObject.GetComponent<OfficeObjectScript>();

                    if (objScript != null)
                    {
                        child.gameObject.GetComponent<OfficeObjectScript>().SetParent(objScript.ObjectIndex);

                        //Debug.Log("New parent: " + objScript.gameObject.name);
                    }
                    //else
                    //    Debug.Log("New parent: " + child.parent.name);
                }
            }
        }
        else
            Debug.Log("Cannot remove essential office items!");
    }

    /// <summary>
    /// Destroys all currently instantiated office objects and clears the currentObjects list.
    /// </summary>
    public void RemoveAllOfficeObjects()
    {
        if (CurrentObjects.Count > 0)
        {
            for (int i = 0; i < CurrentObjects.Count; i++)
            {
                if (!CurrentObjects[i].GetComponent<OfficeObjectScript>().Essential)
                {
                    RemoveOfficeObject(i);
                    i--;
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

            for (int i = 0; i < CurrentObjects.Count; i++)
            {
                GameObject obj = CurrentObjects[i];
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

            //foreach (GameObject obj in CurrentObjects)
            //{
            //    OfficeObjectScript objScript = obj.GetComponent<OfficeObjectScript>();

            //    if (objScript.OfficeItemID > -1)
            //    {
            //        officeItem = new OfficeItem(objScript.OfficeItemID, obj.transform.position, obj.transform.rotation);
            //    }
            //    else
            //    {
            //        officeItem = new OfficeItem(obj.name, obj.transform.position, obj.transform.rotation);
            //    }

            //    data.OfficeItems.Add(officeItem);

            //    if (objScript.ParentIndex != -1)
            //    {
            //        data.Dependencies.Add(new OfficeObjectDependency(objScript.ObjectIndex, objScript.ParentIndex));
            //    }
            //}
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

    public float GetTotalValue()
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

    /// <summary>
    /// Result 0 = Items repossessed, Result 1 = Lifelines used, all items gone
    /// </summary>
    /// <param name="debtValue"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool RepossessItems(float debtValue, out int result)
    {
        bool sufficientValue = true;

        float officeValue = GetTotalValue();
        float accumulatedValue = 0f;
        float remainingValue = 0f;

        result = -1;

        if (officeValue >= debtValue)
        {
            result = 0;

            while (accumulatedValue < debtValue)
            {
                float lowestValue = Mathf.Infinity;
                int iObjectToRemove = -1;

                for (int iObject = 0; iObject < CurrentObjects.Count; iObject++)
                {
                    OfficeObjectScript obj = CurrentObjects[iObject].GetComponent<OfficeObjectScript>();

                    if (!obj.Essential)
                    {
                        if (obj.SO.Price < lowestValue)
                        {
                            lowestValue = obj.SO.Price;
                            iObjectToRemove = iObject;
                        }
                    }
                }

                Debug.Log("*REMOVE: " + CurrentObjects[iObjectToRemove].name);
                RemoveOfficeObject(iObjectToRemove);
                accumulatedValue += lowestValue;
            }

            remainingValue = accumulatedValue - debtValue;

            Debug.Log("*REQUIRED VALUE: " + debtValue.ToString());
            Debug.Log("*ACCUMULATED VALUE: " + accumulatedValue.ToString());
        }
        else if (GameMaster.Instance.Player.HasLifeLine)
        {
            result = 1;

            GameMaster.Instance.Player.HasLifeLine = false;

            Debug.Log("*LIFELINE USED");

            RemoveAllOfficeObjects();
        }
        else
        {
            sufficientValue = false;
        }

        return sufficientValue;
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
