using UnityEngine;

public class Ak47Bullet : MonoBehaviour
{
    public int bulletDamage = 10; // Varsayılan mermi hasarı

    private void OnEnable()
    {
        // Ak47Upgrade olayına abone ol
        EventManager.AddHandler<int>(GameEvent.Ak47Upgrade, UpgradeBulletDamage);
    }

    private void OnDisable()
    {
        // Ak47Upgrade olayından aboneliği kaldır
        EventManager.RemoveHandler<int>(GameEvent.Ak47Upgrade, UpgradeBulletDamage);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
            Destroy(gameObject);  // Mermi yok edilir
        }
    }


    private void UpgradeBulletDamage(int upgradeAmount)
    {
        bulletDamage += upgradeAmount; // Hasar değerini gelen parametre kadar artır
        Debug.Log($"AK-47 Bullet Damage upgraded by {upgradeAmount}. New Damage: {bulletDamage}");
    }
}
