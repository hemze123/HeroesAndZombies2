using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var collectible = collision.gameObject.GetComponent<ICollectible>();
        if (collectible != null)
            collectible.CollectItem();
    }
}
