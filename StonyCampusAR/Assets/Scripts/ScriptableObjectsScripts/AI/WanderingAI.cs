using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/Wanderer")]
public class WanderingAI : CoreAI
{
    public WanderingStudentsRuntimeList runtimeList;

    public override void Init(StudentAIController ai)
    {
        SetList(ai);
        ai.isOccupied = false;
        GoToHome(ai);
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
                        runtimeList.RemoveItem(ai.gameObject);
                        Destroy(ai.gameObject);
                    }
                }
            }
        }
    }

    void GoToHome(StudentAIController ai)
    {
        ai.agent.destination = ai.homePosition;
    }

    void SetList(StudentAIController ai)
    {
        runtimeList.AddItem(ai.gameObject);
    }
}
