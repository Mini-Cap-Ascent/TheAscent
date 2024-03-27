using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    public Transform target;

    public GameObject WaypointParents;
    public float distanceToWaypoint = 0.5f;
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;
    public float sightDistance = 10.0f;
    public float cheerTime = 5.0f;
    public float fov = 70.0f;
    public LayerMask targetMask; // Ensure the player is on this LayerMask
    public LayerMask obstacleMask; // Obstacles that block line of sight

    private int index = 0;
    public List<GameObject> Waypoints = new List<GameObject>();
    public NavMeshAgent agent;
    private Animator animator;
    private float currentCheerTime = 0.0f;
    private bool canSee = false;
    public int currentWaypoint = 0;
    public float health = 100f;
    public Enemy_FSM fsm;


    [HideInInspector] public NavMeshPath path;
    public bool drawPath = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        fsm = GetComponent<Enemy_FSM>();

        if (WaypointParents != null)
        {
            foreach (Transform child in WaypointParents.transform)
            {

                if (!Waypoints.Contains(child.gameObject))
                {


                    Waypoints.Add(child.gameObject);


                }


            }
        }
        else
        {
            Debug.LogError("WaypointParents not assigned");
        }


    }


    public bool isAtDestination()
    {
        // Debug.Log($"Remaining Distance: {agent.remainingDistance}, Stopping Distance: {agent.stoppingDistance}, Path Pending: {agent.pathPending}");
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public void MoveToNextWayPoint()
    {

        if (Waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned.");
            return;
        }

        GameObject nextWaypoint = Waypoints[currentWaypoint];
        agent.SetDestination(nextWaypoint.transform.position);

        // Increment the waypoint index, wrapping back to 0 if it exceeds the list count
        currentWaypoint = (currentWaypoint + 1) % Waypoints.Count;

    }

    public bool CanSeeTarget()
    {
        Debug.Log("CanSeeTarget");
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToTarget) < fov / 2)
        {
            Debug.Log("In FOV");
            float distToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
            {
                return distToTarget <= sightDistance;
            }
        }
        return false;
    }



}
