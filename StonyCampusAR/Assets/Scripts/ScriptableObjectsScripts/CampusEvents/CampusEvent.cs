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
    public int maxNumOfStudents;

    public float agentSpeed;

    [HideInInspector] public int startPeriod;
    [HideInInspector] public int currentNumOfStudents;
    [HideInInspector] public Transform[] eventPositions;

    public void OnEnable()
    {
        campusEventList.AddItem(this);
    }

    public void OnDisable()
    {
        campusEventList.RemoveItem(this);
        currentNumOfStudents = 0;
    }
}


