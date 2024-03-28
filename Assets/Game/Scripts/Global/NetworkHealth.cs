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

    public override void Spawned()
    {
        currentHealth = maxHealth;
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    // This method is expected to be called manually when taking damage.
    public void TakeDamage(int amount, GameObject damageSource)
    {
        if (Object.HasStateAuthority)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    // This method is a callback that Fusion expects to call when 'currentHealth' changes.
   private void OnCurrentHealthChanged(int newHealth)
    {
        Debug.Log($"Health changed to {newHealth}");
        // You can trigger animations or other visual updates here.
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(5.0f); // Adjust time for your death animation

        if (Object.HasStateAuthority)
        {
            // Check if the DeathMenu script is on the same GameObject, otherwise find it.
            DeathMenu deathMenu = FindObjectOfType<DeathMenu>(); // Find it in the scene; consider caching it for efficiency.
            if (deathMenu != null)
            {
                EventBus.Instance.Publish(new DeathEvent());
            }

            Runner.Despawn(Object);
        }
    }

}
