using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //NB: This script controls the Rigidbody of the player; collisions work better this way.

    //Public variables can be edited within Unity, so that the script doesn't need to recompile every time if you had to change them here
    public float walkSpeed = 1f;            // Walking speed
    public float turnSpeed = 1f;       // Turning Speed

    Vector3 movement;                   // Stores the direction of the player's movement.
    Animator animator;                  // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    Quaternion newRotation;             //Used to get the player's rotation in Quaternions

    //Awake() is like Start() but is called regardless of whether the script is enabled or not.
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // FixedUpdate() is called before performing any physics calculations
    private void FixedUpdate()
    {
        // Store the input axes.
        //GetAxisRaw() is used so that movement is instant instead of gradual.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(horizontal, vertical);

        // Animate the player.
        Animating(horizontal, vertical);
    }

    void Move(float h, float v)
    {
        if (h != 0 || v != 0)
        {
            // Set the movement vector based on the axis input.
            movement.Set(h, 0, v);

            Turn();

            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * walkSpeed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition(transform.position + movement);
        }
    }

    void Turn()
    {
        newRotation = Quaternion.LookRotation(movement);

        //Gradually rotates player (from current rotation) to new rotation. NOTE Time.deltaTime multiplier!
        playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, newRotation, Time.deltaTime * turnSpeed);

        //Instantly points player to new rotation
        //playerRigidbody.MoveRotation(newRotation);
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is not equal to 0.
        bool walking = h != 0 || v != 0;

        // Tell the animator whether or not the player is walking.
        animator.SetBool("IsWalking", walking);
    }
}
