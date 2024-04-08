using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskName("PatrolAction")]
[TaskCategory("Generic")]
[TaskDescription("Patrol the area using the NavMeshAgent")]

public class PatrolAction : MoveToGoal
{

    public SharedGameObjectList waypoints;
    public bool loop = true;
    public int currentWaypointIndex = 0;

    public override void OnStart()
    {
        base.OnStart();
        if (currentWaypointIndex < waypoints.Value.Count)
        {

            agent.isStopped = false;
            agent.SetDestination(waypoints.Value[currentWaypointIndex].transform.position);

        }
    }

    public override TaskStatus OnUpdate()
    {
     
        TaskStatus baseStatus = base.OnUpdate();
        if (baseStatus != TaskStatus.Running && currentWaypointIndex != waypoints.Value.Count)
        {

            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Value.Count && loop == true) {
            
                currentWaypointIndex = 0;
            
            }

            if (currentWaypointIndex < waypoints.Value.Count)
            {
                agent.SetDestination(waypoints.Value[currentWaypointIndex].transform.position);
            
                return TaskStatus.Running;
            }

        }
        return baseStatus;
    }

}
