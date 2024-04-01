using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Fusion;
using Cinemachine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;


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

        if(!canMove) return;

        Vector2 inputDirection = Vector2.zero;

        if (GetInput(out NetworkInputData data))
        {
            inputDirection = data.direction;
        }

        if (Mathf.Abs(inputDirection.x) > deadZone)
        {
            float rotationAmount = inputDirection.x * rotationSpeed * Runner.DeltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }

        if (Mathf.Abs(inputDirection.y) > deadZone)
        {
            Vector3 moveDirection = transform.forward * inputDirection.y; 
            _cc.Move(moveSpeed * moveDirection * Runner.DeltaTime);
            // Set the animator "Speed" parameter
            animator.SetFloat("Speed", moveSpeed * inputDirection.magnitude);
        } 
        else
        {
            animator.SetFloat("Speed", 0);
        }
        if (data.jumpPressed)
        {
            _cc.RequestJump();
            
            animator.SetTrigger("IsJumping");
        }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword") || other.gameObject.CompareTag("Gun"))
        {
            // This should only happen on the client
            if (networkObject.HasInputAuthority)
            {
                // Request the server to equip the weapon
                RequestEquipWeapon(other.GetComponent<NetworkObject>());
            }
        }
    }
    private void RequestEquipWeapon(NetworkObject weaponPickupNetworkObject)
    {
        // Ensure this is the client making the request
        if (!networkObject.HasInputAuthority) return;

        // RPC or Command to request server to equip weapon
        // This is pseudocode and will depend on your implementation
        weaponManager.RPC_ServerEquipWeapon(weaponPickupNetworkObject);
    }
    
   
}


