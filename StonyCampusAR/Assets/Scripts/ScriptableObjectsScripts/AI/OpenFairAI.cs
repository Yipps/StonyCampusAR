using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/CampusEventAI/OpenFairAI")]
public class OpenFairAI : EventCoreAI
{

    public override void Init(StudentAIController ai)
    {
        ai.isOccupied = true;
        ai.agent.destination = campusEvent.eventPositions[Random.Range(0, campusEvent.eventPositions.Length)].position;
    }

    public override void Think(StudentAIController ai)
    {
        if (IsEventOver(ai))
            GoHome(ai);

        //Student is occupied if currently attending an event
        if (ai.isOccupied)
        {
            if (HasReachedDestination(ai))
                GoToRandomEventPos(ai);
        }
        else
        {
            if (HasReachedDestination(ai))
            {
                campusEvent.currentNumOfStudents--;
                Destroy(ai.gameObject);
            }
        }
    }

    public void GoToRandomEventPos(StudentAIController ai)
    {
        ai.agent.destination = campusEvent.eventPositions[Random.Range(0, campusEvent.eventPositions.Length)].position;
    }

    public bool IsEventOver(StudentAIController ai)
    {
        if (campusEvent.startPeriod + campusEvent.numOfPeriods == currentDay.currentPeriod || currentDay.currentPeriod == currentDay.maxPeriods)
            return true;
        return false;
    }

    public void GoHome(StudentAIController ai)
    {
        ai.isOccupied = false;
        ai.agent.destination = ai.homePosition;
    }

    public void OnDisable()
    { 
    }
}