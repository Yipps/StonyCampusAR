using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/CampusEventAI/FollowLeaderAI")]
public class QueueAI : EventCoreAI
{
    List<bool> queue = new List<bool>();

    public override void Init(StudentAIController ai)
    {
        ai.isOccupied = true;
        ai.currentIndex = queue.Count - 1;
        queue.Add(true);
    }

    public override void Think(StudentAIController ai)
    {
        CheckIfEventOver(ai);

        //if agent has reached a position on line
        if (ai.isOccupied && HasReachedDestination(ai))
        {
            if (ai.currentIndex == 0)
                ai.ToggleOccupied(3f);
            //Move up if the next space is free
            if (CanMoveUp(ai))
                ai.agent.destination = campusEvent.eventPositions[--ai.currentIndex].position;
        }
        else
        {
            LeaveLine(ai);
        }
           
    }

    public void CheckIfEventOver(StudentAIController ai)
    {
        if (campusEvent.startPeriod + campusEvent.numOfPeriods < currentDay.currentPeriod)
            ai.isOccupied = false;
    }

    public bool CanMoveUp(StudentAIController ai)
    {
        //If someone IS in front in queue return false
        if (ai.currentIndex == 0 || queue[ai.currentIndex - 1])
            return false;
        return true;
    }

    public void LeaveLine(StudentAIController ai)
    {
        ai.isOccupied = false;
        queue[0] = false;
    }

    public void MoveUp(StudentAIController ai)
    {

    }
}