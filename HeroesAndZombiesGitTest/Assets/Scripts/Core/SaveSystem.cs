using UnityEngine;

public static class SaveSystem
{
    private const string CoinsKey = "PlayerCoins";
    private const string SelectedSuffix = "_Selected";
    private const string LevelSuffix = "_Level";

    public static void SaveCoins(int amount)
    {
        PlayerPrefs.SetInt(CoinsKey, amount);
        PlayerPrefs.Save();
    }

    public static int LoadCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, 100);
    }

    public static void SaveItemStatus(string itemName, bool isUnlocked)
    {
        PlayerPrefs.SetInt(itemName + "_Unlocked", isUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool LoadItemStatus(string itemName)
    {
        return PlayerPrefs.GetInt(itemName + "_Unlocked", 0) == 1;
    }

    public static void SaveSelectedItem(string itemName, ItemBaseSO.ItemType itemType)
    {
        string typeKey = itemType.ToString() + SelectedSuffix;
        PlayerPrefs.SetString(typeKey, itemName);
        PlayerPrefs.Save();
    }

    public static string LoadSelectedItem(ItemBaseSO.ItemType itemType)
    {
        string typeKey = itemType.ToString() + SelectedSuffix;
        return PlayerPrefs.GetString(typeKey, "");
    }

    public static void SaveWeaponLevel(string weaponName, int level)
    {
        PlayerPrefs.SetInt(weaponName + LevelSuffix, level);
        PlayerPrefs.Save();
    }

    public static int LoadWeaponLevel(string weaponName)
    {
        return PlayerPrefs.GetInt(weaponName + LevelSuffix, 1);
    }
}