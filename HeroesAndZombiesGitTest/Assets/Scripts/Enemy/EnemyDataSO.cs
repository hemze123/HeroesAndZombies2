using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int damageToPlayer = 10;
    public float attackCooldown = 1f;
    public float moveSpeed = 3.5f;
    public int scoreValue = 10;

    [Header("Loot")]
    [Range(0f, 1f)] public float coinDropChance = 0.5f;
    public int coinAmount = 1;

    [Header("Audio Clips")]
    public AudioClip spawnClip;
    public AudioClip attackClip;
    public AudioClip deathClip;
}
