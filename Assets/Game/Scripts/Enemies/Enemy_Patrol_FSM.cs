using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Patrol_FSM : Enemy_BaseState
{

    private Enemy_Controller enemy;
    private AnimationListener animationListener;
    private GameObject[] Waypoints;
    public Transform player;
    public bool canSeePlayer = false;
    public float chaseDistance = 10f;
   

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        enemy = owner.GetComponent<Enemy_Controller>();
        Waypoints = enemy.Waypoints.ToArray();
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        animationListener = owner.GetComponent<AnimationListener>();


      
    }

    private void OnAnimatorMove()
    {

       


    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

      

        if (enemy.agent.remainingDistance>0)
        {

            enemyAnimator.SetFloat("Speed", 1);


        }
        else
        {

            enemyAnimator.SetFloat("Speed", 0);


        }

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float randomIndex = Random.Range(0.5f, 1);
        if (enemy.agent.remainingDistance > 0)
        {

            enemyAnimator.SetFloat("Speed", 0.75f);


        }
        else
        {

            enemyAnimator.SetFloat("Speed", 0);


        }

        if (Vector3.Distance(enemy.transform.position, player.position) <= chaseDistance)
        {
            canSeePlayer = true; // Or implement your line of sight check here
        }
        else
        {
            canSeePlayer = false;
        }

        if (canSeePlayer)
        {
            // Chase the player
           enemy.agent.SetDestination(player.position);
            enemyAnimator.SetFloat("Speed", 1);
            fsm.ChangeState(fsm.EnemyFoundStateName);
        }
        else
        {
            // Continue patrolling
            if (enemy.isAtDestination())
            {
                enemy.MoveToNextWayPoint();
            }
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
    }
}
