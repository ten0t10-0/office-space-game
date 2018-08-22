using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode { ThirdPerson, FirstPerson, Static }

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

    [HideInInspector]
    public Vector3 Offset;

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
        CameraMode = CameraMode.ThirdPerson;

        //Set initial camera position.
        transform.position = targetController.transform.position + new Vector3(0, 0, (minDistanceFromTarget + zoomSpeed) * -1);
        Offset = targetController.transform.position - transform.position;

        //Cursor.visible = false;
    }

    // Called after all other update functions have been called
    void LateUpdate()
    {
        if (!GameMaster.Instance.UIMode)
        {
            float horizontal = Input.GetAxisRaw("Mouse X") * horizontalSensitivity; //*
            float vertical = Input.GetAxisRaw("Mouse Y") * verticalSensitivity * -1; //*

            int layerMask = 1 << GameMaster.Instance.CustomizationManager.Office.OfficeItemLayer | 1 << 2;
            layerMask = ~layerMask;

            switch (CameraMode)
            {
                case CameraMode.ThirdPerson:
                    {
                        Vector3 targetPosition;

                        Vector3 currentEulerAngles = targetController.transform.rotation.eulerAngles;
                        float currentXAngle = currentEulerAngles.x;

                        Quaternion newRotation;
                        Vector3 newPosition;

                        RaycastHit wallHit;

                        float offsetX;

                        targetController.transform.position = Target.position + (Target.up * OffsetY);

                        if ((currentXAngle % 360) > 180)
                            currentXAngle = currentXAngle - 360;

                        currentXAngle = Mathf.Clamp(currentXAngle + vertical, minTargetAngle, maxTargetAngle);

                        newRotation = Quaternion.Euler(new Vector3(currentXAngle, currentEulerAngles.y + horizontal, 0));
                        targetController.transform.rotation = newRotation;

                        if (orbitOTS)
                        {
                            offsetX = OffsetX;

                            Vector3 tempOrigin = targetController.transform.position;
                            tempOrigin.y = (tempOrigin - (newRotation * Offset)).y ;

                            if (Physics.SphereCast(tempOrigin, physicsSphereRadius, targetController.transform.right, out wallHit, OffsetX, layerMask))
                            {
                                float modifier = physicsSphereRadius - (physicsSphereRadius * (wallHit.distance / OffsetX));
                                offsetX = wallHit.distance - modifier;
                            }

                            targetPosition = targetController.transform.position + (targetController.transform.right * offsetX);
                        }
                        else
                        {
                            targetPosition = targetController.transform.position;
                        }

                        //New position of the camera before taking collision into account:
                        newPosition = targetPosition - (newRotation * Offset); //<Quaternion> * <Vector3> applies the rotation (Quaternion) to the Vector3. Not sure how this works...

                        //Check for collision:
                        if (Physics.SphereCast(targetPosition, physicsSphereRadius, targetController.transform.forward * -1, out wallHit, Vector3.Distance(targetPosition, newPosition), layerMask))
                            newPosition = (wallHit.point + (wallHit.normal * physicsSphereRadius)); //Set the camera's new position to the point where the sphere touched the wall, then a bit away from it

                        transform.position = newPosition;

                        transform.LookAt(targetPosition);

                        break;
                    }
                case CameraMode.FirstPerson:
                    {
                        Vector3 targetControllerPosition = Target.gameObject.GetComponent<PlayerController>().HeadTransform.position;
                        targetControllerPosition.y += Target.gameObject.GetComponent<PlayerController>().HeadTransform.gameObject.GetComponent<SphereCollider>().radius;

                        Vector3 targetPosition;

                        Vector3 currentEulerAngles = targetController.transform.rotation.eulerAngles;
                        float currentXAngle = currentEulerAngles.x;

                        Quaternion newRotation;
                        Vector3 newPosition;

                        targetController.transform.position = targetControllerPosition;

                        if ((currentXAngle % 360) > 180)
                            currentXAngle = currentXAngle - 360;

                        currentXAngle = Mathf.Clamp(currentXAngle + vertical, maxTargetAngle * -1, minTargetAngle * -1);

                        newRotation = Quaternion.Euler(new Vector3(currentXAngle, currentEulerAngles.y + horizontal, 0));
                        targetController.transform.rotation = newRotation;

                        targetPosition = targetController.transform.position;

                        //New position of the camera before taking collision into account:
                        newPosition = targetPosition;

                        ////Check for collision:
                        //if (Physics.SphereCast(targetPosition, physicsSphereRadius, targetController.transform.forward * -1, out wallHit, Vector3.Distance(targetPosition, newPosition), layerMask))
                        //    newPosition = (wallHit.point + (wallHit.normal * physicsSphereRadius)); //Set the camera's new position to the point where the sphere touched the wall, then a bit away from it

                        transform.position = newPosition;

                        transform.rotation = targetController.transform.rotation;

                        break;
                    }
            }
        }
    }

    private void Update()
    {
        if (!GameMaster.Instance.UIMode)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (CameraMode == CameraMode.ThirdPerson)
                {
                    if (Offset.z > minDistanceFromTarget)
                        Offset -= Vector3.forward * zoomSpeed;
                    else
                    {
                        CameraMode = CameraMode.FirstPerson;
                    }
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (CameraMode == CameraMode.ThirdPerson)
                {
                    if (Offset.z < maxDistanceFromTarget)
                        Offset += Vector3.forward * zoomSpeed;
                }
                else if (CameraMode == CameraMode.FirstPerson)
                {
                    Offset.z = minDistanceFromTarget;
                    CameraMode = CameraMode.ThirdPerson;
                }
            }

            //if (Input.GetKeyDown(KeyCode.Tab))
            //    orbitOTS = !orbitOTS;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (CameraMode == CameraMode.ThirdPerson)
                    CameraMode = CameraMode.FirstPerson;
                else if (CameraMode == CameraMode.FirstPerson)
                    CameraMode = CameraMode.ThirdPerson;
            }
        }
    }

    public void ChangeMode(CameraMode cameraMode)
    {
        CameraMode = cameraMode;
    }
}
