using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Attack : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign your projectile Prefab in the inspector
    private Transform spawnPoint; // Assign the spawn point in the inspector

    void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    public void FireProjectile()
    {
        Transform targetPlayer = FindClosestPlayer();
        if (projectilePrefab != null && targetPlayer != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = (targetPlayer.position - spawnPoint.position).normalized;
                rb.velocity = direction * rb.mass; // Adjust speed factor as needed
                projectile.transform.rotation = Quaternion.LookRotation(direction);
            }

            Destroy(projectile, 5f); // Adjust lifetime as needed
        }
    }

    Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, position);
            if (distance < closestDistance)
            {
                closestPlayer = player.transform;
                closestDistance = distance;
            }
        }

        return closestPlayer;
    }
}

