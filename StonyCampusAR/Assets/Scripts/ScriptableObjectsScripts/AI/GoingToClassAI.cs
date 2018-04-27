using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/GoingToClass")]
public class GoingToClassAI : CoreAI {

    public CurrentDay currentDay;
    public GoingToClassStudentsRuntimeList runtimeList;

    public override void Init(StudentAIController ai)
    {
        SetList(ai);
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
                            runtimeList.RemoveItem(ai.gameObject);
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
            if (ai.periodOccupied != currentDay.currentPeriod)
            {
                ExitClass(ai);
                GoToNextTarget(ai);
            }
        }
    }

    private void EnterClass(StudentAIController ai)
    {
        ai.periodOccupied = currentDay.currentPeriod;
        ai.isOccupied = true;
        ai.agent.enabled = false;
        ai.GetComponent<Renderer>().enabled = false;
    }

    private void GoToNextTarget(StudentAIController ai)
    {
        //If its the end of the day, leave campus
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
    }

    void SetList(StudentAIController ai)
    {
        runtimeList.AddItem(ai.gameObject);
    }
}

