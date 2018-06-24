using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeObjectScript : MonoBehaviour
{
    public int OfficeItemID = -1;
    public int ObjectIndex = -1;

    private bool selected = false;
    private bool highlighted = false;
    private bool placementValid = true;

    private int collisionCount = 0;

    public void Initialize(int officeItemId, int objectIndex)
    {
        OfficeItemID = officeItemId;
        ObjectIndex = objectIndex;
    }

    public void Select()
    {
        selected = true;
        highlighted = true;

        gameObject.layer = 2;

        GetComponent<Collider>().isTrigger = true;

        SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);
    }

    public void Deselect()
    {
        selected = false;
        highlighted = false;

        gameObject.layer = GameMaster.Instance.CustomizationManager.Office.OfficeItemLayer;

        GetComponent<Collider>().isTrigger = false;

        ResetObjectMaterials();
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
                    GameMaster.Instance.CustomizationManager.Office.PlaceObject();
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
                }

                switch (placement)
                {
                    case OfficeItemPosition.Floor:
                        {
                            ray = new Ray(newPos, Vector3.down);

                            if (Physics.Raycast(ray, out hit))
                            {
                                newPos = hit.point;
                            }

                            break;
                        }
                    case OfficeItemPosition.Wall:
                        {
                            break;
                        }
                    case OfficeItemPosition.Ceiling:
                        {
                            ray = new Ray(newPos, Vector3.up);

                            if (Physics.Raycast(ray, out hit))
                            {
                                newPos = hit.point;
                            }

                            break;
                        }
                }

                transform.position = newPos;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (GameMaster.Instance.BuildMode && GameMaster.Instance.CustomizationManager.Office.SelectedObjectIndex == -1)
        {
            highlighted = true;

            SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);
        }
    }

    private void OnMouseExit()
    {
        if (GameMaster.Instance.BuildMode && GameMaster.Instance.CustomizationManager.Office.SelectedObjectIndex == -1)
        {
            highlighted = false;

            ResetObjectMaterials();
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
}
