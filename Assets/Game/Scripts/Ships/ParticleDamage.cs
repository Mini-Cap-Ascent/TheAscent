using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public float damage = 10f; // Damage per particle hit

    private ParticleSystem partSys;
    private ParticleSystem.CollisionModule collisionModule;

    void Start()
    {
        partSys = GetComponent<ParticleSystem>();

        if (partSys == null)
        {
            Debug.LogError("Missing ParticleSystem component on this GameObject.");
            return;
        }

        collisionModule = partSys.collision;
        collisionModule.enabled = true;
        collisionModule.sendCollisionMessages = true; // This must be enabled to send OnParticleCollision messages
    }

    void OnParticleCollision(GameObject other)
    {
        ShipHealth targetHealth = other.GetComponent<ShipHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
    }
}
