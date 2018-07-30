﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public variables can be edited within Unity, so that the script doesn't need to recompile every time if you had to change them here
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    [HideInInspector]
    public Transform HeadTransform;

    private Animator animator;                  // Reference to the animator component.
    private Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

    private bool isRunning;
    private bool grounded;

    private int groundCount = 0;

    //Awake() is like Start() but is called regardless of whether the script is enabled or not.
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        HeadTransform = transform.Find("Player").Find("ROOT").Find("Hip_CONT").Find("Hip").Find("Spine").Find("Chest").Find("Neck").Find("Head").gameObject.transform;
    }

    // FixedUpdate() is called before performing any physics calculations
    private void FixedUpdate()
    {
        // Store the input axes.
        //GetAxisRaw() is used so that movement is instant instead of gradual.
        //***Might need to change Input method to work for all devices.

        if (!GameMaster.Instance.UIMode)
        {
            if (!Input.GetKey(KeyCode.RightShift))
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                // Move the player around the scene.
                Move(horizontal, vertical);

                // Animate the player.
                AnimateMoving(horizontal, vertical);
            }
            else
                StopAnimations();
        }
        else
            StopAnimations();
    }

    private void Update()
    {
        if (!GameMaster.Instance.UIMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (animator.GetBool("IsIdling") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Interact01"))
                {
                    animator.SetTrigger("Interact 01");
                } 
            }
        }
    }

    private void Move(float h, float v)
    {
        Vector3 currentPostion, newPosition;
        Quaternion currentRotation, newRotation;

        Vector3 cameraForward, cameraRight;     //Used to move the player according to the X and Z axes (right/left & forward/back) of the camera.

        float speed;

        //float slerpTimeStart = Time.time;

        CameraMode cameraMode = Camera.main.GetComponent<CameraController>().CameraMode;

        if (h != 0 || v != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                isRunning = true;
            else
                isRunning = false;

            currentPostion = playerRigidbody.position;
            currentRotation = playerRigidbody.rotation;

            cameraForward = Camera.main.transform.forward;
            cameraRight = Camera.main.transform.right;
            cameraForward.y = cameraRight.y = 0;    //Prevent player from moving up or down.

            newPosition = cameraForward * v + cameraRight * h;
            newRotation = Quaternion.LookRotation(newPosition, Vector3.up);

            if (isRunning)
                speed = runSpeed;
            else
                speed = walkSpeed;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Interact01"))
            {
                //NB: Rigidbody gets moved so that collisions work properly.
                //Move
                playerRigidbody.MovePosition(currentPostion + (newPosition.normalized * speed * Time.deltaTime));       //Add new position to current position.
            }

            if (cameraMode == CameraMode.ThirdPerson)
            {
                playerRigidbody.MoveRotation(Quaternion.Lerp(currentRotation, newRotation, 0.15f));    //Gradually rotate from current direction to new direction.
            }
        }

        if (cameraMode == CameraMode.FirstPerson)
        {
            cameraForward = Camera.main.transform.forward;
            cameraRight = Camera.main.transform.right;
            cameraForward.y = cameraRight.y = 0;    //Prevent player from moving up or down.

            newPosition = cameraForward;

            playerRigidbody.MoveRotation(Quaternion.LookRotation(newPosition, Vector3.up));
        }
    }

    private void AnimateMoving(float h, float v)
    {
        //// Create a boolean that is true if either of the input axes is not equal to 0.
        //bool walking = h != 0 || v != 0;

        //// Tell the animator whether or not the player is walking.
        //animator.SetBool("IsWalking", walking);

        if (h != 0 || v != 0)
        {
            if (isRunning)
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsWalking", true);
            }

            animator.SetBool("IsIdling", false);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);

            animator.SetBool("IsIdling", true);
        }
    }

    private void StopAnimations()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCount++;
            grounded = true;
            Debug.Log("Grounded!");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCount--;

            if (groundCount == 0)
            {
                grounded = false;
                Debug.Log("Airborne!");
            }
        }
    }
}
