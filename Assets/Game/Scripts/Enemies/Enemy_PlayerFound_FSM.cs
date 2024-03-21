using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PlayerFound_FSM : Enemy_BaseState
{
    private Enemy_Controller enemy;
    private AnimationListener animationListener;
    public Transform player;
    public bool canSeePlayer = false;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
  
    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        enemy = owner.GetComponent<Enemy_Controller>();
        animationListener = owner.GetComponent<AnimationListener>();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.agent.speed = 3.5f;
        enemy.agent.stoppingDistance = attackDistance;
        enemy.agent.autoBraking = true;
        enemy.agent.acceleration = 8f;
        enemy.agent.angularSpeed = 500f;
        enemy.agent.SetDestination(player.position);
        enemy.agent.isStopped = false;
        enemyAnimator.SetFloat("Speed", 1);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(owner.transform.position, player.position) <= attackDistance)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                enemyAnimator.SetBool("IsPlayerNear", true);
            }
        }
        else
        {
           enemyAnimator.SetBool("IsPlayerNear", false);
            ChasePlayer();
        }
    

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.agent.isStopped = true;
        enemyAnimator.SetFloat("Speed", 0);
    }



    private void ChasePlayer()
    {
        if (enemy.agent.enabled)
        {
            enemy.agent.SetDestination(player.position);
        }
    }

}
