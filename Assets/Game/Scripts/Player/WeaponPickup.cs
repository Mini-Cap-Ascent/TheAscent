using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string weaponName; // The name of the weapon to identify it

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                // Call the method to equip the weapon by its name
                weaponManager.EquipWeapon(weaponName);

                // Optionally, deactivate the weapon pickup if it should disappear after pickup
                gameObject.SetActive(false);
            }
        }
    }
}

