using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode { Orbit, Build, Static }

public class CameraController : MonoBehaviour
{
    public float verticalSensitivity = 2f, horizontalSensitivity = 2f;
    public float zoomSpeed = 15f;

    [Range(2.25f, 3.75f)]
    public float OffsetY = 2.5f;
    [Range(0f, 2f)]
    public float OffsetX = 1.25f;

    public bool orbitOTS = true;

    public float maxDistanceFromTarget, minDistanceFromTarget;

    [HideInInspector]
    public CameraMode CameraMode { get; private set; }

    [HideInInspector]
    public Transform Target { get; private set; }

    private GameObject targetController;
    private string targetControllerName = "CameraTargetController";

    private Vector3 offset;

    private float physicsSphereRadius = 0.5f;

    //Used to limit the camera target rotation so that it stops rotating when it is above or below the player.
    private float maxTargetAngle = 89.9f;
    private float minTargetAngle = -45f;

    /// <summary>
    /// Sets the camera target to the specified transform.
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        Target = target;
    }

    private void Awake()
    {
        targetController = new GameObject(targetControllerName);
    }

    // Use this method for initialization
    void Start()
    {
        CameraMode = CameraMode.Orbit;

        //Set initial camera position.
        transform.position = targetController.transform.position + new Vector3(0, 0, ((maxDistanceFromTarget + minDistanceFromTarget) / 2) * -1);
        offset = targetController.transform.position - transform.position;

        //Cursor.visible = false;
    }

    // Called after all other update functions have been called
    void LateUpdate()
    {
        if (!GameMaster.Instance.UIMode)
        {
            float horizontal = Input.GetAxisRaw("Mouse X") * horizontalSensitivity; //*
            float vertical = Input.GetAxisRaw("Mouse Y") * verticalSensitivity * -1; //*

            switch (CameraMode)
            {
                case CameraMode.Orbit:
                    {
                        Vector3 targetPosition;

                        Vector3 currentEulerAngles = targetController.transform.rotation.eulerAngles;
                        float currentXAngle = currentEulerAngles.x;

                        Quaternion newRotation;
                        Vector3 newPosition;

                        RaycastHit wallHit;

                        targetController.transform.position = Target.position + new Vector3(0, OffsetY, 0);

                        if (orbitOTS)
                        {
                            targetPosition = targetController.transform.position + (targetController.transform.right * OffsetX);
                            
                        }
                        else
                        {
                            targetPosition = targetController.transform.position;
                        }

                        if ((currentXAngle % 360) > 180)
                            currentXAngle = currentXAngle - 360;

                        currentXAngle = Mathf.Clamp(currentXAngle + vertical, minTargetAngle, maxTargetAngle);

                        newRotation = Quaternion.Euler(new Vector3(currentXAngle, currentEulerAngles.y + horizontal, 0));
                        targetController.transform.rotation = newRotation;

                        //New position of the camera before taking collision into account:
                        newPosition = targetPosition - (newRotation * offset); //<Quaternion> * <Vector3> applies the rotation (Quaternion) to the Vector3. Not sure how this works...

                        //Check for collision:
                        if (Physics.SphereCast(targetPosition, physicsSphereRadius, targetController.transform.forward * -1, out wallHit, Vector3.Distance(targetPosition, newPosition)))
                            newPosition = (wallHit.point + (wallHit.normal * physicsSphereRadius)); //Set the camera's new position to the point where the sphere touched the wall, then a bit away from it

                        transform.position = newPosition;

                        transform.LookAt(targetPosition);

                        break;
                    }
            }
        }
    }

    private void FixedUpdate()
    {
        //***Might need to change Input method for this to work on all devices.
        if (!GameMaster.Instance.UIMode)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (offset.z > minDistanceFromTarget)
                    offset -= Vector3.forward * zoomSpeed * Time.deltaTime;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (offset.z < maxDistanceFromTarget)
                    offset += Vector3.forward * zoomSpeed * Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Tab))
                orbitOTS = !orbitOTS;
        }
    }
}
