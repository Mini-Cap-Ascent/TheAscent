using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Patrol_FSM : Enemy_BaseState
{
    private Transform enemyTransform;
    private Enemy_Controller enemy;
    private AnimationListener animationListener;
    private GameObject[] Waypoints;
    public Transform player;
    public bool canSeePlayer = false;
    public float chaseDistance = 10f;
    public float health = 100f;
    public float criticalHealthThreshold = 95f;
    public float detectionRange = 20f;
    public float fieldOfViewAngle = 360f; 
    public float distanceToPlayer = 30f;
    private bool isDying = false;

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
        enemyTransform = owner.transform;


      
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
            enemyAnimator.SetFloat("Speed", 1f); // Assuming 'animator' is correctly referencing the enemy's Animator component
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

        CheckHealth();
        // Check for critical injury
        
        

    }

    private void DetectPlayer()
    {
        float eyeLevel = 1.8f; // The enemy's eye level above the ground.
        float forwardOffset = 8.0f; // Forward offset from the enemy's center to start detection.

        // Calculate the detection origin point with eye level and forward offset.
        Vector3 detectionOrigin = enemy.transform.position + Vector3.up * eyeLevel + enemy.transform.forward * forwardOffset;

        // Detect all colliders within the detection range from the detection origin.
        Collider[] detectedColliders = Physics.OverlapSphere(detectionOrigin, detectionRange, targetMask);

        canSeePlayer = false; // Reset the detection flag.
        foreach (Collider collider in detectedColliders)
        {
            if (collider.CompareTag("Player")) // Make sure the player has the "Player" tag.
            {
                // Perform a raycast to check for a clear line of sight to the collider.
                if (Physics.Raycast(detectionOrigin, (collider.transform.position - detectionOrigin).normalized, out RaycastHit hit, detectionRange, targetMask | obstacleMask))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.DrawLine(detectionOrigin, hit.point, Color.blue); // Visual debugging.
                        canSeePlayer = true;
                        return; // Player detected, exit the loop.
                    }
                }
            }
        }
        
        // Optional: Visual debugging to show the detection sphere and the forward direction.
        Debug.DrawRay(detectionOrigin, Vector3.up * 2, Color.yellow);
        Debug.DrawLine(detectionOrigin, detectionOrigin + enemy.transform.forward * detectionRange, Color.yellow);
    }

    void OnDrawGizmosSelected()
    {
        if (enemyTransform == null) return; // Check if the transform is set

        float eyeLevel = 1.8f;
        Gizmos.color = Color.yellow;
        Vector3 gizmoCenter = enemyTransform.position + Vector3.up * eyeLevel;
        Gizmos.DrawWireSphere(gizmoCenter, detectionRange);
    }
 

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
   


    }

    private void CheckHealth()
    {
        EnemyHealth healthComponent = enemy.GetComponent<EnemyHealth>();
        if (healthComponent != null)
        {
            if (healthComponent.currentHealth <= 0)
            {
               Die();// Transition to a death state
            }
            else if (healthComponent.currentHealth <= healthComponent.GetMaxHealth() * criticalHealthThreshold / 100)
            {
                fsm.ChangeState(fsm.DeathStateName); // Example state change on critical health
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {

            ApplyDamage(100f, collision.collider.gameObject);

            if (enemyAnimator != null)
            {


                enemyAnimator.SetTrigger("Hit");

            }

        }

    }

    public void ApplyDamage(float damage, GameObject source)
    {
        var networkHealth = enemy.GetComponent<EnemyHealth>();
        if (networkHealth != null)
        {
            networkHealth.TakeDamage((int)damage, source);
            enemyAnimator.SetTrigger("Hit");// Now passing the source along
        }
    }
    public void Die()
    {
        // Ensure this only triggers once if health reaches 0 multiple times before the GameObject is destroyed or disabled
        if (!isDying)
        {
            isDying = true; // Prevent multiple death triggers
            enemyAnimator.SetTrigger("Die");
            fsm.ChangeState(fsm.DeathStateName);
            // Additional cleanup or gameplay logic here
        }
    }

}
