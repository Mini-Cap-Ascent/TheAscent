using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController2 : MonoBehaviour
{
    public Transform target; // The target to aim at
    public ParticleSystem fxToShoot; // The FX to shoot
    public float fireRate = 1f; // How often to shoot
    public float turnSpeed = 5f; // The speed at which to turn towards the target

    private float fireTimer = 0f;

    private void Update()
    {
        AimAtTarget();

        // Timer to handle the rate of fire
        fireTimer += Time.deltaTime;
        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            ShootFX();
        }
    }

    private void AimAtTarget()
    {
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            // Rotate the aim point towards the target
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    private void ShootFX()
    {
        if (fxToShoot != null)
        {
            fxToShoot.Play(); // Play the FX
        }
        // Add additional code here if you need to handle ammo, sound effects, etc.
    }
}
