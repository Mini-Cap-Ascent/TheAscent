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
        // Trigger the death animation
        animator.SetTrigger("Die");

        // Wait for the death animation to finish
        // Adjust the time according to the length of your animation
        yield return new WaitForSeconds(5.0f);

        // After the animation is done, despawn the object
        if (Object.HasStateAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}
