using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public EnemyDataSO data;
    public GameObject coinPrefab;

    private int currentHealth;
    private float lastAttackTime;
    private Transform player;
    private NavMeshAgent agent;
    private Animator anim;     
    private bool isDead = false;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();   // 

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentHealth = data.maxHealth;
        agent.speed = data.moveSpeed;

        if (data.spawnClip != null)
            AudioManager.Instance.PlaySFX(data.spawnClip);
    }

private void Update()
{
    if (isDead) return;  //  Ölüduyse Update dayanir.

    if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
        player = GameObject.FindGameObjectWithTag("Player").transform;

    if (player != null)
        agent.SetDestination(player.position);  
}


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
{
    isDead = true;  //  Update artik passif

    if (agent != null)
        agent.enabled = false;

    Collider col = GetComponent<Collider>();
    if (col != null)
        col.enabled = false;

    ScoreManager.Instance.AddScore(data.scoreValue);

    if (coinPrefab && Random.value <= data.coinDropChance)
    {
        for (int i = 0; i < data.coinAmount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * 0.5f;
            pos.y = transform.position.y + 0.3f;
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
    }

    if (data.deathClip != null)
        AudioManager.Instance.PlaySFX(data.deathClip);

    if (anim != null)
        anim.SetTrigger("Die");

    Destroy(gameObject, 2f);
}


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= data.attackCooldown)
            {
                PlayerHealth.Instance.TakeDamage(data.damageToPlayer);

                if (data.attackClip != null)
                    AudioManager.Instance.PlaySFX(data.attackClip);

                lastAttackTime = Time.time;
            }
        }
    }
}
