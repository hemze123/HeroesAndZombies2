using System.Collections;
using UnityEngine;
using UnityEngine.AI;  // NavMesh kullanımı için

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private int damageToPlayer = 10;
    private bool canDamage = true;
    
    [SerializeField] private float moveSpeed = 3.5f;  // Düşmanın hızı
    private Transform player;
    private NavMeshAgent agent;

    [SerializeField] private EnemyType enemyType;  // Farklı düşman türleri

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        SetEnemyStats();  // Düşman türüne göre istatistik ayarla
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);  // Oyuncuyu takip et
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            StartCoroutine(DamageCooldown());
            Collector.Instance.PlayerTakeDamage(damageToPlayer);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        Destroy(gameObject);
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(1.0f);
        canDamage = true;
    }

    private void SetEnemyStats()
    {
        switch (enemyType)
        {
            case EnemyType.Fast:
                moveSpeed = 5.0f;
                maxHealth = 75;
                damageToPlayer = 5;
                break;
            case EnemyType.Strong:
                moveSpeed = 2.5f;
                maxHealth = 150;
                damageToPlayer = 15;
                break;
            case EnemyType.Standard:
            default:
                moveSpeed = 3.5f;
                maxHealth = 100;
                damageToPlayer = 10;
                break;
        }
        agent.speed = moveSpeed;
        currentHealth = maxHealth;
    }
}

public enum EnemyType
{
    Standard,
    Fast,
    Strong
}