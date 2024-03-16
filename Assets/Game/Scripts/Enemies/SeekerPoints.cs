using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerPoints : MonoBehaviour
{
    [SerializeField]
    public static float radius = .5f; // Set the radius of the circle
    public float innerCircleRadius = radius * 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the Gizmo
        // Draw a flat circle by rotating the vector around the up axis for 360 degrees
        Vector3 startVector = transform.position + Vector3.right * radius; // Starting point just to the right of the object's position
        Vector3 prevPoint = startVector;
        Vector3 nextPoint = startVector;

        for (int i = 0; i < 360; i++)
        {
            nextPoint = transform.position + Quaternion.AngleAxis(i, Vector3.up) * (startVector - transform.position);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // Adjust this value to set how big the inner circle is relative to the outer circle
        DrawCircle(innerCircleRadius);
        // Connect the end and start points to close the circle
        Gizmos.DrawLine(prevPoint, startVector);
    }

    void DrawCircle(float radius)
    {
        Vector3 startVector = transform.position + Vector3.right * radius; // Starting point
        Vector3 prevPoint = startVector;
        Vector3 nextPoint = startVector;

        for (int i = 0; i <= 360; i++)
        {
            nextPoint = transform.position + Quaternion.AngleAxis(i, Vector3.up) * (startVector - transform.position);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
