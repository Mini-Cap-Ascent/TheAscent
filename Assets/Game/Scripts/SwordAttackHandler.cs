using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackHandler : MonoBehaviour
{
    public float attackRange = 1.5f; // The range of the attack
    public LayerMask enemyLayer; // Layer mask to detect enemies
    public int attackDamage = 20; // Damage dealt by the sword
    public GameObject GameObject;
    public void Hit()
    {
        // Implement attack logic here
        Debug.Log("Attack event received and handled.");

        // Check for enemies within attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Assuming the enemy has a component 'Enemy_Patrol_FSM' which handles taking damage
            Enemy_Patrol_FSM enemyComponent = enemy.GetComponent<Enemy_Patrol_FSM>();
            if (enemyComponent != null)
            {
                // Apply damage to the enemy
                enemyComponent.ApplyDamage(attackDamage, hitEnemies[0].gameObject);
            }
        }
    }

    // This method is optional and used only for debugging purposes to visualize the attack range
    void OnDrawGizmosSelected()
    {
        if (transform == null)
            return;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
