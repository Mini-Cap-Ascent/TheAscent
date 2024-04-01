using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef weaponPrefab; // Assign this in the inspector
    public Transform weaponAttachmentPoint;
    public Transform weaponSlot;

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ServerEquipWeapon(NetworkObject weaponPickupNetworkObject)
    {
        if (!Runner.IsServer)
        {
            Debug.LogError("RPC_ServerEquipWeapon is called on the client. This should only run on the server.");
            return;
        }

        // Despawn any currently equipped weapon
        DespawnCurrentWeapon();

        // Spawn the new weapon at the attachment point
        NetworkObject weaponObject = Runner.Spawn(
            weaponPrefab,
            weaponAttachmentPoint.position,
            weaponAttachmentPoint.rotation
        );

        // Parent the weapon to the attachment point and reset local position/rotation
        weaponObject.transform.SetParent(weaponAttachmentPoint, worldPositionStays: false);
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;
    }

    private void DespawnCurrentWeapon()
    {
        if (weaponAttachmentPoint.childCount > 0)
        {
            NetworkObject currentWeapon = weaponAttachmentPoint.GetChild(0).GetComponent<NetworkObject>();
            if (currentWeapon != null)
            {
                Runner.Despawn(currentWeapon);
            }
        }
    }

    private void EquipWeaponToAttachmentPoint(NetworkObject weaponNetworkObject)
    {
        // Ensure the weapon's NetworkObject isn't null
        if (weaponNetworkObject == null)
        {
            Debug.LogError("Weapon NetworkObject is null. Cannot equip to attachment point.");
            return;
        }

        // Ensure there's an attachment point set
        if (weaponAttachmentPoint == null)
        {
            Debug.LogError("Weapon attachment point not set. Cannot equip weapon.");
            return;
        }

        // Parent the weapon to the attachment point
        weaponNetworkObject.transform.SetParent(weaponAttachmentPoint, false);

        // Optionally, set the local position and rotation to zero if you want the weapon to be positioned exactly at the attachment point
        weaponNetworkObject.transform.localPosition = Vector3.zero;
        weaponNetworkObject.transform.localRotation = Quaternion.identity;

        // If the weapon has any additional setup or adjustments needed, do that here
        // For example, if the weapon needs to be activated or if you need to set the owner
    }
    public void EquipWeapon(NetworkPrefabRef weaponPrefab, NetworkObject pickerUpper)
    {

        if (!Runner.IsServer)
        {
            Debug.LogError("EquipWeapon called on client, should only be called on server.");
            return;
        }

        if (!weaponPrefab.IsValid)
        {
            Debug.LogError("The weaponPrefab is not valid. Make sure it is registered with the NetworkPrefab configuration.");
            return;
        }

        if (HasWeapon())
        {
            NetworkObject currentWeaponNetworkObject = weaponSlot.GetChild(0).GetComponent<NetworkObject>();
            if (currentWeaponNetworkObject != null)
            {
                Runner.Despawn(currentWeaponNetworkObject);
                Debug.Log("Despawning current weapon.");
            }
        }

        // Spawn the new weapon
        NetworkObject weaponNetworkObject = Runner.Spawn(
            weaponPrefab,
            weaponSlot.position,
            weaponSlot.rotation,
            pickerUpper.InputAuthority,
            (runner, obj) =>
            {
                obj.transform.SetParent(weaponSlot, false);
                Debug.Log("New weapon spawned and parented to: " + weaponSlot.name);
            }
        );
    }
    
    public bool HasWeapon()
    {
        return weaponSlot.childCount > 0;
    }
}
