using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CampusEvents/Events")]
public class CampusEvent : ScriptableObject
{
    public EventCoreAI ai;
    public CampusEventsList campusEventList;

    public string eventName;
    public string description;
    public int numOfPeriods;
    public int startPeriod;
    public Transform[] eventPositions;

    public void OnEnable()
    {
        campusEventList.AddItem(this);
    }

    public void OnDisable()
    {
        campusEventList.RemoveItem(this);
    }
}


