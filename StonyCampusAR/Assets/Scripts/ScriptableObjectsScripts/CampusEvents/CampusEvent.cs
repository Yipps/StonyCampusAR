using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CampusEvents/Events")]
public class CampusEvent : ScriptableObject
{
    public EventCoreAI ai;
    public CampusEventsList campusEventList;
    public int numOfPeriods;
    public int maxNumOfStudents;

    public float agentSpeed;

    public int startPeriod;
     public int currentNumOfStudents;
    [HideInInspector] public Transform[] eventPositions;

    public void OnDisable()
    {
        currentNumOfStudents = 0;
    }
}


