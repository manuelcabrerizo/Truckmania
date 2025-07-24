using System;
using System.Collections.Generic;
using UnityEngine;
using EventListener = System.Action<GameEvent>;
using EventListenerList = System.Collections.Generic.List<System.Action<GameEvent>>;

public struct ListenerToRemove
{ 
    public Type type;
    public EventListener listerner;
}

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance = null;

    private Dictionary<Type, EventListenerList> eventListeners = new Dictionary<Type, EventListenerList>();
    
    private List<ListenerToRemove> listenersToRemove = new List<ListenerToRemove>();

    //private Queue<BaseGameEvent>[] eventQueues = new Queue<BaseGameEvent>[2];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (listenersToRemove.Count > 0)
        {
            foreach (var toRemove in listenersToRemove)
            {
                if (eventListeners.ContainsKey(toRemove.type))
                {
                    EventListenerList listenes = eventListeners[toRemove.type];
                    listenes.Remove(toRemove.listerner);
                }
            }
            listenersToRemove.Clear();
        }
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
            Debug.Log( "Listener Already register");
            return false;
        }

        listeners.Add(listener);
        return true;
    }

    public bool RemoveListener<Type>(EventListener listener) where Type : GameEvent
    {        
        if (eventListeners.ContainsKey(typeof(Type)))
        {
            ListenerToRemove toRemove = new ListenerToRemove();
            toRemove.type = typeof(Type);
            toRemove.listerner = listener;
            listenersToRemove.Add(toRemove);
            return true;
        }
        return false;
    }

    public void TriggerEvent(GameEvent gameEvent)
    {
        //int eventType = gameEvent.GetID();
        Type eventType = gameEvent.GetType();
        if (eventListeners.ContainsKey(eventType))
        {
            EventListenerList listeners = eventListeners[eventType];
            foreach (EventListener listener in listeners)
            {
                listener(gameEvent);
            }
        }
    }
}
