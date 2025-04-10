using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedAttackStrategy : IWeaponAttackStrategy
{
    public bool CanAttack(Weapon weapon)
    {
        return Time.time >= weapon.nextFireTime &&
               (weapon.stats.infiniteAmmo || weapon.stats.currentAmmo > 0);
    }

    public void Attack(Weapon weapon)
    {
        Vector3 origin = weapon.GetFireOrigin();
        Vector3 dir = weapon.GetFireDirection();
        float range = weapon.stats.range;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.TakeDamage(weapon.CalculateDamage());
            }
        }

        if (!weapon.stats.infiniteAmmo)
            weapon.stats.currentAmmo--;

        if (weapon.lineRenderer != null)
        {
            weapon.lineRenderer.SetPosition(0, origin);
            weapon.lineRenderer.SetPosition(1, origin + dir * range);
            weapon.Invoke(nameof(weapon.ClearRay), weapon.stats.rayDuration);
        }
    }
}
