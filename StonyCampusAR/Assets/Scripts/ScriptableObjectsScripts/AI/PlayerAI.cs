using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/Player")]
public class PlayerAI : CoreAI
{
    public SelectedBuildingsList selectedBuildings;
    public CurrentDay currentDay;

    public override void Init(StudentAIController ai)
    {
        ai.isOccupied = true;
        GoToNextClass(ai);
    }

    public override void Think(StudentAIController ai)
    {
        //If ai isn't occupied check if it has reached its destination
        if (ai.isOccupied)
        {

            if (ai.periodOccupied != currentDay.currentPeriod)
            {
                ExitClass(ai);
            }

            if (ai.agent.enabled == true && HasReachedDestination(ai))
            {
                if (currentDay.currentPeriod == currentDay.maxPeriods)
                    Destroy(ai.gameObject);
                else
                    EnterClass(ai);
            }
        }
        else
        {
            if (ai.agent.enabled == true &&HasReachedDestination(ai))
                Destroy(ai.gameObject);
        }
    }

    private void EnterClass(StudentAIController ai)
    { 
        ai.agent.enabled = false;
        ai.GetComponent<Renderer>().enabled = false;
        ai.periodOccupied = currentDay.currentPeriod;
    }

    private void GoToNextClass(StudentAIController ai)
    {
        if (currentDay.currentPeriod == currentDay.maxPeriods || currentDay.currentPeriod + 1 > selectedBuildings.list.Count)
        {
            ai.agent.destination = ai.homePosition;
            ai.isOccupied = false;
        }
        else
            ai.agent.SetDestination(selectedBuildings.list[currentDay.currentPeriod].GetNavPos());
    }

    public void ExitClass(StudentAIController ai)
    {
        ai.agent.enabled = true;
        ai.GetComponent<Renderer>().enabled = true;
        GoToNextClass(ai);
    }
}

