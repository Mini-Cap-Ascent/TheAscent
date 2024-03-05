using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private PlayerCont controls;
    private Vector2 move;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerCont();

        // Bind the Move action
        controls.PlayerControlz.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.PlayerControlz.Move.canceled += ctx => move = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}

