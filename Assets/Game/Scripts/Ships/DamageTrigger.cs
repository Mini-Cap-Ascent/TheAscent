using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public ShipHealth ship1;
    public ShipHealth ship2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            ship1.StartDamaging();
            ship2.StartDamaging();
        }
    }
}
