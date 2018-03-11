using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool cameraMouseControl;
    public Transform target;
    public float initialYOffset = 2f; //not used if cameraMouseControl is true.
    public float maxDistanceFromTarget, minDistanceFromTarget;
    public float zoomSpeed = 1f;

    private Vector3 offset;

    private float maxTargetAngle = 89.9f, minTargetAngle = -45f; //used cameraMouseControl is true. Used to limit the camera target rotation so that it stops rotating when it is above or below the player.

    // Use this for initialization
    void Start()
    {
        //Set initial camera position.
        if (!cameraMouseControl)
        {
            transform.position = target.position + new Vector3(0, initialYOffset, ((maxDistanceFromTarget + minDistanceFromTarget) / 2) * -1);
            offset = target.position - transform.position;
        }
        else
        {
            transform.position = target.position + new Vector3(0, 0, ((maxDistanceFromTarget + minDistanceFromTarget) / 2) * -1);
            offset = target.position - transform.position;

            Cursor.visible = false;
        }
    }

    // Called after all other update functions have been called
    void LateUpdate()
    {
        if (!cameraMouseControl)
        {
            transform.position = target.position - offset;

            transform.LookAt(target.position);
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Mouse X") * 2; //*
            float vertical = Input.GetAxisRaw("Mouse Y") * 2 * -1; //*

            Vector3 currentEulerAngles = target.rotation.eulerAngles;
            float currentXAngle = currentEulerAngles.x;

            Vector3 newEulerAngles;
            Vector3 newPosition;

            RaycastHit wallHit;

            if ((currentXAngle % 360) > 180)
                currentXAngle = currentXAngle - 360;

            currentXAngle = Mathf.Clamp(currentXAngle + vertical, minTargetAngle, maxTargetAngle);

            newEulerAngles = new Vector3(currentXAngle, currentEulerAngles.y + horizontal, 0);

            Quaternion newRotation = Quaternion.Euler(newEulerAngles);
            target.rotation = newRotation;

            //New position of the camera before taking collision into account:
            newPosition = target.position - (newRotation * offset); //Quaternion * Vector3 applies the rotation to the Vector3. Not sure how this works...

            //Check for collision:
            if (Physics.SphereCast(target.position, 1f, target.forward * -1, out wallHit, Vector3.Distance(target.position, newPosition)))
                newPosition = wallHit.point + wallHit.normal; //Set the camera's new position to the point where the sphere touched the wall, then a bit away from it

            transform.position = newPosition;

            transform.LookAt(target.position);
        }
    }

    private void FixedUpdate()
    {
        //***Might need to change Input method for this to work on all devices.
        if (!cameraMouseControl)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Vector3.Distance(target.position, transform.position) > minDistanceFromTarget)
                    offset -= transform.forward * zoomSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Vector3.Distance(target.position, transform.position) < maxDistanceFromTarget)
                    offset += transform.forward * zoomSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Vector3.Distance(target.position, transform.position) > minDistanceFromTarget)
                    offset -= Vector3.forward * zoomSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Vector3.Distance(target.position, transform.position) < maxDistanceFromTarget)
                    offset += Vector3.forward * zoomSpeed * Time.deltaTime;
            }
        }
    }
}
