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

    private NetworkCharacterController _cc;
    private CinemachineOrbitalTransposer orbitalTransposer; // Add this for camera control

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(moveSpeed * data.direction * Runner.DeltaTime);

            // Add rotation logic here, if necessary
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

        _cc = GetComponent<NetworkCharacterController>();
        orbitalTransposer = FindObjectOfType<CinemachineOrbitalTransposer>(); // Add this line
    }

    private void Update()
    {
        // Assuming you want to rotate the camera in Update
        if (orbitalTransposer != null)
        {
            // Modify the orbitalTransposer.m_XAxis.Value based on rotate.x input
            // You might want to multiply rotate.x by a speed factor to control the rotation speed
            orbitalTransposer.m_XAxis.Value += rotate.x * rotationSpeed *Time.deltaTime;
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

