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
        DetectPlayer();

        // Movement and animation logic as before
        if (enemy.agent.remainingDistance > 0)
        {
            enemyAnimator.SetFloat("Speed", 0.75f); // Assuming 'animator' is correctly referencing the enemy's Animator component
        }
        else
        {
            enemyAnimator.SetFloat("Speed", 0);
        }

        // Transition logic based on player detection
        if (canSeePlayer)
        {
            // Chase the player
           
            fsm.ChangeState(fsm.EnemyFoundStateName);
        }
        else if (enemy.isAtDestination())
        {
            // Continue patrolling
            enemy.MoveToNextWayPoint();
        }

        // Check for critical injury
        if (IsCriticallyInjured())
        {
            fsm.ChangeState(fsm.DeathStateName); // Change to appropriate state
        }

    }

    private void DetectPlayer()
    {
        // Assume we have a reference to the player's CharacterController component
        CharacterController playerCharacterController = player.GetComponent<CharacterController>();

        float characterControllerHeight = playerCharacterController.height;
        float characterControllerRadius = playerCharacterController.radius;

        // Adjust this value to the enemy's eye level
        float eyeLevel = 1.8f;
        Vector3 detectionOrigin = enemy.transform.position + Vector3.up * eyeLevel;

        // Offset in front of the enemy; adjust the value as needed
        float frontOffset = 8.0f; // 1 meter in front of the enemy, adjust this value as necessary
        detectionOrigin += enemy.transform.forward * frontOffset; // Apply the offset here

        Vector3 directionToPlayer = (player.position - detectionOrigin).normalized;
        float distanceToPlayer = Vector3.Distance(detectionOrigin, player.position);

        // Debugging: draw the detection ray and capsule
        Debug.DrawRay(detectionOrigin, directionToPlayer * detectionRange, Color.green);

        Vector3 capsuleBottom = detectionOrigin;
        Vector3 capsuleTop = capsuleBottom + Vector3.up * (characterControllerHeight - characterControllerRadius * 2);

        // Debugging: draw the capsule
        Debug.DrawLine(capsuleBottom, capsuleTop, Color.red);
        Debug.Log("We Made it");

        RaycastHit hit;
        if (Physics.CapsuleCast(capsuleBottom, capsuleTop, characterControllerRadius, directionToPlayer, out hit, detectionRange, targetMask))
        {
            Debug.Log("Collision detected!");
            if (hit.collider.GetComponent<CharacterController>() != null)
            {
                canSeePlayer = true;
                return;
            }
        }

        canSeePlayer = false;
    }
    public bool IsCriticallyInjured()
    {
        return health <= criticalHealthThreshold; // Define what you consider "critically injured"
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
   


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
