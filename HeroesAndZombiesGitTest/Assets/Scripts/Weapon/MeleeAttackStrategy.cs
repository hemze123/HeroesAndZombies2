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
        Debug.Log($"Melee attack with {weapon.stats.weaponName}");
        
        if (weapon.data.swingClip != null)
        AudioManager.Instance.PlaySFX(weapon.data.swingClip);

        Vector3 origin = weapon.transform.root.position + Vector3.up * 1f;
        Vector3 forward = weapon.transform.root.forward;
        float radius = weapon.stats.attackRadius;
        float angle = weapon.stats.attackAngle;

        Collider[] colliders = Physics.OverlapSphere(origin, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy tapildi: " + collider.name);

                Vector3 directionToTarget = (collider.transform.position - origin).normalized;
                float angleToTarget = Vector3.Angle(forward, directionToTarget);

                if (angleToTarget <= angle / 2f)
                {
                    Debug.Log("Enemy koni içinde! Hasar verilir → " + collider.name);

                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        int damage = weapon.CalculateDamage();
                        enemy.TakeDamage(damage);

                        Debug.Log($"{enemy.name} took {damage} damage! HP azalir.");
                    }
                    else
                    {
                        Debug.LogWarning($"Enemy script tapilmadi: {collider.name}");
                    }
                }
            }
        }

        if (!weapon.stats.infiniteDurability)
        {
            weapon.stats.currentDurability = Mathf.Max(0, weapon.stats.currentDurability - 1);
            Debug.Log($"{weapon.stats.weaponName} durability: {weapon.stats.currentDurability}");

            if (weapon.stats.currentDurability <= 0)
            {
                Debug.Log($"{weapon.stats.weaponName} broke!");
                GameObject.Destroy(weapon.gameObject);
            }
        }

        DebugCone(origin, forward, radius, angle);
    }

    private void PerformConeAttack(Weapon weapon)
    {
        Vector3 origin = weapon.transform.position;
        Vector3 forward = weapon.transform.forward;
        float radius = weapon.stats.attackRadius;
        float angle = weapon.stats.attackAngle;

        Collider[] colliders = Physics.OverlapSphere(origin, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy tapildi: " + collider.name);

                Vector3 directionToTarget = (collider.transform.position - origin).normalized;
                float angleToTarget = Vector3.Angle(forward, directionToTarget);

                if (angleToTarget <= angle / 2f)
                {
                    Debug.Log("Enemy koni içinde! Hasar verilir.");

                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        int damage = weapon.CalculateDamage();
                        Debug.Log($"{enemy.name} took {damage} damage");
                        enemy.TakeDamage(damage);
                    }
                }
            }
        }

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
            Debug.DrawLine(origin, point, Color.green, 0.5f);

            if (i > 0)
                Debug.DrawLine(lastPoint, point, Color.green, 0.5f);

            lastPoint = point;
        }
#endif
    }
}