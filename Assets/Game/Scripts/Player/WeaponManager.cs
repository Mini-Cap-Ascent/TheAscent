using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class WeaponManager : MonoBehaviour
{


    public enum WeaponType
    {
        Pistol,
        Rifle,
        Shotgun,
        Sword,
        // Add as many types as you have in your game
    }
    public static WeaponManager Instance { get; private set; }

    [Serializable]
    public struct WeaponAttachment
    {
        public WeaponType type;
        public Transform attachmentPoint;
    }
    [SerializeField] public WeaponAttachment[] weaponAttachments; //Assign this in the inspector
    private Dictionary<WeaponType, Transform> _attachmentPoints;
    private Dictionary<WeaponType, GameObject> _currentWeapons;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeAttachmentPoints();
    }

    private void InitializeAttachmentPoints()
    {
        _attachmentPoints = new Dictionary<WeaponType, Transform>();
        _currentWeapons = new Dictionary<WeaponType, GameObject>();
        foreach (var attachment in weaponAttachments)
        {
            _attachmentPoints.Add(attachment.type, attachment.attachmentPoint);
        }
    }

    public void PickupWeapon(WeaponType weaponType, GameObject weaponPrefab)
    {
        // If there is already a weapon equipped of this type, we remove it
        if (_currentWeapons.TryGetValue(weaponType, out var existingWeapon))
        {
            Destroy(existingWeapon);
            _currentWeapons.Remove(weaponType);
        }

        if (_attachmentPoints.TryGetValue(weaponType, out var attachmentPoint))
        {
            // Instantiate the new weapon prefab at the correct attachment point
            GameObject newWeapon = Instantiate(weaponPrefab, attachmentPoint.position, attachmentPoint.rotation);
            newWeapon.transform.SetParent(attachmentPoint, worldPositionStays: false);
            newWeapon.transform.localPosition = Vector3.zero;
            
            newWeapon.SetActive(true);
            _currentWeapons.Add(weaponType, newWeapon);

            // Additional setup for the new weapon can go here
        }
        else
        {
            Debug.LogError($"Attachment point for weapon type {weaponType} not found!");
        }
    }

    // Call this method to switch weapons
    public void EquipWeapon(WeaponType weaponType)
    {
        foreach (var weapon in _currentWeapons)
        {
            weapon.Value.SetActive(weapon.Key == weaponType);
        }
    }

}