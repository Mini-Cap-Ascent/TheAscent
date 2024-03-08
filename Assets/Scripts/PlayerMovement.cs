using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private PlayerCont controls;
    private Vector2 move;
    private Rigidbody rb;

    // Spline movement specific variables
    public SplineContainer spline; // Assuming the spline is a BezierSpline
    private bool onSpline = false;
    private float progressAlongSpline = 0f;// Progress along the spline.

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
        Camera mainCamera = Camera.main;

        if (onSpline)
        {
            // This needs to be the actual input method for moving along the spline.
            progressAlongSpline += move.x * Time.fixedDeltaTime * moveSpeed;
            progressAlongSpline = Mathf.Clamp01(progressAlongSpline);

            // Here we need to replace this with the actual method for evaluating the spline position.
            // Pseudocode: You'll need to implement this based on Unity's spline API
            Vector3 splinePosition = GetPointAlongSpline(progressAlongSpline);
            rb.MovePosition(splinePosition);

            // Unity's spline might not give you the tangent directly, this is a placeholder.
            // Pseudocode: You'll need to implement this or a similar method
            Quaternion splineRotation = GetRotationAlongSpline(progressAlongSpline);
            rb.MoveRotation(splineRotation);
        }
        else
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 movement = (cameraForward * move.y + cameraRight * move.x) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                rb.MoveRotation(toRotation);
            }
        }
    }

    // Call this method to toggle whether the player is on the spline or not.
    public void ToggleSplineMovement(bool isOnSpline)
    {
        onSpline = isOnSpline;
        // Reset the progress when attaching to the spline.
        if (onSpline)
        {
            progressAlongSpline = 0f;
        }
    }

    Vector3 GetPointAlongSpline(float progress)
    {
        // If the Spline API provides a method to evaluate the position at a certain progress.
        // Replace 'EvaluatePosition' with the actual method name from the Unity Spline API.
        return spline.EvaluatePosition(progress);
    }

    Quaternion GetRotationAlongSpline(float progress)
    {
        // If the Spline API provides a method to evaluate the tangent at a certain progress.
        // Replace 'EvaluateTangent' with the actual method name from the Unity Spline API.
        Vector3 tangent = spline.EvaluateTangent(progress);
        // Construct a rotation looking in the direction of the tangent.
        // This assumes 'up' is the global Y-axis; modify as needed for your use case.
        return Quaternion.LookRotation(tangent, Vector3.up);
    }
}

