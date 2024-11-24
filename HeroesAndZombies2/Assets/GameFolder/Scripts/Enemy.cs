using UnityEngine;

public class Enemy : MonoBehaviour // ICollectible arayüzünü uygular
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Düşmanın ölmesi için gerekli işlemleri yap (animasyon, yok olma vb.)
        Debug.Log("Düşman öldü!");
        Destroy(gameObject);
    }
    
}
