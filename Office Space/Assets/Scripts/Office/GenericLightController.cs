using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericLightController : MonoBehaviour
{
    private Light lightComponent;
    private bool highlighted = false;

    private void Awake()
    {
        lightComponent = GetComponentInChildren<Light>();

        if (lightComponent == null)
        {
            Debug.Log("***Can't find Light component for " + name + "!");
        }
    }

    private void OnMouseEnter()
    {
        if (!GameMaster.Instance.BuildMode && !GameMaster.Instance.UIMode)
        {
            highlighted = true;
        }
    }

    private void OnMouseExit()
    {
        highlighted = false;
    }

    private void Update()
    {
        if (highlighted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                float distanceToPlayer = Vector3.Distance(GameMaster.Instance.CurrentPlayerObject.transform.position, transform.position);

                if (distanceToPlayer <= GameMaster.Instance.CustomizationManager.Office.ObjectInteractDistance)
                {
                    lightComponent.enabled = !lightComponent.enabled;
                }
            }
        }
    }
}
