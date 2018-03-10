using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float targetHeightOffset;
    public float maxDistanceFromTarget, minDistanceFromTarget;
    public float zoomSpeed = 1f;

    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    // Called after all other update functions have been called
    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform.position + new Vector3(0, targetHeightOffset, 0));
    }

    private void FixedUpdate()
    {
        //***Might need to change Input method for this to work on all devices.
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > minDistanceFromTarget)
                offset += transform.forward * zoomSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < maxDistanceFromTarget)
                offset -= transform.forward * zoomSpeed * Time.deltaTime;
        }
    }
}
