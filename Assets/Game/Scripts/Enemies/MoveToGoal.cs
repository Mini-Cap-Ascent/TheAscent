using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.EventSystems;

[TaskName("MoveToGoal")]
[TaskCategory("Generic")]
[TaskDescription("Move to a goal using the NavMeshAgent")]
public class MoveToGoal : Action
{

    public float angularDampingTime = 5.0f;
    public float deadZone = 10.0f;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected AnimationListener animationListener;

    public bool useAgentStoppingDistance = true;
    public float actionStoppingDistance = 2.0f;


    public override void OnAwake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        animationListener = gameObject.GetComponent<AnimationListener>();
    }

    public override void OnStart()
    {
      
        animationListener.OnAnimatorMoveEvent += OnAnimatiorMove;
    }

    public override void OnEnd()
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatiorMove;
    }

    private void OnAnimatiorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    public override TaskStatus OnUpdate()
    {
       float stoppingDistance = agent.stoppingDistance;
        if (useAgentStoppingDistance==false)
        {
            stoppingDistance = actionStoppingDistance;
        }

        if ((transform.position - agent.destination).magnitude > stoppingDistance) {
        
            float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude * agent.speed;
            animator.SetFloat("Speed", speed);

            float angle = Vector3.Angle(transform.forward, agent.desiredVelocity);
            if (Mathf.Abs(angle) <= deadZone)
            {

                transform.LookAt(transform.position + agent.desiredVelocity);

            }
            else {
            
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(agent.desiredVelocity), angularDampingTime * Time.deltaTime);

            
            }
            return TaskStatus.Running;

        
        }
        animator.SetFloat("Speed", 0);
        return TaskStatus.Success;
    }
}
