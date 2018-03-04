using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public float rotationSpeed = 180f;

    public float movementSpeed = 1f;
    public float height = 1f;

    private Vector3 positionInit;

    private void Start()
    {
        positionInit = transform.position;
    }

    void Update ()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);

        Vector3 position = transform.position;
        float newY = Mathf.Sin(Time.time * movementSpeed);
        transform.position = new Vector3(position.x, (positionInit.y + newY) * height, position.z);
    }
}
