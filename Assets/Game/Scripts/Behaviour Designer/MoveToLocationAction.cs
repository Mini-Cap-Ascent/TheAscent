using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskName("Move to Location")]
[TaskCategory("Custom")]
[TaskDescription("Simulates the move to location in the world. Returns Running if not at goal, otherwise success")]
public class MoveToLocationAction : Action
{
    public SharedString destination;
    public SharedFloat delay = 1.0f;

    private float currentDelay = 0.0f;

    public override void OnStart()
    {
        base.OnStart();

        currentDelay = delay.Value;
    }

    public override TaskStatus OnUpdate()
    {
        currentDelay -= Time.deltaTime;
        if (currentDelay <= 0.0f)
        {
            Debug.Log($"Finally made if to {destination.Value}");
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
