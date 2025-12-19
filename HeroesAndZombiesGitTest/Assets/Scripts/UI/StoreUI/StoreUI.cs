using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance { get; private set; }

    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemCardPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadItems()
    {
        ClearItems();
        
        foreach (var item in DataManager.Instance.GetAllItems())
        {

           
        if (item.hideInStore) 
            continue;
            Instantiate(itemCardPrefab, content).GetComponent<ItemCardUI>().Setup(item);
        }
    }

    private void ClearItems()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}