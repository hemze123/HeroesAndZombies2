using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [Header("Basic Settings")]
    public string weaponName;
    public int baseDamage = 10;
    public float baseFireRate = 0.5f;

    [Header("Ammo Settings")]
    public int maxAmmo = 30;
    public int currentAmmo = 30;
    public bool infiniteAmmo = false;

    [Header("Durability Settings")]
    public int maxDurability = 100;
    public int currentDurability = 100;
    public bool infiniteDurability = false;
    public int upgradeDurabilityBonus = 20;

    [Header("Raycast Settings (Ranged Only)")]
    public float range = 100f;
    public Color rayColor = Color.red;
    public float rayDuration = 0.1f;

    [Header("Melee Settings")]
    public float attackRadius = 2f;
    public float attackAngle = 60f;

    [Header("Upgrade Paths")]
    public List<UpgradePath> upgradePaths = new();
    [HideInInspector] public int currentUpgradeLevel = 0;
}
