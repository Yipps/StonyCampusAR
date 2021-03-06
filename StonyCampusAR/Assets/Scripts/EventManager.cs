﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


    [System.Serializable]
    public class Event : UnityEvent<GameObject> { }

    public class EventManager : MonoBehaviour
    {

        public static EventManager Instance;

        private Dictionary<string, Event> _eventDictionary;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _eventDictionary = new Dictionary<string, Event>();
            }
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public static void StartListening(string eventName, UnityAction<GameObject> listener)
        {
            Event thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new Event();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction<GameObject> listener)
        {
            if (Instance == null) return;
            Event thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName, GameObject arg)
        {
            Event thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(arg);
            }
        }
    }
