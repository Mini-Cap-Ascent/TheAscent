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
    public float jumpHeight = 10.0f;
    public float jumpDuration = 2.0f; // Duration of the jump from start to peak
    private bool wantToJump = false;
    private bool isJumping = false;
    private float jumpStartTime;
    private Animator animator;
    private NetworkCharacterController _cc;
    private CinemachineOrbitalTransposer orbitalTransposer;
    private Vector2 move;
    private Vector2 rotate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _cc = GetComponent<NetworkCharacterController>();
        orbitalTransposer = FindObjectOfType<CinemachineOrbitalTransposer>();

        // Setup your input system here
        var controls = new PlayerCont();
        controls.PlayerControlz.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.PlayerControlz.Move.canceled += ctx => move = Vector2.zero;
        controls.CameraControll.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.CameraControll.Rotate.canceled += ctx => rotate = Vector2.zero;
        controls.PlayerControlz.Jump.performed += ctx => OnJumpInput();
        controls.Enable();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            bool isMoving = data.direction.magnitude > 0;

            // Handle Movement
            if (isMoving)
            {
                _cc.Move(moveSpeed * data.direction * Runner.DeltaTime);
                animator.SetFloat("Speed", _cc.Velocity.magnitude);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }

            // Handle Rotation
            if (move != Vector2.zero)
            {
                Vector3 moveDirection = new Vector3(move.x, 0, move.y);
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            HandleJump();
        }
    }

    

    private void Update()
    {
        // Assuming you want to rotate the camera in Update
        if (orbitalTransposer != null && rotate != Vector2.zero)
        {
            float targetRotationValue = orbitalTransposer.m_XAxis.Value + rotate.x * rotationSpeed * Time.deltaTime;
            orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, targetRotationValue, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleJump()
    {
        if (wantToJump && !isJumping)
        {
            StartJump();
        }
        else if (isJumping)
        {
            ContinueJump();
        }
    }

    private void StartJump()
    {
        animator.SetTrigger("IsJumping");
        jumpStartTime = Time.time;
        isJumping = true;
        wantToJump = false;
    }

    private void ContinueJump()
    {
        float elapsedTime = Time.time - jumpStartTime;
        if (elapsedTime < jumpDuration)
        {
            float height = (-4 * jumpHeight / (jumpDuration * jumpDuration)) * (elapsedTime * elapsedTime - jumpDuration * elapsedTime);
            Vector3 newPosition = _cc.transform.position + Vector3.up * height * Time.deltaTime;
            _cc.Move(newPosition - _cc.transform.position);
        }
        else
        {
            isJumping = false; // End jump
        }
    }

    public void OnJumpInput()
    {
        if (!_cc.Grounded) return;
        wantToJump = true;
    }
}

