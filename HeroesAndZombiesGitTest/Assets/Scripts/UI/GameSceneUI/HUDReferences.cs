using UnityEngine;
using UnityEngine.UI;

public class HUDReferences : MonoBehaviour
{
    public static HUDReferences Instance { get; private set; }

    [Header("Weapon UI")]
    public Slider ammoBar;
    public GameObject barRoot;

    private void Awake()
    {
        Instance = this; 
    }
}
