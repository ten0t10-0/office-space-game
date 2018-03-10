using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool cameraMouseControl;
    public GameObject target;
    public float initialYOffset = 2f; //not used if cameraMouseControl is true.
    public float maxDistanceFromTarget, minDistanceFromTarget;
    public float zoomSpeed = 1f;

    private Vector3 offset;

    private float maxTargetAngle = 89.9f, minTargetAngle = -89.9f; //used cameraMouseControl is true. Used to limit the camera target rotation so that it stops rotating when it is above or below the player.

    // Use this for initialization
    void Start()
    {
        //Set initial camera position.
        if (!cameraMouseControl)
        {
            transform.position = target.transform.position + new Vector3(0, initialYOffset, ((maxDistanceFromTarget + minDistanceFromTarget) / 2) * -1);
            offset = target.transform.position - transform.position;
        }
        else
        {
            transform.position = target.transform.position + new Vector3(0, 0, ((maxDistanceFromTarget + minDistanceFromTarget) / 2) * -1);
            offset = target.transform.position - transform.position;

            Cursor.visible = false;
        }
    }

    // Called after all other update functions have been called
    void LateUpdate()
    {
        if (!cameraMouseControl)
        {
            transform.position = target.transform.position - offset;

            transform.LookAt(target.transform.position);
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Mouse X") * 2; //*
            float vertical = Input.GetAxisRaw("Mouse Y") * 2 * -1; //*

            Vector3 currentEulerAngles = target.transform.rotation.eulerAngles;
            Vector3 newEulerAngles;

            float currentXAngle = currentEulerAngles.x;

            if ((currentXAngle % 360) > 180)
                currentXAngle = currentXAngle - 360;

            currentXAngle = Mathf.Clamp(currentXAngle + vertical, minTargetAngle, maxTargetAngle);

            newEulerAngles = new Vector3(currentXAngle, currentEulerAngles.y + horizontal, 0);

            Quaternion newRotation = Quaternion.Euler(newEulerAngles);

            target.transform.rotation = newRotation;

            transform.position = target.transform.position - (newRotation * offset); //Quaternion * Vector3 applies the rotation to the Vector3. Not sure how this works...

            transform.LookAt(target.transform.position);
        }
    }

    private void FixedUpdate()
    {
        //***Might need to change Input method for this to work on all devices.
        if (!cameraMouseControl)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Vector3.Distance(target.transform.position, transform.position) > minDistanceFromTarget)
                    offset -= transform.forward * zoomSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Vector3.Distance(target.transform.position, transform.position) < maxDistanceFromTarget)
                    offset += transform.forward * zoomSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Vector3.Distance(target.transform.position, transform.position) > minDistanceFromTarget)
                    offset -= Vector3.forward * zoomSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Vector3.Distance(target.transform.position, transform.position) < maxDistanceFromTarget)
                    offset += Vector3.forward * zoomSpeed * Time.deltaTime;
            }
        }
    }
}
