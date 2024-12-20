using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public Transform weaponHolder; // Silahın oyuncuya ekleneceği yer
    private Transform currentWeaponTransform;
    private Weapon currentWeapon;

    public void PickupWeapon(Weapon newWeapon, Transform weaponTransform)
    {
        // Eğer oyuncuda silah varsa, mevcut silahı yere bırak
        if (currentWeaponTransform != null)
        {
            currentWeaponTransform.gameObject.SetActive(true);
            currentWeaponTransform.parent = null;
        }

        // Yeni silahı kuşan
        currentWeapon = newWeapon;
        currentWeaponTransform = weaponTransform;
        currentWeaponTransform.SetParent(weaponHolder);
        currentWeaponTransform.localPosition = Vector3.zero;
        currentWeaponTransform.localRotation = Quaternion.identity;
        currentWeaponTransform.gameObject.SetActive(false);
    }

    public void FireWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }
}
