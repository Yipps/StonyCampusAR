using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdSimulation/PluggableAI/Wanderer")]
public class WanderingAI : CoreAI
{

    public override void Init(StudentAIController ai)
    {
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
                        Destroy(ai.gameObject);
                    }
                }
            }
        }
    }

    public void GoToHome(StudentAIController ai)
    {
        ai.agent.destination = ai.homePosition;
    }
}
