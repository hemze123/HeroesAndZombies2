using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "YeniSilah", menuName = "Əşyalar/Silah")]
public class WeaponItemSO : ItemBaseSO
{
    public Weapon.WeaponType weaponType;

    [Header("Prefablar")]
    [Tooltip("Silah prefabı (əlində və ya yerdə görünəcək)")]
    public GameObject weaponPrefab;

    [Header("VFX Prefablar")]
    [Tooltip("Lülə ağzı effekti (firePoint-də yaranacaq)")]
    public GameObject muzzleFlashPrefab;

    [Tooltip("Güllə izi (LineRenderer və ya TrailRenderer ilə)")]
    public GameObject bulletTrailPrefab;

    [Tooltip("Hədəfə dəymə effekti")]
    public GameObject impactEffectPrefab;


     [Header("Audio Faylları")]
    public AudioClip fireClip;
    public AudioClip reloadClip;
    public AudioClip swingClip; // yaxın döyüş üçün

    [Header("Animasiya (Ümumi)")]
    [Tooltip("SIMPLE Apocalypse Animator Controller 'WeaponType_Int' üçün ümumi ID (1-12). " +
             "Əgər aşağıdakı Idle/Attack ID-ləri > 0 deyilsə, bu dəyər istifadə olunur.")]
    public int weaponTypeAnimID = 0;

    [Header("Animasiya (Vəziyyətə görə dəyişikliklər)")]
    [Tooltip("Idle vəziyyətində istifadə ediləcək 'WeaponType_Int' dəyəri. 0 olarsa ümumi ID istifadə olunur.")]
    public int idleAnimId = 0;

    [Tooltip("Attack vəziyyətində istifadə ediləcək 'WeaponType_Int' dəyəri. 0 olarsa ümumi ID istifadə olunur.")]
    public int attackAnimId = 0;

    [Tooltip("Attack parametrindən neçə saniyə sonra yenidən Idle parametrinə qayıdılsın.")]
    public float attackToIdleDelay = 0.3f;

    [Header("Statistikalar")]
    public WeaponStats stats = new WeaponStats();

    [Header("Yüksəltmə")]
    [Tooltip("Başlanğıc səviyyəsi (1-dən başlayır)")]
    public int currentLevel = 1;

    [Tooltip("Maksimum səviyyə")]
    public int maxLevel = 5;

    [Tooltip("Hər səviyyə üçün yüksəltmə xərcləri (array uzunluğu = maxLevel - 1)")]
   public List<int> upgradeCosts = new List<int>();
}