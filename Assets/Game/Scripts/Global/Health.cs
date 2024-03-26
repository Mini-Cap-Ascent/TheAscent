using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;

    // UnityEvents for external systems (e.g., UI or game logic) to listen to health changes
    public UnityEvent<int, int> onHealthChanged; // Current health, max health
    public UnityEvent onDied;
    public UnityEvent<int> onTakeDamage;
    // Optional: Differentiate between damage sources
    public UnityEvent<GameObject> onDamagedBy; // The object causing the damage

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, GameObject damageSource = null)
    {
        currentHealth -= amount;
        onTakeDamage.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            onDied.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        // Optionally deactivate the GameObject instead of destroying it
        // gameObject.SetActive(false);

        onDied?.Invoke();
        Destroy(gameObject);
    }

    // Expose health info for other components or UI
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
