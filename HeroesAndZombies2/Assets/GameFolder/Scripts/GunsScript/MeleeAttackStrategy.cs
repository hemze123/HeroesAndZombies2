using UnityEngine;

public class MeleeAttackStrategy : IWeaponAttackStrategy
{
    public bool CanAttack(Weapon weapon)
    {
        return Time.time >= weapon.nextFireTime &&
               (weapon.stats.infiniteDurability || weapon.stats.currentDurability > 0);
    }

    public void Attack(Weapon weapon)
    {
        PerformConeAttack(weapon);

        if (!weapon.stats.infiniteDurability)
        {
            weapon.stats.currentDurability = Mathf.Max(0, weapon.stats.currentDurability - 1);
            if (weapon.stats.currentDurability <= 0)
            {
                Debug.Log($"{weapon.stats.weaponName} broke!");
                GameObject.Destroy(weapon.gameObject);
            }
        }
    }

    private void PerformConeAttack(Weapon weapon)
    {
        Vector3 origin = weapon.GetFireOrigin();
        Vector3 forward = weapon.GetFireDirection();
        float radius = weapon.stats.attackRadius;
        float angle = weapon.stats.attackAngle;

        // Yakındaki colliderları bul (düşmanları filtrele)
        Collider[] colliders = Physics.OverlapSphere(origin, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = (collider.transform.position - origin).normalized;
                float angleToTarget = Vector3.Angle(forward, directionToTarget);

                if (angleToTarget <= angle / 2f)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(weapon.CalculateDamage());
                    }
                }
            }
        }

        // Debug görseli
        DebugCone(origin, forward, radius, angle);
    }

    private void DebugCone(Vector3 origin, Vector3 forward, float radius, float angle)
    {
#if UNITY_EDITOR
        int segments = 20;
        float halfAngle = angle / 2f;
        Vector3 lastPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (angle * i / segments);
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            Vector3 direction = rotation * forward;

            Vector3 point = origin + direction * radius;
            Debug.DrawLine(origin, point, Color.red, 0.5f);

            if (i > 0)
            {
                Debug.DrawLine(lastPoint, point, Color.red, 0.5f);
            }

            lastPoint = point;
        }
#endif
    }
}
