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

    private NetworkCharacterController _cc;
    private NetworkHealth healthComponent;
    private NetworkObject networkObject;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<NetworkHealth>();
        networkObject = GetComponent<NetworkObject>();
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

    //private void HandleJump()
    //{
    //    if (wantToJump && !isJumping)
    //    {
    //        StartJump();
    //    }
    //    else if (isJumping)
    //    {
    //        ContinueJump();
    //    }
    //}

    //private void StartJump()
    //{
    //    animator.SetTrigger("IsJumping");
    //    jumpStartTime = Time.time;
    //    isJumping = true;
    //    wantToJump = false;
    //}

    //void OnCollisionEnter(Collision collision)
    //{
    //    Enemy_Patrol_FSM enemy = collision.collider.GetComponent<Enemy_Patrol_FSM>();
    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(10f); // Deals 10 damage
    //    }
    //}
    //private void ContinueJump()
    //{
    //    float elapsedTime = Time.time - jumpStartTime;
    //    if (elapsedTime < jumpDuration)
    //    {
    //        float height = (-4 * jumpHeight / (jumpDuration * jumpDuration)) * (elapsedTime * elapsedTime - jumpDuration * elapsedTime);
    //        Vector3 newPosition = _cc.transform.position + Vector3.up * height * Time.deltaTime;
    //        _cc.Move(newPosition - _cc.transform.position);
    //    }
    //    else
    //    {
    //        isJumping = false; // End jump
    //    }
    //}

    //public void OnJumpInput()
    //{
    //    if (!_cc.Grounded) return;
    //    wantToJump = true;
    //}
}

