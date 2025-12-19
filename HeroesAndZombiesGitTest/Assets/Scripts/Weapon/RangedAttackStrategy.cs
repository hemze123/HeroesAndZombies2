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

    Vector3 hitPoint = origin + dir * range;

    if (Physics.Raycast(origin, dir, out RaycastHit hit, range))
    {
        hitPoint = hit.point;

        if (hit.collider.CompareTag("Enemy"))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(weapon.CalculateDamage());
        }

       
        if (weapon.data.impactEffectPrefab != null)
        {
            GameObject impact = GameObject.Instantiate(weapon.data.impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject.Destroy(impact, 1f);
        }
    }

    if (!weapon.stats.infiniteAmmo)
        weapon.stats.currentAmmo--;

    
    weapon.PlayShootEffects(hitPoint);

   
    weapon.nextFireTime = Time.time + weapon.GetFireRate();
}

}
