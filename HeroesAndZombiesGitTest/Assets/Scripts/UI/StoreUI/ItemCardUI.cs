using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button actionButton;

    private ItemBaseSO itemData;

    public void Setup(ItemBaseSO data)
    {
        itemData = data;
        icon.sprite = data.icon;
        nameText.text = data.itemName;
        costText.text = data.cost.ToString();

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (itemData is WeaponItemSO weaponData)
        {
            levelText.text = weaponData.currentLevel >= weaponData.upgradeCosts.Count
                ? "MAX"
                : "Lv." + weaponData.currentLevel;
        }
        else
        {
            levelText.text = string.Empty;
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (!itemData.isUnlocked)
        {
            actionButton.GetComponentInChildren<TMP_Text>().text = "UNLOCK";
            actionButton.interactable = true;
            return;
        }

        if (itemData is WeaponItemSO weapon)
        {
            if (weapon.currentLevel >= weapon.upgradeCosts.Count)
            {
                actionButton.GetComponentInChildren<TMP_Text>().text = "MAX";
                actionButton.interactable = false;
            }
            else
            {
                actionButton.GetComponentInChildren<TMP_Text>().text = "UPGRADE";
                actionButton.interactable = true;
            }
        }
        else
        {
            actionButton.GetComponentInChildren<TMP_Text>().text = "SELECT";
            actionButton.interactable = true;
        }
    }

    public void OnActionButtonClick()
    {
        AudioManager.Instance.PlayUIClick(); // 🔊 Buton sesi

        if (!itemData.isUnlocked)
        {
            TryUnlockItem();
        }
        else if (itemData is WeaponItemSO)
        {
            TryUpgradeWeapon();
        }
        else
        {
            SelectItem();
        }
    }

    private void TryUnlockItem()
    {
        if (CurrencyManager.Instance.HasEnoughCoins(itemData.cost))
        {
            CurrencyManager.Instance.SpendCoins(itemData.cost);
            itemData.isUnlocked = true;
            SaveSystem.SaveItemStatus(itemData.itemName, true);
            RefreshUI();
        }
        else
        {
            // AudioManager.Instance.PlaySFX(errorClip);
            // UIManager.Instance.ShowError("Not enough coins!");
        }
    }

    private void TryUpgradeWeapon()
    {
        WeaponItemSO weapon = itemData as WeaponItemSO;

        if (weapon.currentLevel >= weapon.upgradeCosts.Count)
        {
            RefreshUI();
            return;
        }

        int upgradeCost = weapon.upgradeCosts[weapon.currentLevel];

        if (CurrencyManager.Instance.HasEnoughCoins(upgradeCost))
        {
            CurrencyManager.Instance.SpendCoins(upgradeCost);
            weapon.currentLevel++;
            SaveSystem.SaveWeaponLevel(weapon.itemName, weapon.currentLevel);
            ApplyUpgradeStats(weapon);
            RefreshUI();
        }
    }

    private void ApplyUpgradeStats(WeaponItemSO weapon)
    {
        // Upgrade sonrası istatistik artışları buraya.
        // Örn: weapon.damage += 5;
    }

    private void SelectItem()
    {
        foreach (var item in DataManager.Instance.GetAllItems())
        {
            if (item.itemType == itemData.itemType)
                item.isSelected = false;
        }

        itemData.isSelected = true;
        SaveSystem.SaveSelectedItem(itemData.itemName, itemData.itemType);
        RefreshUI();
    }
}
