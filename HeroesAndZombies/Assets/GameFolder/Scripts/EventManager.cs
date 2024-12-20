using System;
using System.Collections.Generic;

public enum GameEvent
{
    Ak47Upgrade,
    Mp90Upgrade,
    AxeUpgrade,
    OnIncreaseHealthUI,
    OnDecreaseHealthUI,
    OnIncreaseCoinUI,
    OnDecreaseCoinUI
}

public static class EventManager
{
    private static Dictionary<GameEvent, Delegate> eventTable = new Dictionary<GameEvent, Delegate>();

    public static void AddHandler<T>(GameEvent gameEvent, Action<T> handler)
    {
        if (!eventTable.ContainsKey(gameEvent))
        {
            eventTable[gameEvent] = null;
        }

        eventTable[gameEvent] = (Action<T>)eventTable[gameEvent] + handler;
    }

    public static void RemoveHandler<T>(GameEvent gameEvent, Action<T> handler)
    {
        if (eventTable.ContainsKey(gameEvent))
        {
            eventTable[gameEvent] = (Action<T>)eventTable[gameEvent] - handler;
            if (eventTable[gameEvent] == null)
            {
                eventTable.Remove(gameEvent);
            }
        }
    }

    public static void Broadcast<T>(GameEvent gameEvent, T arg)
    {
        if (eventTable.ContainsKey(gameEvent) && eventTable[gameEvent] is Action<T> action)
        {
            action.Invoke(arg);
        }
    }
}
