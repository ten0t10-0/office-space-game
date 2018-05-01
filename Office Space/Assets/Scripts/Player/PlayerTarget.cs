using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    //The position of "target" acts as the starting point for where the camera will point to.
    //The Offset value is added to the position of the target so that the camera's position (for now, height only) is adjusted.

    public Transform target;
    [Range(2.25f, 3.75f)]
    public float yOffset = 2.5f;

    private void LateUpdate()
    {
        //Places the target above the Root bone by adding the Y Offset.
        transform.position = target.position + new Vector3(0, yOffset, 0);
    }
}
