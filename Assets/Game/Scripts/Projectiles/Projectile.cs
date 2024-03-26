using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 50; // Damage the projectile can inflict

    // Update is called once per frame
    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has a Health component
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            // Apply damage to the object
            health.TakeDamage(damage);
        }

        // Optionally destroy the projectile on collision
        Destroy(gameObject);
    }
}
