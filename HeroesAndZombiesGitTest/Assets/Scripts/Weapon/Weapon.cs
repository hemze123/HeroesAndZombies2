using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Ranged }

    [Header("Data")]
    public WeaponItemSO data;

    [Header("Owner")]
    public Transform ownerRoot;

    [Header("Visual Roots")]
    public Transform boxRoot;
    public Transform gunRoot;
    public Transform firePoint;
    public Transform gripPoint;

    [Header("Runtime")]
    public WeaponStats stats;
    public float nextFireTime;
    public LineRenderer lineRenderer;

    private Collider pickupCollider;
    private Rigidbody rb;
    private IWeaponAttackStrategy attackStrategy;

    public WeaponType weaponType => data != null ? data.weaponType : WeaponType.Melee;

   private void Awake()
{
    pickupCollider = GetComponentInChildren<Collider>(true);
    rb = GetComponentInChildren<Rigidbody>(true);

    if (boxRoot == null) boxRoot = transform.Find("Box");
    if (gunRoot == null) gunRoot = transform.Find("Gun");

    stats = data.stats.Clone();

    attackStrategy = weaponType == WeaponType.Melee
        ? new MeleeAttackStrategy()
        : new RangedAttackStrategy();

    SetVisuals(false);
}

    #region PICKUP / DROP

    public void OnPickup()
{
    if (rb != null)
    {
        Destroy(rb);
        rb = null;
    }

    if (pickupCollider != null)
        pickupCollider.enabled = false;

    SetVisuals(true);
}


   public void OnDrop(Vector3 dropPosition, Vector3 dropForce, float torque = 0f)
{
    transform.SetParent(null);
    transform.position = dropPosition;

    SetVisuals(false);

    if (pickupCollider != null)
    {
        pickupCollider.enabled = true;
        pickupCollider.isTrigger = false;
    }

    rb = gameObject.AddComponent<Rigidbody>();
    rb.useGravity = true;
    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    rb.interpolation = RigidbodyInterpolation.Interpolate;

    rb.AddForce(dropForce, ForceMode.Impulse);

    if (torque > 0f)
        rb.AddTorque(Random.insideUnitSphere * torque, ForceMode.Impulse);

    ownerRoot = null;
}

    private void SetVisuals(bool inHand)
    {
        if (boxRoot != null) boxRoot.gameObject.SetActive(!inHand);
        if (gunRoot != null) gunRoot.gameObject.SetActive(inHand);
    }

    #endregion

    #region FIRE + STRATEGY

    public void Fire()
    {
        TryAttack();

        if (data != null && data.fireClip != null)
            AudioManager.Instance.PlaySFX(data.fireClip);
    }

    public void TryAttack()
    {
        if (attackStrategy == null) return;

        if (attackStrategy.CanAttack(this))
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
            damage *= Mathf.Pow(
                stats.upgradePaths[0].damageMultiplier,
                stats.currentUpgradeLevel
            );
        }

        return Mathf.CeilToInt(damage);
    }

    public void PlayShootEffects(Vector3 hitPoint)
    {
        if (data != null && data.muzzleFlashPrefab != null && firePoint != null)
        {
            GameObject flash = Instantiate(
                data.muzzleFlashPrefab,
                firePoint.position,
                firePoint.rotation,
                firePoint
            );
            Destroy(flash, 0.3f);
        }

        if (data != null && data.bulletTrailPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(
                data.bulletTrailPrefab,
                firePoint.position,
                Quaternion.identity
            );

            AnimatedLine anim = bullet.GetComponent<AnimatedLine>();
            if (anim != null)
                anim.Init(firePoint.position, hitPoint);
        }
    }

    public float GetFireRate()
    {
        float fireRate = stats.baseFireRate;

        if (stats.upgradePaths.Count > 0 && stats.currentUpgradeLevel > 0)
        {
            fireRate *= Mathf.Pow(
                stats.upgradePaths[0].fireRateMultiplier,
                stats.currentUpgradeLevel
            );
        }

        return fireRate;
    }

    #endregion

    #region HELPERS

    public Vector3 GetFireOrigin()
    {
        return firePoint != null ? firePoint.position : transform.position;
    }

    public Vector3 GetFireDirection()
    {
        return firePoint != null ? firePoint.forward : transform.forward;
    }

    public void ClearRay()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }

    #endregion
}
