using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target; // The target the turret should aim at
    public float fireRate = 1f; // The rate of fire
    public float turnSpeed = 5f; // Speed at which the turret turns to face the target
    public ParticleSystem firingEffect1; // The particle system for the first barrel's firing effect
    public ParticleSystem firingEffect2; // The particle system for the second barrel's firing effect

    private float fireTimer;

    void Update()
    {
        // Aim the turret towards the target every frame
        AimAtTarget();

        // Increment the fireTimer with the time that has passed since last frame
        fireTimer += Time.deltaTime;

        // Check if it's time to fire again based on the fireRate
        if (fireTimer >= 1f / fireRate)
        {
            // Reset the fireTimer
            fireTimer = 0f;

            // Trigger the firing effects for both barrels
            Fire();
        }
    }

    void AimAtTarget()
    {
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            // Ensure the turret's up direction remains unchanged to prevent rolling
            Quaternion correctedTargetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctedTargetRotation, Time.deltaTime * turnSpeed);
        }
    }

    void Fire()
    {
        // Play the firing effect for the first barrel
        if (firingEffect1 != null)
        {
            firingEffect1.Play();
        }

        // Play the firing effect for the second barrel
        if (firingEffect2 != null)
        {
            firingEffect2.Play();
        }

        // You can also add sound effects or other firing logic here
    }
}