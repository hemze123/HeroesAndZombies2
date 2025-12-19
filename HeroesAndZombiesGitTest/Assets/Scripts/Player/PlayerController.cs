using UnityEngine;

[RequireComponent(typeof(WeaponSystem))]
public class PlayerController : MonoBehaviour
{
    private WeaponSystem weaponSystem;

    private void Awake()
    {
        weaponSystem = GetComponent<WeaponSystem>();
    }

 private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Weapon"))
    {
        // Silahın rigidbody'si hele aktif mi yoxla
        Rigidbody weaponRB = collision.gameObject.GetComponent<Rigidbody>();
        if (weaponRB != null && weaponRB.detectCollisions == true)
        {
            weaponSystem.TryPickupWeapon(collision.gameObject);
        }
    }
}

    public void OnPointerDown() => weaponSystem.StartFiring();
    public void OnPointerUp() => weaponSystem.StopFiring();
}
