using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string weaponName; // The name of the weapon to identify it
    public GameObject weaponPrefab; // The actual weapon prefab to instantiate

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player GameObject has the tag "Player"
        {
            EventBus.Instance.Publish(new WeaponPickupEvent(weaponName, weaponPrefab));

            // Optionally, deactivate the weapon spawn point if it should disappear after pickup
            gameObject.SetActive(false);
        }
    }
}

