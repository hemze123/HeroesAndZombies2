using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour, ICollectible
{
    public int coinValue = 1;
    public ParticleSystem collectEffect; // Toplanma efekti
    public AudioClip collectSound; // Toplanma sesi

    private Collector _collector;

    [Inject]
    public void Construct(Collector collector)
    {
        _collector = collector;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    public void CollectItem()
    {
        _collector.CollectCoin(coinValue);
        Debug.Log($"Collected coin worth {coinValue}!");

        // Efekt ve ses oynat
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        Destroy(gameObject); // Toplandıktan sonra coin'i yok et
    }
}