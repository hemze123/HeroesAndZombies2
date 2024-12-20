using System.Collections;
using UnityEngine;

public class WeaponUpgradeManager : MonoBehaviour
{
    // Her bir silah için varsayılan yükseltme miktarları
    public int ak47UpgradeAmount = 5;
    public int mp90UpgradeAmount = 4;
    public int axeUpgradeAmount = 3;

    // AK-47 için yükseltme işlemi
    public void Ak47Upgrade()
    {
        EventManager.Broadcast(GameEvent.Ak47Upgrade, ak47UpgradeAmount);
        Debug.Log($"AK-47 upgraded by {ak47UpgradeAmount}");
    }

    // MP90 için yükseltme işlemi
    public void Mp90Upgrade()
    {
        EventManager.Broadcast(GameEvent.Mp90Upgrade, mp90UpgradeAmount);
        Debug.Log($"MP90 upgraded by {mp90UpgradeAmount}");
    }

    // Balta (Axe) için yükseltme işlemi
    public void AxeUpgrade()
    {
        EventManager.Broadcast(GameEvent.AxeUpgrade, axeUpgradeAmount);
        Debug.Log($"Axe upgraded by {axeUpgradeAmount}");
    }
}
