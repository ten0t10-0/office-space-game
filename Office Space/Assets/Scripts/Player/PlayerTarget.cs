using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    //"targetRoot" is the actual object the camera will be targeting, but it will ignore its y position and point towards the initial local y position of "target" instead.
    //This is done so that the camera can technically point towards "target" but not point to its realtime position.
    //Eg. if we make "target" the head, and always point straight towards its realtime position, the camera will move up and down when you walk around (which can be irritating, motion sickness?)

    public GameObject targetRoot;
    public GameObject target;

    private float offsetYFromRoot;

    private void Start()
    {
        //Gets the distance between "targetRoot" y position & "target" y position. This will get the initial local y position of "target".
        offsetYFromRoot = target.transform.position.y - targetRoot.transform.position.y;
    }

    private void LateUpdate()
    {
        //Places the target above the Root bone by adding the distance calculated in Start() to the Y position.
        transform.position = targetRoot.transform.position + new Vector3(0, offsetYFromRoot, 0);
    }
}
