using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject suankiSilah;
    public Transform silahPosition;
    PlayerMovement playerMovement; // PlayerMovement script'ine referans
    private Weapon currentGun;
    public GameObject DefaultBicak; // Varsayılan bıçak objesi
    public Button fireButton; // Ateş etme butonu

    public GameObject Bar; // Mermi barı UI objesi
    public Slider ammoBar;
    private bool isFiring;
    private bool isAttacking;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.playerAnimator.SetInteger("WeaponType_int", 0);
    }

    void Update()
    {
        if (currentGun != null && isFiring)
        {
            UpdateAmmoBar();
            if (currentGun.weaponType == Weapon.WeaponType.Ranged)
            {
                currentGun.Fire();
            }
            else if (currentGun.weaponType == Weapon.WeaponType.Melee)
            {
                isAttacking = true;
            }
        }
        else
        {
            isAttacking = false;
        }

        if (suankiSilah == null)
        {
            Bar.gameObject.SetActive(false);
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
                    Bar.gameObject.SetActive(true);
                }
                else
                {
                    PickUpWeapon(newWeapon);
                    Bar.gameObject.SetActive(true);
                }
                UpdateAmmoBar();
                currentGun = gunComponent;
            }
        }
    }

    private void DropAndPickup(GameObject currentWeapon, GameObject newWeapon)
    {
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<BoxCollider>().enabled = true;

        if (currentWeapon.GetComponent<Rigidbody>() == null)
        {
            currentWeapon.AddComponent<Rigidbody>();
        }

        Transform box = currentWeapon.transform.Find("Box");
        Transform gun = currentWeapon.transform.Find("Gun");

        if (box != null && gun != null)
        {
            box.gameObject.SetActive(true);
            gun.gameObject.SetActive(false);
        }

        Vector3 dropDirection = transform.forward;
        Vector3 dropPosition = currentWeapon.transform.position + dropDirection * 3 + Vector3.up * 2;
        currentWeapon.transform.position = dropPosition;

        PickUpWeapon(newWeapon);
    }

    private void PickUpWeapon(GameObject newWeapon)
    {
        if (newWeapon.GetComponent<Rigidbody>() != null)
        {
            Destroy(newWeapon.GetComponent<Rigidbody>());
        }
        suankiSilah = newWeapon;
        suankiSilah.transform.parent = transform;
        Transform box = suankiSilah.transform.Find("Box");
        Transform gun = suankiSilah.transform.Find("Gun");
        newWeapon.GetComponent<BoxCollider>().enabled = false;
        newWeapon.transform.position = silahPosition.position;
        gun.transform.position = silahPosition.position;
        gun.transform.rotation = silahPosition.rotation;

        if (box != null && gun != null)
        {
            box.gameObject.SetActive(false);
            gun.gameObject.SetActive(true);
        }
    }

    public void PlayerFire()
    {
        if (currentGun != null)
        {
            currentGun.Fire();
        }
    }

    private void UpdateAmmoBar()
    {
        if (ammoBar != null && currentGun != null && currentGun.weaponType == Weapon.WeaponType.Ranged)
        {
            ammoBar.value = (float)currentGun.stats.currentAmmo / currentGun.stats.maxAmmo;
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