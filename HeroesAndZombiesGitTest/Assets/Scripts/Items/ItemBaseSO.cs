using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ItemBase")]
public class ItemBaseSO : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    public int cost;

    
     [Header("Visibility")]
    [Tooltip("item magazada gorulmur")]
    public bool hideInStore;
    
    [Header("Unlock Status")]
    public bool isUnlocked;
    public bool isSelected;
    public bool isDefault;
    
    [Header("Item Type")]
    public ItemType itemType;

    public enum ItemType
    {
        Character,
        Weapon,
        Level
    }
}