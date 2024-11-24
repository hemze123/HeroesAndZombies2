using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour, ICollectible
{
    public int medkitValue;
    public void CollectItem()
    {
        Collector.Instance.CollectMedKit(medkitValue);
        Debug.Log("MedKit");
    }
}
