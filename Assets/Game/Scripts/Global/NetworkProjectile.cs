using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkProjectile : NetworkBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 50; // Damage the projectile can inflict
    private NetworkObject networkObject;
    // Update is called once per frame

    private void Awake()
    {
        // Initialize network-related components and other necessary references here
        networkObject = GetComponent<NetworkObject>();
    }
    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has a Health component
        NetworkHealth healthComponent = collision.gameObject.GetComponent<NetworkHealth>();

        if (healthComponent != null)
        {
            // Check if this projectile has the authority to apply damage
            if (networkObject.HasStateAuthority)
            {
                // Directly apply damage to the health component
                healthComponent.TakeDamage(damage, collision.collider.gameObject);

                // Optionally destroy the projectile on collision
                Destroy(gameObject);
            }
        }
    }
}
    // Start is called before the first frame update

