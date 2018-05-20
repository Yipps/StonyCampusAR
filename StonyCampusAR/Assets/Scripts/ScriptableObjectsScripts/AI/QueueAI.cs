using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Use ai.gameobject to start a coroutine/Invoke dequeue
[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/CampusEventAI/QueueAI")]
public class QueueAI : EventCoreAI
{
    public bool isDestoyedOnDequeue = true;
    public List<StudentAIController> studentAIs = new List<StudentAIController>();
    private bool eventOverFlag;

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
                GoHome(ai);

            //If student has reached the front of line, leave in x seconds
            if (HasReachedDestination(ai) && studentAIs.IndexOf(ai) == 0)
                ai.ToggleOccupied(3f);
        }
        else
        {
            //Student leaves and rest of the line moves up
            if (!IsEventOver(ai) && studentAIs.IndexOf(ai) == 0 )
            {
                MoveLineUp(ai);
            }


            //TEMP CHANGE
            if (isDestoyedOnDequeue && !eventOverFlag)
            {
                campusEvent.currentNumOfStudents--;
                Destroy(ai.gameObject);
            }
            else if (HasReachedDestination(ai))
            {
                campusEvent.currentNumOfStudents--;
                Destroy(ai.gameObject);
            }
        }
    }

    private void MoveLineUp(StudentAIController ai)
    {
        studentAIs.RemoveAt(0);
        //campusEvent.currentNumOfStudents--;
        GoHome(ai);
        foreach (StudentAIController i in studentAIs)
        {
            i.agent.destination = campusEvent.eventPositions[studentAIs.IndexOf(i)].position;
        }
    }

    public bool IsEventOver(StudentAIController ai)
    {
        if (campusEvent.startPeriod + campusEvent.numOfPeriods == currentDay.currentPeriod || currentDay.currentPeriod == currentDay.maxPeriods)
        {
            eventOverFlag = true;
            studentAIs.Clear();
            return true;
        }
        return false;
    }

    public void GoHome(StudentAIController ai)
    {
        ai.isOccupied = false;
        ai.agent.destination = ai.homePosition;
    }

    public void OnDisable()
    {
        studentAIs.Clear();
    }

    public void OnDestroy()
    {
        studentAIs.Clear();
    }
}