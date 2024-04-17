using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform[] waypoints;
    public bool isLooping = true; // Loop back to the start or ping-pong when reaching the end
    public bool pingPong = false; // Move back and forth between the waypoints

    public Transform GetWaypoint(int index)
    {
        if (index >= 0 && index < waypoints.Length)
        {
            return waypoints[index];
        }
        return null;
    }

    public int NextIndex(int current, bool forward = true)
    {
        if (pingPong)
        {
            if (forward)
            {
                if (current + 1 >= waypoints.Length)
                    return waypoints.Length - 2; // Turn around at the end
            }
            else
            {
                if (current - 1 < 0)
                    return 1; // Turn around at the start
            }
            return forward ? current + 1 : current - 1;
        }
        else if (isLooping)
        {
            return (current + 1) % waypoints.Length; // Loop around
        }
        else
        {
            return current + 1 < waypoints.Length ? current + 1 : current; // Stop at the last waypoint
        }
    }
}

