using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDamageRef : NetworkBehaviour
{
    private Health healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void TakeDamageRPC(int amount, NetworkObject damageSource)
    {
        healthComponent?.TakeDamage(amount, damageSource.gameObject);

        // Optionally, if the damage source needs to be networked, you might need to adjust the Health component to accept NetworkObject or some form of network ID
    }
}
