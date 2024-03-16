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
    public float health = 100f;
    public float criticalHealthThreshold = 95f;
    public float detectionRange = 20f;
    public float fieldOfViewAngle = 110f; 
    public float distanceToPlayer = 30f;

    // Angle for field of view
    public LayerMask targetMask; // Layer on which the player is located
    public LayerMask obstacleMask;


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
        bool playerInSight = false;
        Vector3 directionToPlayer = (player.position - enemy.transform.position).normalized;
        if (Vector3.Angle(enemy.transform.forward, directionToPlayer) < fieldOfViewAngle / 2)
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.position);
            if (!Physics.Raycast(enemy.transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
            {
                playerInSight = true;
            }
        }

        // Use playerInSight variable to decide whether to chase the player or not
        if (playerInSight && distanceToPlayer <= detectionRange)
        {
            canSeePlayer = true;
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
        if (IsCriticallyInjured()) // This method needs to be implemented in Enemy_Controller
        {
            fsm.ChangeState(fsm.DeathStateName); // Example, change to a hurt state
        }

    }
    public bool IsCriticallyInjured()
    {
        return health <= criticalHealthThreshold; // Define what you consider "critically injured"
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die(); // Handle death, possibly change state
        }
        else
        {
            // Optionally trigger a reaction, like a hurt animation
            ; // Assuming fsm is accessible and this is your hurt state name
        }
    }



    public void Die()
    {
        fsm.ChangeState(fsm.DeathStateName);

    }
}
