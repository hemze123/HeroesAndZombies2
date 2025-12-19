using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [SerializeField] private List<CharacterItemSO> characters;
    [SerializeField] private List<WeaponItemSO> weapons;
    [SerializeField] private List<LevelItemSO> levels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAllItemData()
    {
        foreach (var item in GetAllItems())
        {
            item.isUnlocked = SaveSystem.LoadItemStatus(item.itemName);
            
            string selectedItemName = SaveSystem.LoadSelectedItem(item.itemType);
            item.isSelected = (item.itemName == selectedItemName);
        }
    }

    public List<ItemBaseSO> GetAllItems()
    {
        List<ItemBaseSO> allItems = new List<ItemBaseSO>();
        allItems.AddRange(characters);
        allItems.AddRange(weapons);
        allItems.AddRange(levels);
        return allItems;
    }

    public CharacterItemSO GetSelectedCharacter()
    {
        return characters.Find(c => c.isSelected) ?? characters[0];
    }

    public WeaponItemSO GetDefaultWeapon()
    {
        return weapons.Find(w => w.isDefault) ?? weapons[0];
    }

    public List<WeaponItemSO> GetUnlockedWeapons()
    {
        return weapons.FindAll(w => w.isUnlocked);
    }

    public bool LevelExists(string sceneName)
    {
        return levels.Exists(level => level.sceneName == sceneName);
    }

    public LevelItemSO GetDefaultLevel()
    {
        return levels.Find(level => level.isDefault) ?? levels[0];
    }
}