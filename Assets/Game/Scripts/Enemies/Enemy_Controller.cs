using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{

 
    public Animator animator;
    public GameObject WaypointParents;
    public List<GameObject> Waypoints = new List<GameObject>();
    public int currentWaypoint = 0;
    public NavMeshAgent agent;



    [HideInInspector]public NavMeshPath path;
    public bool drawPath = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        if(WaypointParents != null)
        {
            foreach(Transform child in WaypointParents.transform)
            {

                if (!Waypoints.Contains(child.gameObject)) { 
                
                
                    Waypoints.Add(child.gameObject);


                }


            }
        }else
        {
            Debug.LogError("WaypointParents not assigned");
        }

        
    }


    public bool isAtDestination()
    {
        Debug.Log($"Remaining Distance: {agent.remainingDistance}, Stopping Distance: {agent.stoppingDistance}, Path Pending: {agent.pathPending}");
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public void MoveToNextWayPoint() { 
    
        if(Waypoints.Count == 0)
        {
            Debug.LogError("No Waypoints assigned");
            return;
        }

       currentWaypoint = (currentWaypoint + 1) % Waypoints.Count;
        GameObject nextWaypoint = Waypoints[currentWaypoint];
        agent.SetDestination(nextWaypoint.transform.position);

    }

}
