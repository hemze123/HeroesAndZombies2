using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject suankiSilah;
    public Transform silahPosition;
    private PlayerMovement playerMovement;
    private Weapon currentGun;
    public GameObject DefaultBicak; // Varsayılan bıçak objesi
    public Button fireButton;
    public GameObject Bar;
    public Slider ammoBar;
    private bool isFiring;
    private bool isAttacking;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.playerAnimator.SetInteger("WeaponType_int", 0);

        if (DefaultBicak == null)
        {
            Debug.LogError("DefaultBicak nesnesi atanmamış! Lütfen Inspector'da bir varsayılan bıçak belirtin.");
            return;
        }

        EquipDefaultKnife(); // Oyunun başlangıcında varsayılan bıçağı donat
    }

    void Update()
    {
        if (currentGun != null)
        {
            if (currentGun.weaponType == Weapon.WeaponType.Ranged && currentGun.stats.currentAmmo <= 0)
            {
                HandleWeaponDestruction(); // Mermi bittiyse silah yok et
            }
            else if (currentGun.weaponType == Weapon.WeaponType.Melee && currentGun.stats.durability <= 0)
            {
                HandleWeaponDestruction(); // Dayanıklılık bittiyse silah yok et
            }

            if (isFiring)
            {
                if (currentGun.weaponType == Weapon.WeaponType.Ranged)
                {
                    currentGun.Fire();
                    UpdateAmmoBar();
                }
                else if (currentGun.weaponType == Weapon.WeaponType.Melee)
                {
                    isAttacking = true;
                    currentGun.Fire();
                    UpdateAmmoBar(); // Dayanıklılık barını güncelle
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    public void OnPointerDown()
    {
        isFiring = true;
    }

    public void OnPointerUp()
    {
        isFiring = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleWeaponCollection(collision);
    }

    private void HandleWeaponCollection(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.tag == "Weapon")
        {
            GameObject newWeapon = collidedObject;

            Weapon gunComponent = newWeapon.GetComponent<Weapon>();
            if (gunComponent != null)
            {
                if (suankiSilah != null)
                {
                    DropAndPickup(suankiSilah, newWeapon);
                }
                else
                {
                    PickUpWeapon(newWeapon);
                }
                currentGun = gunComponent;
                Bar.gameObject.SetActive(true);
                UpdateAmmoBar();
            }
        }
    }

    private void DropAndPickup(GameObject currentWeapon, GameObject newWeapon)
    {
        DropCurrentWeapon();
        PickUpWeapon(newWeapon);
    }

    private void DropCurrentWeapon()
    {
        if (suankiSilah != null)
        {
            suankiSilah.transform.parent = null;
            suankiSilah.GetComponent<BoxCollider>().enabled = true;

            if (suankiSilah.GetComponent<Rigidbody>() == null)
            {
                suankiSilah.AddComponent<Rigidbody>();
            }

            Transform box = suankiSilah.transform.Find("Box");
            Transform gun = suankiSilah.transform.Find("Gun");

            if (box != null && gun != null)
            {
                box.gameObject.SetActive(true);
                gun.gameObject.SetActive(false);
            }

            Vector3 dropDirection = transform.forward;
            Vector3 dropPosition = suankiSilah.transform.position + dropDirection * 3 + Vector3.up * 2;
            suankiSilah.transform.position = dropPosition;
        }
    }

private void HandleWeaponDestruction()
{
    // Mevcut silahı yok et
    if (suankiSilah != null)
    {
        Destroy(suankiSilah); // Silahı yok et
        suankiSilah = null;
        currentGun = null;
    }

    // Varsayılan bıçağı yeniden aktif et
    EquipDefaultKnife();

    // Ammo barını kapat (varsayılan bıçak için gerekmez)
    Bar.gameObject.SetActive(false);
}

private void EquipDefaultKnife()
{
    if (DefaultBicak == null)
    {
        Debug.LogError("DefaultBicak tanımlanmamış! Lütfen Inspector'da atayın.");
        return;
    }

    // Varsayılan bıçağı aktif hale getir
    DefaultBicak.SetActive(true);

    // Varsayılan bıçağı doğru pozisyona ve rotasyona ayarla
    DefaultBicak.transform.SetParent(silahPosition, false);
    DefaultBicak.transform.localPosition = Vector3.zero;
    DefaultBicak.transform.localRotation = Quaternion.identity;

    // Animator'ı varsayılan bıçağa uygun şekilde güncelle
    playerMovement.playerAnimator.SetInteger("WeaponType_int", 0);

    // SuankiSilah referansını DefaultBicak olarak ayarla
    suankiSilah = DefaultBicak;
}


private void PickUpWeapon(GameObject newWeapon)
{
    if (newWeapon == null)
    {
        Debug.LogError("PickUpWeapon() metodu için verilen silah nesnesi null!");
        return;
    }

    if (newWeapon.GetComponent<Rigidbody>() != null)
    {
        Destroy(newWeapon.GetComponent<Rigidbody>());
    }

    // Yeni silah alındığında varsayılan bıçağı pasif yap
    if (DefaultBicak != null)
    {
        DefaultBicak.SetActive(false);
    }

    suankiSilah = newWeapon;
    suankiSilah.transform.parent = silahPosition;

    Transform box = suankiSilah.transform.Find("Box");
    Transform gun = suankiSilah.transform.Find("Gun");
    newWeapon.GetComponent<BoxCollider>().enabled = false;
    newWeapon.transform.position = silahPosition.position;

    if (gun != null)
    {
        gun.transform.position = silahPosition.position;
        gun.transform.rotation = silahPosition.rotation;
    }

    if (box != null && gun != null)
    {
        box.gameObject.SetActive(false);
        gun.gameObject.SetActive(true);
    }

    currentGun = suankiSilah.GetComponent<Weapon>();
    Bar.gameObject.SetActive(true); // Ammo barını göster
    UpdateAmmoBar();
}






    private void UpdateAmmoBar()
    {
        if (ammoBar != null && currentGun != null)
        {
            if (currentGun.weaponType == Weapon.WeaponType.Ranged)
            {
                ammoBar.value = (float)currentGun.stats.currentAmmo / currentGun.stats.maxAmmo;
            }
            else if (currentGun.weaponType == Weapon.WeaponType.Melee)
            {
                ammoBar.value = (float)currentGun.stats.durability / 100; // Dayanıklılık için % bar
            }
        }
    }

    public void UpgradeCurrentWeapon()
    {
        if (currentGun != null)
        {
            currentGun.UpgradeWeapon();
        }
    }
}
