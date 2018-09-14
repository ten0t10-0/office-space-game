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

    private Vector3? position_temp;
    private Quaternion rotation_temp;

    public OfficeItemSO SO
    {
        get
        {
            if (OfficeItemID > -1)
                return GameMaster.Instance.CustomizationManager.Office.Items[OfficeItemID];
            else
                return null;
        }
    }

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
        if (GameMaster.Instance.BuildMode && !GameMaster.Instance.UIMode)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (highlighted && !selected)
                {
                    position_temp = transform.position;
                    rotation_temp = transform.rotation;

                    GameMaster.Instance.CustomizationManager.Office.SelectObject(ObjectIndex);
                }
                else if (selected && placementValid)
                {
                    position_temp = null;

                    GameMaster.Instance.CustomizationManager.Office.PlaceObject(tempObj);
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (selected)
                {
                    if (ParentIndex != -1)
                        tempObj = transform.parent.gameObject;

                    GameMaster.Instance.CustomizationManager.Office.PlaceObject(tempObj);
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (selected)
                {
                    GameMaster.Instance.CustomizationManager.Office.RemoveOfficeObject(ObjectIndex);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (GetObjectPlacement() != OfficeItemPosition.Wall)
                {
                    if (GameMaster.Instance.BuildMode && selected)
                    {
                        transform.Rotate(Vector3.up, -45f);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (GetObjectPlacement() != OfficeItemPosition.Wall)
                {
                    if (GameMaster.Instance.BuildMode && selected)
                    {
                        transform.Rotate(Vector3.up, 45f);
                    }
                }
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

                OfficeItemPosition placement = GetObjectPlacement();

                if (placement == OfficeItemPosition.None)
                {
                    objectPlacementDistance = 1f;
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

                tempObj = null;

                if (Physics.Linecast(Camera.main.transform.position, newPos, out hit))
                {
                    //Debug.Log("#Cast 01: " + hit.collider.name);

                    newPos = hit.point;

                    if (GetComponent<Rigidbody>() == null)
                        tempObj = hit.collider.gameObject;
                }

                switch (placement)
                {
                    case OfficeItemPosition.Floor:
                        {
                            ray = new Ray(newPos + Vector3.up, Vector3.down);

                            if (Physics.Raycast(ray, out hit))
                            {
                                //Debug.Log("#Cast 02: " + hit.collider.name);

                                newPos = hit.point;

                                if (!hit.collider.gameObject.GetComponent<Rigidbody>())
                                {
                                    tempObj = hit.collider.gameObject;

                                    if (collisionCount == 0)
                                    {
                                        SetPlacementValidation(true);
                                    }
                                }
                                else
                                {
                                    SetPlacementValidation(false);
                                }
                            }
                            else
                            {
                                tempObj = null;
                            }

                            if (!tempObj)
                                SetPlacementValidation(false);

                            break;
                        }
                    case OfficeItemPosition.Wall:
                        {
                            if (hit.normal != Vector3.zero && hit.collider.gameObject.layer != GameMaster.Instance.CustomizationManager.Office.OfficeItemLayer)
                            {
                                Quaternion newRotation = Quaternion.LookRotation(hit.normal);

                                if (newRotation.eulerAngles.x == 0)
                                {
                                    if (collisionCount == 0)
                                    {
                                        SetPlacementValidation(true);
                                    }

                                    transform.rotation = newRotation;
                                }
                                else
                                {
                                    SetPlacementValidation(false);
                                }
                            }
                            else
                            {
                                SetPlacementValidation(false);
                            }

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
                    case OfficeItemPosition.None:
                        {
                            break;
                        }
                }

                transform.position = newPos;
            }
        }
        else
        {
            if (selected)
            {
                GameMaster.Instance.CustomizationManager.Office.PlaceObject(tempObj);
            }
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

            SetPlacementValidation(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameMaster.Instance.BuildMode && selected)
        {
            collisionCount--;

            if (collisionCount == 0)
            {
                SetPlacementValidation(true);
            }
        }
    }

    private void SetPlacementValidation(bool valid)
    {
        if (valid)
        {
            if (!placementValid)
            {
                placementValid = true;

                SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);

                List<GameObject> familiy = GetFamily();

                foreach (GameObject member in familiy)
                {
                    member.GetComponent<OfficeObjectScript>().SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialHighlighted);
                }
            }
        }
        else
        {
            if (placementValid)
            {
                placementValid = false;

                SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialInvalidPlacement);

                List<GameObject> familiy = GetFamily();

                foreach (GameObject member in familiy)
                {
                    member.GetComponent<OfficeObjectScript>().SetObjectMaterials(GameMaster.Instance.CustomizationManager.Office.MaterialInvalidPlacement);
                }
            }
        }
    }

    private OfficeItemPosition GetObjectPlacement()
    {
        OfficeItemPosition placement;

        if (!UseCustomPlacement)
        {
            if (OfficeItemID > -1)
                placement = SO.Placement;
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

        return placement;
    }

    private void SetObjectMaterials(Material material)
    {
        //Material[] newMaterials = new Material[GetComponent<Renderer>().sharedMaterials.Length];

        //for (int i = 0; i < newMaterials.Length; i++)
        //{
        //    newMaterials[i] = material;
        //}

        //GetComponent<Renderer>().sharedMaterials = newMaterials;

        //Debug.Log("Materials: " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);

        List<Renderer> renderers = GetObjectRenderers();

        foreach (Renderer r in renderers)
        {
            if (r.sharedMaterials.Length == 1)
                r.sharedMaterial = material;
            else
            {
                Material[] newMaterials = new Material[r.sharedMaterials.Length];

                for (int i = 0; i < newMaterials.Length; i++)
                    newMaterials[i] = material;

                r.sharedMaterials = newMaterials;
            }
        }
    }

    private void ResetObjectMaterials()
    {
        List<Renderer> renderers = GetObjectRenderers();

        if (OfficeItemID > -1)
        {
            Renderer[] renderersDefault = SO.Object.GetComponentsInChildren<Renderer>();
            Dictionary<string, Material[]> materialsDefault = new Dictionary<string, Material[]>();

            Material[] matArray;

            for (int i = 0; i < renderersDefault.Length; i++)
            {
                matArray = renderersDefault[i].sharedMaterials;

                materialsDefault.Add(renderersDefault[i].gameObject.name, matArray);
            }

            foreach (Renderer r in renderers)
            {
                string objName = r.gameObject.name;

                if (!GameMaster.Instance.CustomizationManager.Office.MaterialEssentialObjectsDefault.ContainsKey(objName))
                {
                    if (!materialsDefault.ContainsKey(objName))
                    {
                        objName = objName.Substring(0, objName.Length - "(Clone)".Length);
                    }

                    r.sharedMaterials = materialsDefault[objName];
                }
            }
        }
        else
        {
            foreach (Renderer r in renderers)
            {
                r.sharedMaterials = GameMaster.Instance.CustomizationManager.Office.MaterialEssentialObjectsDefault[r.gameObject.name];
            }
        }
    }

    private List<Renderer> GetObjectRenderers()
    {
        List<Renderer> result = new List<Renderer>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (GetComponent<Renderer>() != null)
            result.Add(GetComponent<Renderer>());

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].gameObject.GetComponent<OfficeObjectScript>() == null)
            {
                result.Add(renderers[i]);
            }
        }

        return result;
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

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

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
        if (position_temp.HasValue)
        {
            transform.position = position_temp.Value;
            transform.rotation = rotation_temp;
        }

        gameObject.layer = GameMaster.Instance.CustomizationManager.Office.OfficeItemLayer;

        GetComponent<Collider>().isTrigger = false;

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }

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
