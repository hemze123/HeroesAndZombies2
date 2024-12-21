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
            HandleWeaponDestruction();
        }
        else if (currentGun.weaponType == Weapon.WeaponType.Melee && currentGun.stats.durability <= 0)
        {
            HandleWeaponDestruction();
        }

        if (isFiring)
        {
            currentGun.Fire();
            UpdateAmmoBar();
        }
    }
    else if (suankiSilah == null || !suankiSilah.activeSelf)
    {
        EquipDefaultKnife();
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
        // Eğer silah default bıçak ise hiçbir şey yapma
        if (suankiSilah == DefaultBicak)
        {
            return;
        }

        suankiSilah.transform.parent = null;
        suankiSilah.GetComponent<BoxCollider>().enabled = true;

        // Rigidbody ekleme işlemi sadece DefaultBicak değilse yapılır
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


private void EquipDefaultKnife()
{
    if (DefaultBicak == null)
    {
        Debug.LogError("DefaultBicak tanımlanmamış! Lütfen Inspector'da atayın.");
        return;
    }

    // Default bıçağı aktif hale getir
    DefaultBicak.SetActive(true);
    
    // SuankiSilah referansını güncelle
    suankiSilah = DefaultBicak;

    // Bıçağın pozisyonunu ve rotasyonunu güncelle (isteğe bağlı)
    DefaultBicak.transform.SetParent(silahPosition);
    DefaultBicak.transform.localPosition = Vector3.zero;
    DefaultBicak.transform.localRotation = Quaternion.identity;

    // Animator'ı bıçak moduna güncelle
    playerMovement.playerAnimator.SetInteger("WeaponType_int", 0);
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

    // Varsayılan bıçağı sadece pasif yap, fakat parent'ı değiştirme
    if (DefaultBicak != null)
    {
        DefaultBicak.SetActive(false);
        DefaultBicak.transform.SetParent(transform);  // Bıçak her zaman player'ın child'ı olarak kalır
    }

    suankiSilah = newWeapon;

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


private void HandleWeaponDestruction()
{
    if (suankiSilah != null)
    {
        Destroy(suankiSilah);
        suankiSilah = null;
        currentGun = null;
    }
           
    // Varsayılan bıçağı yeniden aktif et
    EquipDefaultKnife();

    // Ammo barını kapat (varsayılan bıçak için gerekmez)
    Bar.gameObject.SetActive(false);
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
