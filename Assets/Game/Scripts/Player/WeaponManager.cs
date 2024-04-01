using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class WeaponManager : MonoBehaviour
{
    public GameObject sword;
    public GameObject rifle;
    public GameObject pistol;

    private void Awake()
    {
        // Initially set the sword to be active if the player starts with it.
        EquipWeapon("Sword");
    }

    public void EquipWeapon(string weaponName)
    {
        if (sword != null) sword.SetActive(false);
        if (rifle != null) rifle.SetActive(false);
        if (pistol != null) pistol.SetActive(false);

        // Activate the selected weapon based on the weaponName
        switch (weaponName)
        {
            case "Sword":
                if (sword != null) sword.SetActive(true);
                break;
            case "Rifle":
                if (rifle != null) rifle.SetActive(true);
                break;
            case "Pistol":
                if (pistol != null) pistol.SetActive(true);
                break;
            default:
                Debug.LogWarning("Weapon not found: " + weaponName);
                break;
        }
    }
}