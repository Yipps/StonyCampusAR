using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/CampusEventAI/FollowLeaderAI")]
public class FollowLeaderAI : EventCoreAI {

	public override void Init(StudentAIController ai)
    {
        ai.isOccupied = true;
    }

    public override void Think(StudentAIController ai)
    {
        CheckIfEventOver(ai);

        if (ai.isOccupied)
            ai.agent.destination = campusEvent.eventPositions[0].position;
        else
            ai.agent.destination = ai.homePosition;
    }

    public void CheckIfEventOver(StudentAIController ai)
    {
        if (campusEvent.startPeriod + campusEvent.numOfPeriods < currentDay.currentPeriod)
            ai.isOccupied = false;
    }

    IEnumerator FollowLeader(StudentAIController ai)
    {
        yield return new WaitForSeconds(1f);
        ai.agent.destination = ai.homePosition;
    }
}
