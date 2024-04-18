using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    public float health = 100f;
    public float damagePerSecond = 5f; // Damage each ship inflicts on the other per second
    public bool isDamaging = false; // Controls whether the ship is currently taking damage

    private Rigidbody rb; // Reference to the Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Ensure gravity is initially off
            rb.isKinematic = true; // Prevent the Rigidbody from being affected by physics forces
        }
    }

    void Update()
    {
        if (isDamaging)
        {
            TakeDamage(damagePerSecond * Time.deltaTime); // Apply damage over time
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDamaging)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }

        } else
        {

            Debug.Log("Ship is not taking damage.");
        }
    }

    private void Die()
    {
        Debug.Log("Ship Destroyed!");
        isDamaging = false; // Stop taking continuous damage

        // Enable gravity and allow physics to take over
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Optionally, trigger explosion effects or other notifications here
        Destroy(gameObject, 30f); // Destroys the ship object after 5 seconds
    }

    public void StartDamaging()
    {
        isDamaging = true;
    }
}
