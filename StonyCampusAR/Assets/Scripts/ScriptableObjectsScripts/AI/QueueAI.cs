using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/CampusEventAI/QueueAI")]
public class QueueAI : EventCoreAI
{
    public List<StudentAIController> studentAIs = new List<StudentAIController>();

    public override void Init(StudentAIController ai)
    {
        ai.isOccupied = true;
        //Used to init queue 
        if (studentAIs.Count <= campusEvent.maxNumOfStudents)
            studentAIs.Add(ai);

        ai.agent.destination = campusEvent.eventPositions[studentAIs.IndexOf(ai)].position;
    }

    public override void Think(StudentAIController ai)
    {
        //Student is occupied if currently attending an event
        if (ai.isOccupied)
        {
            //If event is over, student is no longer occupied and goes home
            if (IsEventOver(ai))
                LeaveLine(ai);

            //If student has reached the front of line, leave in x seconds
            if (HasReachedDestination(ai) && studentAIs.IndexOf(ai) == 0)
                ai.ToggleOccupied(5f);
        }
        else
        {
            //Student leaves and rest of the line moves up
            if (studentAIs.IndexOf(ai) == 0)
            {
                campusEvent.currentNumOfStudents--;
                MoveLineUp(ai);
            }

            
            if (HasReachedDestination(ai))
                Destroy(ai.gameObject);
        }
    }

    private void MoveLineUp(StudentAIController ai)
    {
        studentAIs.RemoveAt(0);
        LeaveLine(ai);
        foreach(StudentAIController i in studentAIs)
        {
            i.agent.destination = campusEvent.eventPositions[studentAIs.IndexOf(i)].position;
        }
    }

    public bool IsEventOver(StudentAIController ai)
    {
        if (campusEvent.startPeriod + campusEvent.numOfPeriods < currentDay.currentPeriod)
            return true;
        return false;
    }

    public void LeaveLine(StudentAIController ai)
    {
        ai.isOccupied = false;
        ai.agent.destination = ai.homePosition;
    }

    public void OnDisable()
    {
        studentAIs.Clear();
    }
}