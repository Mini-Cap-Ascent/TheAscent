using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWanderSteeringBehaviour : SteeringBehaviourBase
{
    public float wanderRadius = 10.0f;
    public float wanderDistance = 5.0f;
    public float wanderJitter = 1.0f; // This controls the random change in wander direction.

    private float wanderAngle = 0.0f;

    // Override the base class's Start method if needed.
    protected void Start()
    {
      
        wanderAngle = Random.Range(0f, 360f); // Initialize the wander angle.
    }

    public override Vector3 CalculateForce()
    {
        // Calculate the circle center in front of the agent.
        Vector3 circleCenter = steeringAgent.velocity.normalized * wanderDistance;

        // Adjust the wander angle randomly.
        wanderAngle += Random.Range(-1f, 1f) * wanderJitter;

        // Calculate the displacement vector.
        Vector3 displacement = new Vector3(0, 0, -1) * wanderRadius;
        displacement = Quaternion.AngleAxis(wanderAngle, Vector3.up) * displacement;

        // Target is the circle center plus the displacement.
        Vector3 wanderForce = circleCenter + displacement;

        // Scale the force to attempt to move at max speed.
        wanderForce = wanderForce.normalized * steeringAgent.maxSpeed;

        // The final steering force is adjusted by subtracting the current velocity.
        Vector3 steeringForce = wanderForce.normalized * steeringAgent.maxSpeed - steeringAgent.velocity;

        // Make the force more aggressive by potentially increasing its magnitude but ensuring it does not exceed maxForce.
        float forceMultiplier = 5.0f; // Adjust this value to scale the force. Be mindful to keep behavior realistic.
        steeringForce *= forceMultiplier;
        if (steeringForce.magnitude > steeringAgent.maxForce)
        {
            steeringForce = steeringForce.normalized * steeringAgent.maxForce;
        }

        return steeringForce;
    }

    void Update()
    {
        if (useMouseInput)
        {
            CheckMouseInput();
        }
    }
}
