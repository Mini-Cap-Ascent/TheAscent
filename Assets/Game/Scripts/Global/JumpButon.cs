using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButon : MonoBehaviour
{
    public float boostMultiplier = 2f; // The amount to multiply the jump force
    public float boostDuration = 5f; // The duration of the jump boost


    // This is the larger trigger collider for detecting player proximity
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure to tag your player GameObject with the "Player" tag
        {
            // Show UI prompt or activate some visual cue to let the player know they can interact.
            // You could also store a reference to the player here if needed.
        }
    }

    // Detects when the player exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide UI prompt or deactivate the visual cue.
        }
    }
}
