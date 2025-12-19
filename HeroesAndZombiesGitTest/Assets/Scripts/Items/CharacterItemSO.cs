using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Items/Character")]
public class CharacterItemSO : ItemBaseSO
{
    public GameObject characterPrefab;
    public float speed;
    public float health;
}