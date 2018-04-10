using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeaderAI : EventCoreAI {

	public override void Init(StudentAIController ai)
    {
        ai.isOccupied = true;
    }

    public override void Think(StudentAIController ai)
    {
        if (ai.isOccupied)
            ai.agent.destination = campusEvent.eventPositions[0].position;
        else
            ai.agent.destination = ai.homePosition;
    }
}
