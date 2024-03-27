using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : SteeringBehaviourBase
{
    public Transform centralPoint;
    public float interestRadius = 10f; // How close it gets to the central point
    public float checkInterval = 5f; // How often to check for a change in behavior
    public float wanderRadius = 2f;
    public float wanderDistance = 5f;
    public float wanderJitter = 1f;

    private Vector3 wanderTarget;
    private float lastCheckTime = 0f;
    private bool isInterested = false; // Whether the spaceship is currently interested in the central point

    void Start()
    {
        wanderTarget = transform.position + (Vector3.forward * wanderDistance);
    }

    public override Vector3 CalculateForce()
    {
        if (Time.time - lastCheckTime > checkInterval)
        {
            lastCheckTime = Time.time;
            isInterested = Random.value > 0.5f; // Randomly decide if it's interested in the central point
        }

        if (isInterested && centralPoint != null)
        {
            // Move towards the central point
            Vector3 directionToCenter = (centralPoint.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, centralPoint.position);
            if (distance > interestRadius)
            {
                // If outside the interest radius, move closer
                return directionToCenter * steeringAgent.maxSpeed - steeringAgent.velocity;
            }
            else
            {
                // If inside the interest radius, wander around
                isInterested = false; // Reset interest to encourage wandering away
            }
        }

        // Regular wandering behavior outside
        float jitterTimeSlice = wanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(Random.Range(-5f, 5f) * jitterTimeSlice, 0, Random.Range(-1f, 1f) * jitterTimeSlice);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformPoint(targetLocal) - transform.position;
        targetWorld.y = 0; // Keep movement in 2D plane, if necessary

        return targetWorld.normalized * steeringAgent.maxSpeed - steeringAgent.velocity;
    }
}


