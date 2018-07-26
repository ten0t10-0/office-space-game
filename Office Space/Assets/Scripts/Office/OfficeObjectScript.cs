using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeObjectScript : MonoBehaviour
{
    public int OfficeItemID = -1;
    public int ObjectIndex = -1;

    public int ParentIndex = -1;
    public bool ParentSelected = false;

    public bool Essential = false;
    public bool UseCustomPlacement = false;
    public OfficeItemPosition CustomPlacement;

    private bool selected = false;
    private bool highlighted = false;
    private bool placementValid = true;

    private int collisionCount = 0;

    private GameObject tempObj = null;

    public void Initialize(int officeItemId, int objectIndex)
    {
        OfficeItemID = officeItemId;
        ObjectIndex = objectIndex;

        ParentIndex = -1;
    }

    public void SetParent(int parentIndex)
    {
        ParentIndex = parentIndex;
        transform.parent = GameMaster.Instance.CustomizationManager.Office.CurrentObjects[parentIndex].transform;
    }

    public void Select()
    {
        List<GameObject> children = GetChildren();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].GetComponent<OfficeObjectScript>().ParentSelected = true;
        }

        SelectSetup();

        selected = true;
        highlighted = true;
    }

    public void Deselect()
    {
        List<GameObject> children = GetChildren();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].GetComponent<OfficeObjectScript>().ParentSelected = false;
        }

        DeselectSetup();

        selected = false;
        highlighted = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GameMaster.Instance.BuildMode)
            {
                if (highlighted && !selected)
                {
                    GameMaster.Instance.CustomizationManager.Office.SelectObject(ObjectIndex);
                }
                else if (selected && placementValid)
                {
                    GameMaster.Instance.CustomizationManager.Office.PlaceObject(tempObj);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameMaster.Instance.BuildMode && selected)
            {
                transform.Rotate(Vector3.up, -45f);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameMaster.Instance.BuildMode && selected)
            {
                transform.Rotate(Vector3.up, 45f);
            }
        }
    }

    private void LateUpdate()
    {
        if (GameMaster.Instance.BuildMode && !GameMaster.Instance.UIMode)
        {
            if (selected)
            {
                float objectPlacementDistance = GameMaster.Instance.CustomizationManager.Office.ObjectPlacementDistance;

                OfficeItemPosition placement;

                if (!UseCustomPlacement)
                {
                    if (OfficeItemID > -1)
                        placement = GameMaster.Instance.CustomizationManager.Office.Items[OfficeItemID].Placement;
                    else
                    {
                        placement = OfficeItemPosition.Floor;
                        Debug.Log("OfficeItemID = -1!");
                    }
                }
                else
                {
                    placement = CustomPlacement;
                }

                Vector3 newPos;

                //*TEMP?:
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    //Target at screen center (Camera direction)
                    newPos = Camera.main.transform.position + (Camera.main.transform.forward * (Camera.main.GetComponent<CameraController>().Offset.z + objectPlacementDistance));
                }
                else
                {
                    //Target at mouse position (Camera to mouse direction)
                    newPos = Camera.main.transform.position + (Camera.main.ScreenPointToRay(Input.mousePosition).direction * (Camera.main.GetComponent<CameraController>().Offset.z + objectPlacementDistance));
                }

                Ray ray;
                RaycastHit hit;

                if (Physics.Linecast(Camera.main.transform.position, newPos, out hit))
                {
                    newPos = hit.point;

                    tempObj = hit.collider.gameObject;
                }
                else
                {
                    tempObj = null;
                }

                switch (placement)
                {
                    case OfficeItemPosition.Floor:
                        {
                            ray = new Ray(newPos, Vector3.down);

                            if (Physics.Raycast(ray, out hit))
                            {
                                newPos = hit.point;

                                tempObj = hit.collider.gameObject;
                            }
                            else
                            {
                                tempObj = null;
                            }

                            break;
                        }
                    case OfficeItemPosition.Wall:
                        {
                            break;
                        }
                    case OfficeItemPosition.Ceiling:
                        {
                            //ray = new Ray(newPos, Vector3.up);

                            //if (Physics.Raycast(ray, out hit))
                            //{
                            //    newPos = hit.point;
                            //}

                            break;
                        }
                }

                transform.position = newPos;
            }
        }
        else
        {
            if (highlighted)
            {
                highlighted = false;

                Dehighlight();
            }
        }
    }

    private void OnMouseEnter()
    {
        if (GameMaster.Instance.BuildMode && !GameMaster.Instance.CustomizationManager.Office.ObjectSelected)
        {
            highlighted = true;

            Highlight();
        }
    }

    private void OnMouseExit()
    {
        if (GameMaster.Instance.BuildMode && !GameMaster.Instance.CustomizationManager.Office.ObjectSelected)
        {
            highlighted = false;

            Dehighlight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameMaster.Instance.BuildMode && selected)
        {
            collisionCount++;

            if (placementValid)
            {
                placementValid = false;

                SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialInvalidPlacement);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameMaster.Instance.BuildMode && selected)
        {
            collisionCount--;

            if (collisionCount == 0)
            {
                placementValid = true;

                SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);
            }
        }
    }

    private void SetObjectMaterials(Material material)
    {
        Material[] newMaterials = new Material[GetComponent<Renderer>().sharedMaterials.Length];

        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = material;
        }

        GetComponent<Renderer>().sharedMaterials = newMaterials;

        Debug.Log("Materials: " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    }

    private void ResetObjectMaterials()
    {
        if (OfficeItemID > -1)
            GetComponent<Renderer>().sharedMaterials = GameMaster.Instance.CustomizationManager.Office.Items[OfficeItemID].Object.GetComponent<Renderer>().sharedMaterials;
        else
            GetComponent<Renderer>().sharedMaterials = GameMaster.Instance.CustomizationManager.Office.MaterialEssentialObjectsDefault[name];
    }

    private List<GameObject> GetChildren()
    {
        List<GameObject> children = new List<GameObject>();

        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.GetComponent<OfficeObjectScript>() != null)
            {
                children.Add(child);
            }
        }

        return children;
    }

    private List<GameObject> GetFamily()
    {
        List<GameObject> family = new List<GameObject>();

        List<GameObject> children = GetChildren();

        if (children.Count > 0)
        {
            for (int iChild = 0; iChild < children.Count; iChild++)
            {
                family.Add(children[iChild]);

                List<GameObject> childFamily = children[iChild].GetComponent<OfficeObjectScript>().GetFamily();

                for (int iChildFamily = 0; iChildFamily < childFamily.Count; iChildFamily++)
                {
                    family.Add(childFamily[iChildFamily]);
                }
            }
        }

        return family;
    }

    public void SelectSetup()
    {
        gameObject.layer = 2;

        GetComponent<Collider>().isTrigger = true;

        SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);

        List<GameObject> family = GetFamily();

        if (family.Count > 0)
        {
            foreach (GameObject member in family)
            {
                member.GetComponent<OfficeObjectScript>().SelectSetup();
            }
        }
    }

    public void DeselectSetup()
    {
        gameObject.layer = GameMaster.Instance.CustomizationManager.Office.OfficeItemLayer;

        GetComponent<Collider>().isTrigger = false;

        ResetObjectMaterials();

        List<GameObject> family = GetFamily();

        if (family.Count > 0)
        {
            foreach (GameObject member in family)
            {
                member.GetComponent<OfficeObjectScript>().DeselectSetup();
            }
        }
    }

    private void Highlight()
    {
        SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);

        List<GameObject> family = GetFamily();

        if (family.Count > 0)
        {
            foreach (GameObject member in family)
            {
                member.GetComponent<OfficeObjectScript>().Highlight();
            }
        }
    }

    private void Dehighlight()
    {
        ResetObjectMaterials();

        List<GameObject> family = GetFamily();

        if (family.Count > 0)
        {
            foreach (GameObject member in family)
            {
                member.GetComponent<OfficeObjectScript>().Dehighlight();
            }
        }
    }
}
