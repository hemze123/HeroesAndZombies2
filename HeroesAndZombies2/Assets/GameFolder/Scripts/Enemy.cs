using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 10; // Oyuncuya verilen hasar
    private bool canDamage = true; // Hasar verme kontrolü

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            StartCoroutine(DamageCooldown());
            Collector.Instance.PlayerTakeDamage(damageToPlayer);
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false; // Hasar vermeyi devre dışı bırak
        yield return new WaitForSeconds(1.0f); // 1 saniye bekle
        canDamage = true; // Hasar vermeyi tekrar etkinleştir
    }
}
