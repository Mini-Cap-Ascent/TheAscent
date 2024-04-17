using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Fusion;
using Cinemachine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using System.Linq;


public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 50f;
    public float deadZone = 0.1f;
    private Animator animator;
    private bool canMove = true;
    private WeaponManager weaponManager;
    private WeaponPickup pickupScript;
    private NetworkCharacterController _cc;
    private NetworkHealth healthComponent;
    private NetworkObject networkObject;


    private void Awake()
    {

        _cc = GetComponent<NetworkCharacterController>();
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<NetworkHealth>();
        networkObject = GetComponent<NetworkObject>();
        weaponManager = GetComponent<WeaponManager>();
        pickupScript = GetComponent<WeaponPickup>();
    }
    //private void Update()
    //{
    //    // Since we're using Update to check for input, it's okay to not use FixedUpdateNetwork.
    //    // However, we should check for input authority.
    //    if (!networkObject.HasInputAuthority) return;

    //    // Handle jump input
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        HandleJumpInput();
    //    }
    //}

    private void HandleJumpInput()
    {
        _cc.RequestJump();

    }
    public override void FixedUpdateNetwork()
    {

        if (!canMove) return;

        Vector2 inputDirection = Vector2.zero;

        // Get the input data
        if (GetInput(out NetworkInputData data))
        {
            inputDirection = data.direction;
        }

        // Smaller deadZone value for more responsive input handling
        float deadZone = 0.05f;  // Reduced from 0.1f to 0.05f

        // Rotate based on horizontal input
        if (Mathf.Abs(inputDirection.x) > deadZone)
        {
            float rotationAmount = inputDirection.x * rotationSpeed * Runner.DeltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }

        // Move forward/backward based on vertical input
        if (Mathf.Abs(inputDirection.y) > deadZone)
        {
            Vector3 moveDirection = transform.forward * inputDirection.y;
            _cc.Move(moveSpeed * moveDirection * Runner.DeltaTime);
            animator.SetFloat("Speed", moveSpeed * inputDirection.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // Jumping action
        if (data.jumpPressed)
        {
            _cc.RequestJump();
            animator.SetTrigger("IsJumping");
        }

        // Handle attack inputs
        HandleAttackInput();
    }

    public void LockMovement(float lockDuration) {

        if (networkObject.HasStateAuthority)
        {
            canMove = false;
            StartCoroutine(UnlockMovement(lockDuration));
        }

    
    }

    private IEnumerator UnlockMovement(float lockDuration)
    {
        yield return new WaitForSeconds(lockDuration);
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BigBall"))
        {
            // Check for authority
            if (networkObject.HasStateAuthority)
            {
                // Directly call TakeDamage on the healthComponent
                healthComponent?.TakeDamage(20, collision.collider.gameObject);

                // Trigger the hit animation
                if (animator != null)
                {
                    animator.SetTrigger("Hit");
                    LockMovement(2.0f); // Lock movement for 2 seconds, for example
                }
            }
        }
    }

    private void HandleAttackInput()
    {
        if (!networkObject.HasInputAuthority) return; // Only handle input if we have the authority

        // Check for attack input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Determine if we're moving
            bool isMoving = _cc.Velocity.magnitude > 0;
            if (isMoving)
            {
                _cc.MovingAttack();
            }
            else
            {
                _cc.IdleAttack();
            }
        }
    }
    public void ActivateJumpBoost(float boostMultiplier, float duration)
    {
        StartCoroutine(JumpBoostRoutine(boostMultiplier, duration));
    }

    private IEnumerator JumpBoostRoutine(float boostMultiplier, float duration)
    {
        if (_cc != null)
        {
            _cc.jumpImpulse *= boostMultiplier; // Increase the jump impulse
        }

        yield return new WaitForSeconds(duration); // Wait for the duration of the power-up

        if (_cc != null)
        {
            _cc.jumpImpulse /= boostMultiplier; // Reset the jump impulse
        }
    }
    // Method to attach a weapon to the player


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponSpawnPoint"))
        {
            // You might have a script on the weapon object that holds its details
            WeaponPickup weaponDetails = other.GetComponent<WeaponPickup>();

            // Publish the weapon pickup event
            EventBus.Instance.Publish(new WeaponPickupEvent(weaponDetails.weaponName, other.gameObject));

            // Optionally, deactivate the weapon spawn point if it should disappear after pickup
            other.gameObject.SetActive(false);
        }
    }

}


