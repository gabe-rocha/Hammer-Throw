using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{
    private Dictionary<Data.Events, UnityEvent> simpleEventDictionary = new Dictionary<Data.Events, UnityEvent>();
    private Dictionary<Data.Events, UnityEvent<GameObject>> paramGOEventDictionary = new Dictionary<Data.Events, UnityEvent<GameObject>>();
    private Dictionary<Data.Events, UnityEvent<int>> paramIntEventDictionary = new Dictionary<Data.Events, UnityEvent<int>>();
    private Dictionary<Data.Events, UnityEvent<Vector3>> paramVec3EventDictionary = new Dictionary<Data.Events, UnityEvent<Vector3>>();

    public static EventManager Instance { get; private set; }

    private void Awake() {
            if (Instance == null) {
        Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    //========================
    public void StartListening(Data.Events eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        if (simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            simpleEventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StartListeningWithGOParam(Data.Events eventName, UnityAction<GameObject> listener)
    {
        UnityEvent<GameObject> thisParamEvent = null;
        if (paramGOEventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.AddListener(listener);
        }
        else
        {
            thisParamEvent = new UnityEvent<GameObject>();
            thisParamEvent.AddListener(listener);
            paramGOEventDictionary.Add(eventName, thisParamEvent);
        }
    }

    public void StartListeningWithIntParam(Data.Events eventName, UnityAction<int> listener)
    {
        UnityEvent<int> thisParamEvent = null;
        if (paramIntEventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.AddListener(listener);
        }
        else
        {
            thisParamEvent = new UnityEvent<int>();
            thisParamEvent.AddListener(listener);
            paramIntEventDictionary.Add(eventName, thisParamEvent);
        }
    }

    public void StartListeningWithVec3Param(Data.Events eventName, UnityAction<Vector3> listener)
    {
        UnityEvent<Vector3> thisParamEvent = null;
        if (paramVec3EventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.AddListener(listener);
        }
        else
        {
            thisParamEvent = new UnityEvent<Vector3>();
            thisParamEvent.AddListener(listener);
            paramVec3EventDictionary.Add(eventName, thisParamEvent);
        }
    }

    internal void TriggerEvent(object onHammerHitGround)
    {
        throw new NotImplementedException();
    }

    //========================
    public void StopListening(Data.Events eventName, UnityAction listener)
    {
        if (Instance == null) return;
        UnityEvent thisEvent = null;
        if (simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void StopListeningWithGOParam(Data.Events eventName, UnityAction<GameObject> listener)
    {
        if (Instance == null) return;
        UnityEvent<GameObject> thisParamEvent = null;
        if (paramGOEventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.RemoveListener(listener);
        }
    }
    public void StopListeningWithIntParam(Data.Events eventName, UnityAction<int> listener)
    {
        if (Instance == null) return;
        UnityEvent<int> thisParamEvent = null;
        if (paramIntEventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.RemoveListener(listener);
        }
    }

    public void StopListeningWithVec3Param(Data.Events eventName, UnityAction<Vector3> listener)
    {
        if (Instance == null) return;
        UnityEvent<Vector3> thisParamEvent = null;
        if (paramVec3EventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.RemoveListener(listener);
        }
    }


    //========================
    public void TriggerEvent(Data.Events eventName)
    {
        UnityEvent thisEvent = null;
        if (simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public void TriggerEventWithGOParam(Data.Events eventName, GameObject go)
    {
        UnityEvent<GameObject> thisParamEvent = null;
        if (paramGOEventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.Invoke(go);
        }
    }

    public void TriggerEventWithIntParam(Data.Events eventName, int i)
    {
        UnityEvent<int> thisParamEvent = null;
        if (paramIntEventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.Invoke(i);
        }
    }

    public void TriggerEventWithVec3Param(Data.Events eventName, Vector3 vec3)
    {
        UnityEvent<Vector3> thisParamEvent = null;
        if (paramVec3EventDictionary.TryGetValue(eventName, out thisParamEvent))
        {
            thisParamEvent.Invoke(vec3);
        }
    }
}