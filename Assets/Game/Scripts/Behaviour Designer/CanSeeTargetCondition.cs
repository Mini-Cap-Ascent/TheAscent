using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskName("Can See Target")]
[TaskCategory("Generic")]
[TaskDescription("Returns Success if it can see the target")]
public class CanSeeTargetCondition : Conditional
{
    public SharedGameObject target;
    public float distance = 10.0f;
    public float angle = 35.0f;

    private Color handleColor = Color.green;

    public override TaskStatus OnUpdate()
    {
        if (target.Value != null)
        {
            Vector3 dirToTarget = target.Value.transform.position - transform.position;
            
            Vector3 dirNom = dirToTarget.normalized;
            float magDistance = dirToTarget.magnitude;

            float viewAngle = Vector3.Angle(transform.forward, dirNom);
            if (Mathf.Abs(viewAngle) < angle && magDistance <= distance)
            {
                handleColor = Color.red;
                SharedVector3 destination = Owner.GetVariable("Destination") as SharedVector3;
                if (destination != null)
                {
                    destination.Value = target.Value.transform.position;
                }
                return TaskStatus.Success;
            }
        }

        handleColor = Color.green;
        return TaskStatus.Failure;
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (transform != null)
        {
            float halfAngle = angle * 0.5f;
            Vector3 targetDirection = Quaternion.AngleAxis(-halfAngle, transform.up) * transform.forward;
            UnityEditor.Handles.color = handleColor;
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, targetDirection, angle, distance);
        }
#endif
    }
}
