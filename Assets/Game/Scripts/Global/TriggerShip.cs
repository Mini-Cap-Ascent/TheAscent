using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShip : MonoBehaviour
{
    [SerializeField]
    private Elevator wayPointPath; // Reference to the Elevator script that holds the waypoints
    [SerializeField]
    private float speed = 1.0f; // Speed of the platform movement

    private int targetWayPointIndex = 0;
    private Transform targetWayPoint;
    private Transform previousWayPoint;

    private float timeToWayPoint;
    private float elapsedTime = 0.0f;

    private bool isActive = false; // Control activation of the platform

    void Start()
    {
        if (wayPointPath.waypoints.Length > 0)
        {
            previousWayPoint = wayPointPath.GetWaypoint(targetWayPointIndex);
            targetWayPoint = wayPointPath.GetWaypoint(targetWayPointIndex);
            transform.position = previousWayPoint.position;
        }
    }

    void Update()
    {
        if (isActive && previousWayPoint != null && targetWayPoint != null)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        elapsedTime += Time.deltaTime;
        float elapsedPercentage = elapsedTime / timeToWayPoint;
        transform.position = Vector3.Lerp(previousWayPoint.position, targetWayPoint.position, elapsedPercentage);

        if (elapsedPercentage >= 1.0f)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        previousWayPoint = targetWayPoint;
        targetWayPointIndex = wayPointPath.NextIndex(targetWayPointIndex, true);
        targetWayPoint = wayPointPath.GetWaypoint(targetWayPointIndex);

        elapsedTime = 0.0f;
        if (previousWayPoint != null && targetWayPoint != null)
        {
            float distanceToWayPoint = Vector3.Distance(previousWayPoint.position, targetWayPoint.position);
            timeToWayPoint = distanceToWayPoint / speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Activate the platform when a specific object enters the trigger
        if (other.CompareTag("Player")) // Ensure the player or another object has the correct tag
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: Deactivate when the object exits the trigger
        if (other.CompareTag("Player"))
        {
            isActive = false;
        }
    }
}
