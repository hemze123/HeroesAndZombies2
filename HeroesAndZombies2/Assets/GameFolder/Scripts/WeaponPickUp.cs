using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
     public Weapon weaponData; // Weapon ScriptableObject referansı

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponManager weaponManager = other.GetComponent<PlayerWeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.PickupWeapon(weaponData, this.transform);
            }
        }
    }
}
