using System;
using System.Collections.Generic;
using EventListener = System.Action<GameEvent>;
using EventListenerList = System.Collections.Generic.List<System.Action<GameEvent>>;

public class GameEventManager
{
    private static GameEventManager instance = null;
    public static GameEventManager Instance
    {
        get 
        {
            if (instance == null)
            { 
                instance = new GameEventManager();
            }
            return instance;
        }
    }

    private Dictionary<Type, EventListenerList> eventListeners = new Dictionary<Type, EventListenerList>();

    public void Update()
    { 
        
    }

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
