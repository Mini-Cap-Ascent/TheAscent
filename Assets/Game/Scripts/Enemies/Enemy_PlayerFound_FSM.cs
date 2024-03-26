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
    public GameObject projectilePrefab;
    public float launchForce = 10f;
    private FireBall_Attack fireBallAttack;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        enemy = owner.GetComponent<Enemy_Controller>();
        animationListener = owner.GetComponent<AnimationListener>();
        player = owner.GetComponent<Transform>();
        fireBallAttack = owner.GetComponent<FireBall_Attack>();
  
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChasePlayer();
        enemyAnimator.SetFloat("Speed", 1);

        if (Vector3.Distance(owner.transform.position, player.position) <= attackDistance)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                enemyAnimator.SetBool("IsPlayerNear", true);
                ShootProjectileAtPlayer();
            }
        }
        else
        {
           enemyAnimator.SetBool("IsPlayerNear", false);
            
        }
    

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.agent.isStopped = true;
        enemyAnimator.SetFloat("Speed", 0);
    }



    private void ChasePlayer()
    {
        if (enemy.agent.enabled && player != null)
        {
            enemy.agent.SetDestination(player.position);
        }
    }
    void ShootProjectileAtPlayer()
    {
        if (fireBallAttack != null)
        {
            fireBallAttack.FireProjectile();
        }
    }

}
