﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CampusEventSystem : MonoBehaviour {

    public List<GameObject> allEvents;
    public CurrentDay currentDay;

	// Use this for initialization
	void Start () {
        allEvents = Resources.LoadAll<GameObject>("Prefabs/Campus Events").ToList<GameObject>();
	}
	
    public void CheckToSpawnEvent()
    {
        if ((currentDay.currentPeriod - 1) % 2 == 0 && currentDay.currentPeriod != currentDay.maxPeriods)
        {
            SpawnEvent();
        }
            
    }

    public void SpawnEvent()
    {
        GameObject randomEvent = allEvents[Random.Range(0, allEvents.Count)];
        allEvents.Remove(randomEvent);
        Instantiate(randomEvent,transform);
    }
}
