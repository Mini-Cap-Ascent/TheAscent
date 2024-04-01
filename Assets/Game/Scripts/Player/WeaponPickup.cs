using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : NetworkBehaviour
{
    public NetworkPrefabRef weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // Only the server can instantiate and assign weapons
        if (!Runner.IsServer) return;

        if (other.gameObject.CompareTag("Player"))
        {
            var weaponManager = other.GetComponent<WeaponManager>();

            if (weaponManager != null && weaponPrefab.IsValid)
            {
                // Spawn the weapon and get the NetworkObject
                NetworkObject weaponNetworkObject = Runner.Spawn(weaponPrefab, transform.position, transform.rotation);

                // Call the RPC method to equip the weapon on the server
                weaponManager.RPC_ServerEquipWeapon(weaponNetworkObject);
            }
        }
    }
}

