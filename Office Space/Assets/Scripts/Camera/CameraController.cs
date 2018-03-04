using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public GameObject playerTarget;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - playerRigidbody.transform.position;
        transform.LookAt(playerTarget.transform);
    }

    // Called after all other update functions have been called
    void LateUpdate()
    {
        transform.position = playerRigidbody.transform.position + offset;
    }
}
