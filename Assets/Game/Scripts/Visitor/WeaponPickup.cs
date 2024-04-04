using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, ICollectible
{
    public string weaponName; // The name of the weapon to identify it
    public GameObject weaponPrefab; // The actual weapon prefab to instantiate

    public void Accept(IPlayerVisitor visitor)
    {
        visitor.Visit(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IPlayerVisitor visitor = other.GetComponent<IPlayerVisitor>();
            if (visitor != null)
            {
                Accept(visitor);
            }

            gameObject.SetActive(false);
        }
    }
}

