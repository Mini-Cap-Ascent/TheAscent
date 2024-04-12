using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[TaskName("Move To Goal")]
[TaskCategory("Generic")]
[TaskDescription("Moves a NPC Navmesh Agent to a destination. Will run until it arrives")]
public class MoveToGoalAction : Action
{
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected AnimationListener animationListener;

    public bool useAgentStoppingDistance = true;
    public float actionStoppingDistance = 2.0f;

    public override void OnAwake()
    {
        animationListener = gameObject.GetComponent<AnimationListener>();
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;
    }

    private void OnAnimatorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    public override TaskStatus OnUpdate()
    {
        float stoppingDistance = agent.stoppingDistance;
        if (useAgentStoppingDistance == false)
        {
            stoppingDistance = actionStoppingDistance;
        }

        if ((transform.position - agent.destination).magnitude > stoppingDistance)
        {
            float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude * agent.speed;
            animator.SetFloat("Speed", speed);

            float angle = Vector3.Angle(transform.forward, agent.desiredVelocity);
            if (Mathf.Abs(angle) <= deadZone)
            {
                transform.LookAt(transform.position + agent.desiredVelocity);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                     Quaternion.LookRotation(agent.desiredVelocity),
                                                     Time.deltaTime * angularDampeningTime);
            }

            return TaskStatus.Running;
        }

        animator.SetFloat("Speed", 0.0f);
        return TaskStatus.Success;
    }

    public override void OnEnd()
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
    }
}
