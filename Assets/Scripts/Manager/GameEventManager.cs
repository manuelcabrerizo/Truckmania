using System;
using System.Collections.Generic;
using UnityEngine;
using EventListener = System.Action<GameEvent>;
using EventListenerList = System.Collections.Generic.List<System.Action<GameEvent>>;

public class GameEventManager : Singleton
{
    private Dictionary<Type, EventListenerList> eventListeners = new Dictionary<Type, EventListenerList>();
    public bool AddListener<Type>(EventListener listener) where Type : GameEvent
    {
        if (!eventListeners.ContainsKey(typeof(Type)))
        {
            eventListeners.Add(typeof(Type), new EventListenerList());
        }

        EventListenerList listeners = eventListeners[typeof(Type)];
        if (listeners.Contains(listener))
        {
            return false;
        }

        listeners.Add(listener);
        return true;
    }

    public bool RemoveListener<Type>(EventListener listener) where Type : GameEvent
    {        
        if (eventListeners.ContainsKey(typeof(Type)))
        {
            EventListenerList listeners = eventListeners[typeof(Type)];
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
                return true;
            }
        }
        return false;
    }

    public void TriggerEvent(GameEvent gameEvent)
    {
        Type eventType = gameEvent.GetType();
        if (eventListeners.ContainsKey(eventType))
        {
            EventListenerList listeners = eventListeners[eventType];
            // Loop backwards in case a listener from this list gets remove
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i](gameEvent);
            }
        }
    }
}
