using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeManager : MonoBehaviour
{
    
  public void Ak47Upgrade(){
     EventManager.Broadcast(GameEvent.Ak47Upgrade);
  }

  
  public void Mp90UpgradeUpgrade(){
     EventManager.Broadcast(GameEvent.Mp90Upgrade);
  }

  public void AxeUpgrade(){
   EventManager.Broadcast(GameEvent.AxeUpgrade);
  }
     
}
