using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Fusion;


public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 10.0f;
    private PlayerCont controls;
    private Vector2 move;
    private Rigidbody rb;

    private NetworkCharacterController _cc;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerCont();

        controls.PlayerControlz.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.PlayerControlz.Move.canceled += ctx => move = Vector2.zero;
        _cc = GetComponent<NetworkCharacterController>();
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

