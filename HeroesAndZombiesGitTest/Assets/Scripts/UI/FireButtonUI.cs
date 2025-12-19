// FireButtonUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireButtonUI : MonoBehaviour
{
    private EventTrigger eventTrigger;
    private PlayerController playerController;

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null)
            eventTrigger = gameObject.AddComponent<EventTrigger>();
    }

    public void BindToPlayer(PlayerController player)
    {
        playerController = player;

        eventTrigger.triggers.Clear();

        AddTrigger(EventTriggerType.PointerDown, () => playerController.OnPointerDown());
        AddTrigger(EventTriggerType.PointerUp, () => playerController.OnPointerUp());
    }

    private void AddTrigger(EventTriggerType type, System.Action callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((data) => callback.Invoke());
        eventTrigger.triggers.Add(entry);
    }
}
