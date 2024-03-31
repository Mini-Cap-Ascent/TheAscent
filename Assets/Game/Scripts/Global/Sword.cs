using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage = 25;
    public GameObject gameObject;// Damage dealt by the sword

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        // Check if the collider belongs to an enemy
        if (collision.gameObject.CompareTag("Enemy")) // Make sure your enemy GameObjects have the "Enemy" tag
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Apply damage to the enemy
                enemyHealth.TakeDamage(damage, collision.collider.gameObject);
            }
        }
    }
}
