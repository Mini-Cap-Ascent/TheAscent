using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnBob : MonoBehaviour
{
    public int damage = 10; // Amount of damage fire does
    public AudioClip fireSound; // Fire sound effect

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        { // Ensure there is an AudioSource component
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only players are affected
        {
            NetworkHealth healthComponent = other.GetComponent<NetworkHealth>();
            Animator animator = other.GetComponent<Animator>();
            if (healthComponent != null && animator != null)
            {
                healthComponent.TakeDamage(damage, gameObject);
                animator.SetTrigger("OnFire"); // Trigger fire animation

                // Play fire sound effect
                audioSource.PlayOneShot(fireSound);
            }
        }
    }
}
