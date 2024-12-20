using UnityEngine;

public class Ak47Bullet : Bullet
{
    public int BulletDamage = 10; // Varsayılan mermi hasarı

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

    private void UpgradeBulletDamage(int upgradeAmount)
    {
        BulletDamage += upgradeAmount; // Hasar değerini gelen parametre kadar artır
        Debug.Log($"AK-47 Bullet Damage upgraded by {upgradeAmount}. New Damage: {BulletDamage}");
    }
}
