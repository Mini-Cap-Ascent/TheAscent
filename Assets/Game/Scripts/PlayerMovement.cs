using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Fusion;
using Cinemachine;
using UnityEngine.AddressableAssets;


public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private Animator animator;

    private NetworkCharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        animator = GetComponent<Animator>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Vector2 inputDirection = data.direction;
            if (inputDirection != Vector2.zero)
            {
                Vector3 worldDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
                worldDirection = Camera.main.transform.TransformDirection(worldDirection);
                worldDirection.y = 0;
                worldDirection.Normalize();

                _cc.Move(moveSpeed * worldDirection * Runner.DeltaTime);

                Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);

                // Set the animator "Speed" parameter
                animator.SetFloat("Speed", moveSpeed * inputDirection.magnitude);
            }
            else
            {
                // Make sure to set "Speed" to 0 if there's no input
                animator.SetFloat("Speed", 0);
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

