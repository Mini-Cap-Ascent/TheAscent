using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekSteeringBehaviour : SteeringBehaviourBase
{
    protected Vector3 desiredVelocity;

    public override Vector3 CalculateForce()
    {
        CheckMouseInput();
        return CalculateSeekForce();
    }

    protected Vector3 CalculateSeekForce()
    {
        desiredVelocity = (target - transform.position).normalized;
        desiredVelocity = desiredVelocity * steeringAgent.maxSpeed;
        return (desiredVelocity - steeringAgent.velocity);
    }

    protected virtual void OnDrawGizmos()
    {
        if(steeringAgent != null)
        {
            DebugExtension.DebugArrow(transform.position, desiredVelocity, Color.red);
            DebugExtension.DebugArrow(transform.position, steeringAgent.velocity, Color.blue);
        }

        DebugExtension.DebugPoint(target, Color.magenta);
    }

}
