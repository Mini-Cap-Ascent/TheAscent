using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
   public float maxHealth = 100f;
    private Animator animator;
    // Use SerializeField to make it visible in the Inspector, but keep it private to protect from unwanted access
    [SerializeField]
    public float currentHealth;
    [SerializeField]
    public float knockbackForce = 10.0f;
    public bool isDying = false;

    void Awake()
    {
        currentHealth = maxHealth;
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
        Enemy_FSM fsm = GetComponent<Enemy_FSM>();
        fsm.ChangeState(fsm.DeathStateName);

        yield return new WaitForSeconds(5.0f); // Adjust time for your death animation
        if (!isDying) {
        
            isDying = true;
            fsm.ChangeState(fsm.DeathStateName);

        }
       
        
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Public method to safely modify health if needed from other scripts (e.g., for debugging or specific game logic)
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Public getters for current and max health if needed by other components or UI
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
