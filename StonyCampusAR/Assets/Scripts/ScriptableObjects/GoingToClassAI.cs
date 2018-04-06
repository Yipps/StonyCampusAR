using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PluggableAI")]
public class GoingToClassAI : CoreAI {

    public CurrentDay currentDay;
    private float currentPeriod;

    public override void Init(StudentAIController ai)
    {
        currentPeriod = currentDay.currentPeriod;
        GoToNextTarget(ai);
    }

    public override void Think(StudentAIController ai)
    {
        //If ai isn't occupied check if it has reached its destination
        if (!ai.isOccupied)
        {
            if (!ai.agent.pathPending)
            {
                if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
                {
                    if (!ai.agent.hasPath || ai.agent.velocity.sqrMagnitude == 0f)
                    {
                        if(currentDay.currentPeriod == currentDay.maxPeriods)
                        {
                            Destroy(ai.gameObject);
                        }
                        else
                        {
                            EnterClass(ai);
                        }
                        
                    }
                }
            }
        }
        else
        {
            if (currentPeriod != currentDay.currentPeriod)
            {
                ExitClass(ai);
            }
        }
    }

    private void LeaveCampus(StudentAIController ai)
    {
        throw new NotImplementedException();
    }

    private void EnterClass(StudentAIController ai)
    {
        ai.isOccupied = true;
        ai.agent.enabled = false;
        ai.GetComponent<Renderer>().enabled = false;
    }

    private void GoToNextTarget(StudentAIController ai)
    {
        if (currentDay.currentPeriod == currentDay.maxPeriods)
            ai.agent.destination = ai.homePosition;
        else
            ai.agent.destination = BuildingManager.instance.GetRandomBuilding().GetComponent<Building>().GetNavPos();
    }

    public void ExitClass(StudentAIController ai)
    {
        ai.isOccupied = false;
        ai.agent.enabled = true;
        ai.GetComponent<Renderer>().enabled = true;
        GoToNextTarget(ai);
    }
}

