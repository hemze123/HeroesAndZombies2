using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour, ICollectible
{
    public int coinValue = 1;
    public ParticleSystem collectEffect;
    public AudioClip collectSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            CollectItem();
    }

    public void CollectItem()
    {
        CurrencyManager.Instance.AddCoins(coinValue);

        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        Destroy(gameObject);
    }
}
