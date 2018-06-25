using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeObjectScript : MonoBehaviour
{
    public int OfficeItemID = -1;
    public int ObjectIndex = -1;

    public int ParentIndex = -1;
    public bool ParentSelected = false;

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
        GameObject[] children = GetChildren();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].GetComponent<OfficeObjectScript>().ParentSelected = true;
        }

        SelectSetup();

        selected = true;
        highlighted = true;
    }

    public void Deselect()
    {
        GameObject[] children = GetChildren();

        for (int i = 0; i < children.Length; i++)
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

                OfficeItemPosition placement = GameMaster.Instance.CustomizationManager.Office.Items[OfficeItemID].Placement;

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
        GetComponent<Renderer>().sharedMaterials = GameMaster.Instance.CustomizationManager.Office.Items[OfficeItemID].Object.GetComponent<Renderer>().sharedMaterials;
    }

    private GameObject[] GetChildren()
    {
        GameObject[] children = new GameObject[transform.childCount];

        for (int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }

        return children;
    }

    private List<GameObject> GetFamily()
    {
        List<GameObject> family = new List<GameObject>();

        GameObject[] children = GetChildren();

        if (children.Length > 0)
        {
            for (int iChild = 0; iChild < children.Length; iChild++)
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
