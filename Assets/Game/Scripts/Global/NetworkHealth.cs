using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [Networked]
    public int currentHealth { get; set; }

    private Animator animator;
    public int maxHealth = 100;
    public float knockbackForce = 10.0f;

    public override void Spawned()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount, GameObject damageSource)
    {
        currentHealth -= amount;

        // Handle the damage source, e.g., for a knockback effect or identifying the attacker.
        if (damageSource != null)
        {
            // For example, apply knockback from the damage source's position
            Vector3 knockbackDirection = (transform.position - damageSource.transform.position).normalized;
           GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // Or record the attacker for scoring purposes
            // LastAttacker = damageSource;
        }

        // Trigger a hit animation or other immediate reaction.
        animator.SetTrigger("Hit");

        // If health drops to zero, begin the death coroutine.
        if (currentHealth <= 0)
        {
            StartCoroutine(Die()); // If you need the damage source on death, pass it to the Die coroutine.
        }
    }

    private IEnumerator Die()
    {
        // Play the death animation.
        animator.SetTrigger("Die");

        // Wait for the death animation to complete before continuing.
        yield return new WaitForSeconds(5.0f);

        // Handle the player's death, like disabling the GameObject, respawning, etc.
        if (Object.HasStateAuthority)
        {
            // Only the network authority can despawn the object to ensure proper network synchronization.
            Runner.Despawn(Object);
        }
        else
        {
            // Disable the GameObject if not the authority, or handle as appropriate for your game design.
            gameObject.SetActive(false);
        }
    }
}
