using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour
{
    [Header("References")]
    public WeaponItemSO defaultWeaponSO;
    public Transform weaponSocket;
    public Slider ammoBar;
    public GameObject barRoot;

    private GameObject defaultWeaponGO;
    public Weapon currentWeapon;
    private Animator anim;

    private bool isFiring = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        
        if (weaponSocket == null && anim != null)
        {
            var leftHand = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            if (leftHand != null)
            {
                Transform ws = leftHand.Find("WeaponHolder");
                if (ws != null) weaponSocket = ws;
            }
        }
    }

    private void OnEnable()
    {
        if (HUDReferences.Instance != null)
        {
            ammoBar = HUDReferences.Instance.ammoBar;
            barRoot = HUDReferences.Instance.barRoot;
        }
    }

    private void Start()
    {
        EquipDefaultWeapon();
    }

    private void Update()
    {
        if (isFiring && currentWeapon != null)
        {
            if (Time.time >= currentWeapon.nextFireTime)
            {
                currentWeapon.Fire();
                UpdateAmmoBar();

                // Animasyon
                if (currentWeapon.weaponType == Weapon.WeaponType.Melee)
                {
                    SetAnimatorAttack();
                }
                else if (currentWeapon.weaponType == Weapon.WeaponType.Ranged)
                {
                    SetAnimatorAttack();
                }

                // Ammo / Durability qurtaranda default’a qayit
                if (currentWeapon.weaponType == Weapon.WeaponType.Ranged &&
                    !currentWeapon.stats.infiniteAmmo &&
                    currentWeapon.stats.currentAmmo <= 0)
                {
                    DropCurrentWeapon(keepDefault: false, destroyInstead: true);
                    EquipDefaultWeapon();
                }

                if (currentWeapon.weaponType == Weapon.WeaponType.Melee &&
                    !currentWeapon.stats.infiniteDurability &&
                    currentWeapon.stats.currentDurability <= 0)
                {
                    DropCurrentWeapon(keepDefault: false, destroyInstead: true);
                    EquipDefaultWeapon();
                }
            }
        }

        // ammo/durability güncelle
        UpdateAmmoBar();
    }

    #region FIRE
    public void StartFiring()
    {
        isFiring = true;
    }

    public void StopFiring()
    {
        isFiring = false;
        if (currentWeapon != null)
            SetAnimatorIdle();
    }
    #endregion

    #region EQUIP / PICKUP / DROP
    public void EquipDefaultWeapon()
    {
        if (defaultWeaponSO == null || defaultWeaponSO.weaponPrefab == null)
        {
            Debug.LogError("[WeaponSystem] Default Weapon SO / Prefab bos!");
            return;
        }

        
        if (defaultWeaponGO == null)
        {
            defaultWeaponGO = Instantiate(defaultWeaponSO.weaponPrefab);
        }

        currentWeapon = defaultWeaponGO.GetComponent<Weapon>();

        if (currentWeapon == null)
        {
            Debug.LogError("[WeaponSystem] Default weapon prefab içinde Weapon componenti yoxdur!");
            return;
        }

        
        currentWeapon.OnPickup();

        // Elde tarazla
        AttachWeaponToHand(currentWeapon);

        // Default için bar gizli
        if (barRoot != null) barRoot.SetActive(false);

        UpdateAmmoBar();
        SetAnimatorIdle();
    }

    public void TryPickupWeapon(GameObject weaponGO)
{
    if (weaponGO == null) return;

    Weapon newWeapon = weaponGO.GetComponent<Weapon>();
    if (newWeapon == null) return;

    
    if (currentWeapon != null && newWeapon.gameObject == currentWeapon.gameObject)
        return;

   
    Rigidbody newWeaponRB = newWeapon.GetComponent<Rigidbody>();
    if (newWeaponRB != null && newWeaponRB.isKinematic)
    {
        
        return;
    }

   
    DropCurrentWeapon(keepDefault: true);

    
    newWeapon.OnPickup();

   
    if (currentWeapon != null && currentWeapon.gameObject == defaultWeaponGO)
    {
       
        defaultWeaponGO.SetActive(false);
    }

    currentWeapon = newWeapon;

   
    AttachWeaponToHand(newWeapon);

   
    if (barRoot != null) barRoot.SetActive(true);
    UpdateAmmoBar();
    SetAnimatorIdle();
}

 public void DropCurrentWeapon(bool keepDefault = true, bool destroyInstead = false)
{
    if (currentWeapon == null) return;

    bool isDefault = (defaultWeaponGO != null && currentWeapon.gameObject == defaultWeaponGO);

   
    if (isDefault)
    {
        
        defaultWeaponGO.SetActive(false);
        currentWeapon = null;
        
        // HUD'ı gizle
        if (barRoot != null) barRoot.SetActive(false);
        return;
    }

  
    Vector3 dropPos = weaponSocket.position + transform.forward * 1.0f + Vector3.up * 0.5f;
    Vector3 throwDir = (transform.forward + Vector3.up * 0.2f).normalized;
    float throwPower = 5f;
    Vector3 throwForce = throwDir * throwPower;
    float torque = 3f;

   
    currentWeapon.OnDrop(dropPos, throwForce, torque);


    Weapon droppedWeapon = currentWeapon;
    currentWeapon = null;

   
    StartCoroutine(ResetWeaponPickupCooldown(droppedWeapon));

    
    if (keepDefault && defaultWeaponGO != null)
    {
       
        defaultWeaponGO.SetActive(true); 
        currentWeapon = defaultWeaponGO.GetComponent<Weapon>();
        
        AttachWeaponToHand(currentWeapon);
        
        
        if (barRoot != null) barRoot.SetActive(false);
    }
    else
    {
        
        if (barRoot != null) barRoot.SetActive(false);
    }
    
    
    UpdateAmmoBar();
}


private IEnumerator ResetWeaponPickupCooldown(Weapon droppedWeapon)
{
    
    Collider weaponCollider = droppedWeapon.GetComponent<Collider>();
    if (weaponCollider != null)
    {
        weaponCollider.enabled = false;
    }
    
  
    yield return new WaitForSeconds(0.5f);
    
    
    if (weaponCollider != null)
    {
        weaponCollider.enabled = true;
    }
}
    #endregion

    #region ATTACH
    private void AttachWeaponToHand(Weapon weapon)
    {
        if (weaponSocket == null || weapon == null) return;

        
        weapon.ownerRoot = weaponSocket.root;

     
        if (weapon.gripPoint == null)
        {
            weapon.transform.SetParent(weaponSocket);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            return;
        }

      
        weapon.transform.SetParent(weaponSocket, false);

       
        Vector3 worldOffsetPos = weaponSocket.position - weapon.gripPoint.position;
        Quaternion worldOffsetRot = weaponSocket.rotation * Quaternion.Inverse(weapon.gripPoint.rotation);

        weapon.transform.position += worldOffsetPos;
        weapon.transform.rotation = worldOffsetRot * weapon.transform.rotation;
    }
    #endregion

    #region UI
    private void UpdateAmmoBar()
    {
        if (ammoBar == null || currentWeapon == null)
        {
            if (barRoot != null) barRoot.SetActive(false);
            return;
        }

        if (currentWeapon.weaponType == Weapon.WeaponType.Ranged)
        {
            if (barRoot != null) barRoot.SetActive(true);
            ammoBar.value = (float)currentWeapon.stats.currentAmmo / currentWeapon.stats.maxAmmo;
        }
        else if (currentWeapon.weaponType == Weapon.WeaponType.Melee)
        {
            if (barRoot != null) barRoot.SetActive(true);
            ammoBar.value = (float)currentWeapon.stats.currentDurability / currentWeapon.stats.maxDurability;
        }
        else
        {
            if (barRoot != null) barRoot.SetActive(false);
        }
    }

    private void SetAnimatorIdle()
    {
        if (anim == null || currentWeapon == null) return;

        int idleId = currentWeapon.data.idleAnimId > 0
            ? currentWeapon.data.idleAnimId
            : currentWeapon.data.weaponTypeAnimID;

        anim.SetInteger("WeaponType_int", idleId);
    }

    private void SetAnimatorAttack()
    {
        if (anim == null || currentWeapon == null) return;

        int attackId = currentWeapon.data.attackAnimId > 0
            ? currentWeapon.data.attackAnimId
            : currentWeapon.data.weaponTypeAnimID;

        anim.SetInteger("WeaponType_int", attackId);

     
        float delay = Mathf.Max(0f, currentWeapon.data.attackToIdleDelay);
        if (delay > 0f) Invoke(nameof(SetAnimatorIdle), delay);
    }
    #endregion
}
