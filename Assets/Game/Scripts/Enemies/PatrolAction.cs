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
    private int index = 0;

    public override void OnStart()
    {
        base.OnStart();
        if (index < waypoints.Value.Count)
        {

            agent.isStopped = false;
            agent.SetDestination(waypoints.Value[index].transform.position);

        }
    }

    public override TaskStatus OnUpdate()
    {
     
        TaskStatus baseStatus = base.OnUpdate();
        if (baseStatus != TaskStatus.Running && index != waypoints.Value.Count)
        {

            index++;
            if (index >= waypoints.Value.Count && loop == true) {
            
                index = 0;
            
            }

            if (index < waypoints.Value.Count)
            {
                agent.SetDestination(waypoints.Value[index].transform.position);
            
                return TaskStatus.Running;
            }

        }
        return baseStatus;
    }

}
