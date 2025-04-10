using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject suankiSilah; // Şu anda kullanılan silah
    public Transform silahPosition; // Silahın tutulacağı pozisyon
    private PlayerMovement playerMovement; // Oyuncu hareket kontrolü
    private Weapon currentGun; // Şu anki silahın bileşeni
    public GameObject DefaultBicak; // Varsayılan bıçak objesi
    public Button fireButton; // Ateş etme butonu
    public GameObject Bar; // Mermi/dayanıklılık barı
    public Slider ammoBar; // Mermi/dayanıklılık barı slider'ı
    private bool isFiring; // Ateş ediliyor mu?
    private bool isAttacking; // Saldırı yapılıyor mu?

    public Animator playerAnimatorController;

     void Awake(){
          playerAnimatorController = GetComponentInChildren<Animator>();
     }
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimatorController.SetInteger("WeaponType_int", 5); // Başlangıçta bıçak animasyonu

        if (DefaultBicak == null)
        {
            Debug.LogError("DefaultBicak nesnesi atanmamış! Lütfen Inspector'da bir varsayılan bıçak belirtin.");
            return;
        }

        EquipDefaultKnife(); // Oyun başlangıcında varsayılan bıçağı donat
    }

    void Update()
    {
        if (currentGun != null)
    {
        // Silahın mermisi veya dayanıklılığı bittiğinde
        if ((currentGun.weaponType == Weapon.WeaponType.Ranged && currentGun.stats.currentAmmo <= 0) ||
            (currentGun.weaponType == Weapon.WeaponType.Melee && currentGun.stats.currentDurability <= 0))
        {
            HandleWeaponDestruction();
        }

        // Ateş etme işlemi
        if (isFiring && currentGun != null)
        {
            currentGun.Fire();
            UpdateAmmoBar();

            // Melee silah ateş ettiğinde animasyonu değiştir
            if (currentGun.weaponType == Weapon.WeaponType.Melee)
            {
                playerAnimatorController.SetInteger("WeaponType_int", 12);
            }
        }
    }
    else if (suankiSilah == null || !suankiSilah.activeSelf)
    {
        EquipDefaultKnife(); // Silah yoksa varsayılan bıçağı donat
    }
    }

  public void OnPointerDown()
{
    isFiring = true; // Ateş etmeye başla

    if (currentGun != null)
    {
        if (currentGun.weaponType == Weapon.WeaponType.Melee)
        {
            playerAnimatorController.SetInteger("WeaponType_int", 12); // Melee saldırı animasyonu
        }
        
        currentGun.Fire(); // Silah ateş etme fonksiyonunu çağır
        UpdateAmmoBar();
    }
}

public void OnPointerUp()
{
    isFiring = false; // Ateş etmeyi durdur
}

    private void OnCollisionEnter(Collision collision)
    {
        HandleWeaponCollection(collision); // Çarpışma durumunda silah toplama
    }

    private void HandleWeaponCollection(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Weapon")) // Çarpışan nesne silah mı?
        {
            GameObject newWeapon = collidedObject;

            Weapon gunComponent = newWeapon.GetComponent<Weapon>();
            if (gunComponent != null)
            {
                if (suankiSilah != null)
                {
                    DropAndPickup(suankiSilah, newWeapon); // Mevcut silahı bırak ve yeni silahı al
                }
                else
                {
                    PickUpWeapon(newWeapon); // Yeni silahı al
                }
                currentGun = gunComponent;
                Bar.gameObject.SetActive(true); // Bar'ı göster
                UpdateAmmoBar(); // Bar'ı güncelle
            }
        }
    }

    private void DropAndPickup(GameObject currentWeapon, GameObject newWeapon)
    {
        DropCurrentWeapon(); // Mevcut silahı bırak
        PickUpWeapon(newWeapon); // Yeni silahı al
    }

    private void DropCurrentWeapon()
    {
        if (suankiSilah != null && suankiSilah != DefaultBicak) // Varsayılan bıçak değilse
        {
            suankiSilah.transform.parent = null; // Parent'ı kaldır
            suankiSilah.GetComponent<BoxCollider>().enabled = true; // Collider'ı etkinleştir

            // Rigidbody ekle (eğer yoksa)
            if (suankiSilah.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = suankiSilah.AddComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }

            // Silahın kutusunu ve modelini ayarla
            Transform box = suankiSilah.transform.Find("Box");
            Transform gun = suankiSilah.transform.Find("Gun");

            if (box != null && gun != null)
            {
                box.gameObject.SetActive(true); // Kutu görünür
                gun.gameObject.SetActive(false); // Silah modeli gizli
            }

            // Silahı ileriye doğru fırlat
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

        // Varsayılan bıçağı aktif et
        DefaultBicak.SetActive(true);
        suankiSilah = DefaultBicak;

        // Bıçağı doğru pozisyon ve rotasyona yerleştir
        DefaultBicak.transform.SetParent(silahPosition);
        DefaultBicak.transform.localPosition = Vector3.zero;
        DefaultBicak.transform.localRotation = Quaternion.identity;

        // Animator'ı bıçak moduna ayarla
        playerMovement.playerAnimatorMove.SetInteger("WeaponType_int", 5);

        // Bar'ı gizle (bıçak için gerekmez)
        Bar.gameObject.SetActive(false);
    }

    private void PickUpWeapon(GameObject newWeapon)
    {
       if (newWeapon == null)
    {
        Debug.LogError("PickUpWeapon() metodu için verilen silah nesnesi null!");
        return;
    }

    // Rigidbody'yi kaldır
    if (newWeapon.GetComponent<Rigidbody>() != null)
    {
        Destroy(newWeapon.GetComponent<Rigidbody>());
    }

    // Varsayılan bıçağı gizle
    if (DefaultBicak != null)
    {
        DefaultBicak.SetActive(false);
    }

    // Yeni silahı aktif et
    suankiSilah = newWeapon;
    newWeapon.GetComponent<BoxCollider>().enabled = false; // Collider'ı devre dışı bırak
    newWeapon.transform.SetParent(silahPosition); // Silahı silahPosition'a bağla
    
    currentGun = newWeapon.GetComponent<Weapon>();

    // Silahın türüne göre farklı tutuş pozisyonları ve rotasyonları ayarla
    if (currentGun.weaponType == Weapon.WeaponType.Ranged)
    {
        newWeapon.transform.localPosition = Vector3.zero; // Pozisyonu sıfırla
        newWeapon.transform.localRotation = Quaternion.Euler(0, 90, 0); // Y ekseninde 90 derece döndür
      //  playerAnimatorController.SetInteger("WeaponType_int", 10); // Ranged silah animasyonu
    }
    else if (currentGun.weaponType == Weapon.WeaponType.Melee)
    {
        newWeapon.transform.localPosition = new Vector3(0, -0.2f, 0.3f); // Melee silah için özel pozisyon
        newWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0); // Melee silah için düz tutuş
       // playerAnimatorController.SetInteger("WeaponType_int", 12); // Melee silah animasyonu
    }

    // Silahın kutusunu ve modelini ayarla
    Transform box = suankiSilah.transform.Find("Box");
    Transform gun = suankiSilah.transform.Find("Gun");

    if (box != null && gun != null)
    {
        box.gameObject.SetActive(false); // Kutu gizli
        gun.gameObject.SetActive(true); // Silah modeli görünür
    }

    Bar.gameObject.SetActive(true); // Bar'ı göster
    UpdateAmmoBar(); // Bar'ı güncelle
    }

    private void HandleWeaponDestruction()
    {
        if (suankiSilah != null && suankiSilah != DefaultBicak)
        {
            Destroy(suankiSilah); // Silahı yok et
            suankiSilah = null;
            currentGun = null;
        }

        // Varsayılan bıçağı donat
        EquipDefaultKnife();
    }

    private void UpdateAmmoBar()
    {
        if (ammoBar != null && currentGun != null)
        {
            ammoBar.value = (currentGun.weaponType == Weapon.WeaponType.Ranged) 
                ? (float)currentGun.stats.currentAmmo / currentGun.stats.maxAmmo 
                : (float)currentGun.stats.currentDurability / 100;
        }
    }
}
