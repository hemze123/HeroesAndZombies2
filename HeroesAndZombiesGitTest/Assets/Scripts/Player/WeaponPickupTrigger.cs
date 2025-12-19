using UnityEngine;

public class WeaponPickupTrigger : MonoBehaviour
{
    private Weapon weapon;
    
    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        if (weapon == null)
            weapon = GetComponentInParent<Weapon>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            WeaponSystem weaponSystem = other.GetComponent<WeaponSystem>();
            if (weaponSystem != null && weapon != null)
            {
                // Silah zaten elde değilse pickup et
                if (weaponSystem.currentWeapon != weapon)
                {
                    weaponSystem.TryPickupWeapon(weapon.gameObject);
                }
            }
        }
    }
}