using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
       CollectibleObject(collision);
       CollectWeapon(collision);
        
    }
    void CollectibleObject(Collision collision){
       ICollectible collectibleItems = collision.gameObject.GetComponent<ICollectible>();
        if (collectibleItems != null)
        {
            collectibleItems.CollectItem();
            Destroy(collision.gameObject);
        }
    }


    void CollectWeapon(Collision collision){
        Weapon weapon = collision.gameObject.GetComponent<Weapon>();
        if (weapon != null)
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }
}


