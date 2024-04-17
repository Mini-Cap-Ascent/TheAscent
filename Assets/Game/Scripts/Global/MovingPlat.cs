using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{

    [SerializeField]
    private Elevator wayPointPath; // Reference to the Elevator script that holds the waypoints
    [SerializeField]
    private float speed = 1.0f; // Speed of the platform movement

    private int targetWayPointIndex = 0; // Index of the current target waypoint
    private Transform targetWayPoint; // Reference to the current target waypoint Transform
    private Transform previousWayPoint; // Reference to the previous waypoint Transform

    private float timeToWayPoint; // Time needed to reach the current waypoint
    private float elapsedTime = 0.0f; // Time elapsed since starting to move towards the current waypoint

    void Start()
    {
        if (wayPointPath.waypoints.Length > 0)
        {
            previousWayPoint = wayPointPath.GetWaypoint(targetWayPointIndex); // Initialize at the first waypoint
            targetWayPoint = wayPointPath.GetWaypoint(targetWayPointIndex);
            transform.position = previousWayPoint.position; // Start at the position of the first waypoint
            TargetNextWaypoint();
        }
    }

    void Update()
    {
        if (previousWayPoint != null && targetWayPoint != null)
        {
            // Move the platform towards the target waypoint
            elapsedTime += Time.deltaTime;
            float elapsedPercentage = elapsedTime / timeToWayPoint;
            transform.position = Vector3.Lerp(previousWayPoint.position, targetWayPoint.position, elapsedPercentage);

            if (elapsedPercentage >= 1.0f)
            {
                // When the target is reached, update to the next waypoint
                TargetNextWaypoint();
            }
        }
    }

    private void TargetNextWaypoint()
    {
        previousWayPoint = targetWayPoint; // Current becomes previous
        targetWayPointIndex = wayPointPath.NextIndex(targetWayPointIndex, true); // Calculate the next waypoint index
        targetWayPoint = wayPointPath.GetWaypoint(targetWayPointIndex); // Update the target waypoint

        // Reset the elapsed time and calculate the new travel time
        elapsedTime = 0.0f;
        if (previousWayPoint != null && targetWayPoint != null)
        {
            float distanceToWayPoint = Vector3.Distance(previousWayPoint.position, targetWayPoint.position);
            timeToWayPoint = distanceToWayPoint / speed;
        }
    }
}
