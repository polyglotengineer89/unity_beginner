using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private float moveSpeed = 4f;

    [Header("Movement System")]
    public float walkSpeed = 4f;

    public float runSpeed = 8f;
    public float jumpHeight = 3f;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundYOffSet;
    private Vector3 spherePos;

    [SerializeField] private float gravity = -9.81f;

    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Gravity();
    }

    public void Move()
    {
        // get horizontal and vertical input as number;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Direction in normalize vector
        Vector3 directionPlayer = new Vector3(horizontal, 0f, vertical).normalized;
        velocity = moveSpeed * Time.deltaTime * directionPlayer;

        // Is sprint key is press
        if (Input.GetButton("Sprint"))
        {
            // Set the animation to run and increase our moves
            moveSpeed = runSpeed;
            animator.SetBool("isRunning", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("isRunning", false);
        }

        //Check if there is movement
        if (directionPlayer.magnitude >= 0.1f)
        {
            // Look towards Direction
            transform.rotation = Quaternion.LookRotation(directionPlayer);

            // Move
            controller.Move(velocity);
        }

        animator.SetFloat("Speed", velocity.magnitude);
    }

    private bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffSet, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    private void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0f) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmoz()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }
}