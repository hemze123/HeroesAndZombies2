using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47Bullet : Bullet
{

    public int BulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnEnable() {
        EventManager.AddHandler(GameEvent.Ak47Upgrade,Ak47BulletUpgrade);
    }
   private void OnDisable() {
       EventManager.RemoveHandler(GameEvent.Ak47Upgrade,Ak47BulletUpgrade);
    }


    public void Ak47BulletUpgrade(){
        BulletDamage += 5;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

}