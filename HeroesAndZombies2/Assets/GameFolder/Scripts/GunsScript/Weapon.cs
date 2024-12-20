using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public string weaponName;
    public int damage;
    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public int upgradeLevel = 0;
}

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Ranged }
    public WeaponType weaponType;
    public int damage;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public WeaponStats stats;
    private float nextFireTime;
    public int maxUpgradeLevel = 3; // Maximum upgrade level

    // Melee weapon properties
    public float attackDamage = 25f;      // Damage dealt
    public float attackRange = 2f;        // Attack range
    public LayerMask enemyLayer;          // Enemy layer to detect only enemies

    void Start()
    {
        if (weaponType == WeaponType.Ranged)
        {
            stats.currentAmmo = stats.maxAmmo;
            damage = stats.damage;
        }
    }

    public void Fire()
    {
        if (weaponType == WeaponType.Ranged)
        {
            if (Time.time > nextFireTime && stats.currentAmmo > 0)
            {
                nextFireTime = Time.time + stats.fireRate;
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                stats.currentAmmo--;
            }
        }
         if (weaponType == WeaponType.Melee)
        {
            // Detect enemies within a certain radius
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            // Apply damage to each enemy hit
            foreach (Collider enemy in hitEnemies)
            {
                Enemy enemyHealth = enemy.GetComponent<Enemy>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                    Debug.Log(enemy.name + " took damage: " + attackDamage);
                }
            }
        }
    }

    // Melee weapon attack function
    public void Attack()
    {
       if(Input.GetKeyDown(KeyCode.H)){
         Fire();
       }
       
    }

    public void UpgradeWeapon()
    {
        if (stats.upgradeLevel < maxUpgradeLevel) // Check maximum upgrade level
        {
            stats.upgradeLevel++;
            // Increase stats based on upgrade level
            stats.damage += 5;
            stats.fireRate -= 0.1f;
            // Additional upgrades can be applied here
        }
    }

    // Visualize the melee attack range in the editor
    private void OnDrawGizmosSelected()
    {
        if (weaponType == WeaponType.Melee)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}