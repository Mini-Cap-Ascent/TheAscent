using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [Networked] public int currentHealth { get; set; }
    
    private Animator animator;

    public int maxHealth = 100;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Spawned()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, GameObject damageSource)
    {
        if (Object.HasStateAuthority)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Optional: Play death animation or sound before destruction
        animator.SetTrigger("Die");
        // Ensure this is called by the authority controlling the object
        if (Object.HasStateAuthority)
        {
            // Remove the object from all clients
            Runner.Despawn(Object);
        }
    }
}
