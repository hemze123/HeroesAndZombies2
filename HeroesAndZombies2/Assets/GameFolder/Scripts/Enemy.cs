using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int damageToPlayer = 10;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private Transform player;
    private NavMeshAgent agent;

    public event Action onDeath;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        onDeath?.Invoke(); // Spawn manager için takip
        Destroy(gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time - lastAttackTime >= attackCooldown)
        {
            Collector.Instance.PlayerTakeDamage(damageToPlayer);
            lastAttackTime = Time.time;
        }
    }
}
