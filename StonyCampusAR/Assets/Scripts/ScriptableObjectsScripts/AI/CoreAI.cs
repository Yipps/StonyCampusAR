using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreAI : ScriptableObject {

    public virtual void Init(StudentAIController ai) { }
    public abstract void Think(StudentAIController ai);

    public bool HasReachedDestination(StudentAIController ai)
    {
        if (!ai.agent.pathPending)
        {
            if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
            {
                if (!ai.agent.hasPath || ai.agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
