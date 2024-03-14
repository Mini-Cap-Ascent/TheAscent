using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Fusion;
using Cinemachine;


public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 100.0f;
    private PlayerCont controls;
    private Vector2 move;
    private Vector2 rotate; // Add this line to store the rotation value
    private Rigidbody rb;
    private Animator animator;

    private NetworkCharacterController _cc;
    private CinemachineOrbitalTransposer orbitalTransposer; // Add this for camera control

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            bool isMoving = data.direction.magnitude > 0; // Check if there is movement

            // Move the character
            if (isMoving)
            {
                _cc.Move(moveSpeed * data.direction * Runner.DeltaTime);
                animator.SetFloat("Speed", data.direction.magnitude); // Set the Speed parameter for the Animator
            }
            else
            {
                animator.SetFloat("Speed", 0); // Set the Speed to 0 if not moving
            }
            if (move != Vector2.zero) {

                Vector3 moveDirection = new Vector3(move.x, 0, move.y);
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            }
            // Handle rotation here if necessary
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerCont();

        controls.PlayerControlz.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.PlayerControlz.Move.canceled += ctx => move = Vector2.zero;

        controls.CameraControll.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>(); // Add this line
        controls.CameraControll.Rotate.canceled += ctx => rotate = Vector2.zero; // Add this line

        animator = GetComponent<Animator>();

        _cc = GetComponent<NetworkCharacterController>();
        orbitalTransposer = FindObjectOfType<CinemachineOrbitalTransposer>(); // Add this line
    }

    private void Update()
    {
        // Assuming you want to rotate the camera in Update
        if (orbitalTransposer != null)
        {
            // Determine the target rotation value
            float targetRotationValue = orbitalTransposer.m_XAxis.Value + rotate.x * rotationSpeed * Time.deltaTime;

            // Smoothly interpolate the camera's rotation towards the target rotation value
            orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, targetRotationValue, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}

