using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [Tooltip("Silah adı")]
    public string weaponName;

    [Tooltip("Temel hasar miktarı")]
    public int baseDamage;

    [Tooltip("Temel ateş hızı (saniye cinsinden)")]
    public float baseFireRate;

    [Tooltip("Maksimum mermi sayısı (sadece ranged silahlar için)")]
    public int maxAmmo;

    [Tooltip("Mevcut mermi sayısı (sadece ranged silahlar için)")]
    public int currentAmmo;

    [Tooltip("Yükseltme seviyesi")]
    public int upgradeLevel = 0;

    [Tooltip("Dayanıklılık (sadece melee silahlar için)")]
    public int durability = 100;

    [Tooltip("Yükseltme başına hasar çarpanı")]
    public float upgradeDamageMultiplier = 1.2f;

    [Tooltip("Yükseltme başına ateş hızı azaltma çarpanı")]
    public float upgradeFireRateMultiplier = 0.9f;

    [Tooltip("Yükseltme başına dayanıklılık artışı (sadece melee için)")]
    public int upgradeDurabilityBonus = 20;

    [Tooltip("Bıçak gibi dayanıklılığı sonsuz olan silahlar için")]
    public bool infiniteDurability = false;
}

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Ranged }

    [Tooltip("Silah türü (Melee veya Ranged)")]
    public WeaponType weaponType;

    [Tooltip("Mermi prefabı (sadece ranged silahlar için)")]
    public GameObject bulletPrefab;

    [Tooltip("Ateşleme noktası (sadece ranged silahlar için)")]
    public Transform firePoint;

    [Tooltip("Silah istatistikleri")]
    public WeaponStats stats;

    private float nextFireTime;
    public int maxUpgradeLevel = 5;

    void Start()
    {
        InitializeWeapon();
    }

    private void InitializeWeapon()
    {
        if (weaponType == WeaponType.Ranged)
        {
            stats.currentAmmo = stats.maxAmmo;
        }
    }

    public void Fire()
    {
        if (weaponType == WeaponType.Ranged)
        {
            HandleRangedFire();
        }
        else if (weaponType == WeaponType.Melee)
        {
            HandleMeleeAttack();
        }
    }

    private void HandleRangedFire()
    {
        if (Time.time > nextFireTime && stats.currentAmmo > 0)
        {
            nextFireTime = Time.time + stats.baseFireRate / Mathf.Pow(stats.upgradeFireRateMultiplier, stats.upgradeLevel);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            stats.currentAmmo--;

            Debug.Log($"Fired! Ammo left: {stats.currentAmmo}");
        }
        else if (stats.currentAmmo <= 0)
        {
            Debug.Log("No ammo left!");
        }
    }

    private void HandleMeleeAttack()
    {
        if (!stats.infiniteDurability) // Eğer dayanıklılık sonsuz değilse
        {
            stats.durability--;
            Debug.Log($"Melee attack! Remaining durability: {stats.durability}");

            if (stats.durability <= 0)
            {
                Debug.Log($"{stats.weaponName} is broken!");
                Destroy(gameObject); // Silah yok olur
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponType == WeaponType.Melee && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                int totalDamage = Mathf.CeilToInt(stats.baseDamage * Mathf.Pow(stats.upgradeDamageMultiplier, stats.upgradeLevel));
                // enemy.TakeDamage(totalDamage); // Burayı kendi düşman sistemine göre tamamla
                Debug.Log($"{totalDamage} damage dealt to {enemy.name}");
            }
        }
    }

    public void UpgradeWeapon()
    {
        if (stats.upgradeLevel < maxUpgradeLevel)
        {
            stats.upgradeLevel++;
            stats.baseDamage = Mathf.CeilToInt(stats.baseDamage * stats.upgradeDamageMultiplier);
            stats.baseFireRate *= stats.upgradeFireRateMultiplier;

            if (!stats.infiniteDurability) // Eğer dayanıklılık sonsuz değilse
            {
                stats.durability += stats.upgradeDurabilityBonus;
            }

            Debug.Log($"{stats.weaponName} upgraded to level {stats.upgradeLevel}! New damage: {stats.baseDamage}, Fire rate: {stats.baseFireRate}");
        }
        else
        {
            Debug.Log($"{stats.weaponName} is already at max upgrade level!");
        }
    }
}
