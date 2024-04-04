using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour, IPlayerVisitor
{
    private NetworkCharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }

    public void Visit(WeaponPickup powerUp)
    {
        // Apply the powerup effect to the player
        Debug.Log($"Player picked up {powerUp.weaponName}");
        // Instantiate the weapon prefab, equip it, etc.
    }


}
