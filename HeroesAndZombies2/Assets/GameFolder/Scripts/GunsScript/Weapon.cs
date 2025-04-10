using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Ranged }
    public WeaponType weaponType;
    public WeaponStats stats;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    public float nextFireTime;

    private IWeaponAttackStrategy attackStrategy;

    private void Awake()
    {
        attackStrategy = weaponType switch
        {
            WeaponType.Melee => new MeleeAttackStrategy(),
            WeaponType.Ranged => new RangedAttackStrategy(),
            _ => null
        };

        if (weaponType == WeaponType.Ranged)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = stats.rayColor };
        }

        if (!stats.infiniteDurability)
            stats.currentDurability = stats.maxDurability;
    }

    public void TryAttack()
    {
        if (attackStrategy != null && attackStrategy.CanAttack(this))
        {
            attackStrategy.Attack(this);
            nextFireTime = Time.time + GetFireRate();
        }
    }

    public int CalculateDamage()
    {
        float damage = stats.baseDamage;

        if (stats.upgradePaths.Count > 0 && stats.currentUpgradeLevel > 0)
        {
            damage *= Mathf.Pow(stats.upgradePaths[0].damageMultiplier, stats.currentUpgradeLevel);
        }

        return Mathf.CeilToInt(damage);
    }

    public float GetFireRate()
    {
        float fireRate = stats.baseFireRate;

        if (stats.upgradePaths.Count > 0 && stats.currentUpgradeLevel > 0)
        {
            fireRate *= Mathf.Pow(stats.upgradePaths[0].fireRateMultiplier, stats.currentUpgradeLevel);
        }

        return fireRate;
    }

    public Vector3 GetFireOrigin() => firePoint != null ? firePoint.position : transform.position;
    public Vector3 GetFireDirection() => firePoint != null ? firePoint.forward : transform.forward;

    public void ClearRay()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }


    public void Fire()
    {
      TryAttack();
    }
}
