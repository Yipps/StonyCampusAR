using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : StudentAI {

    // Use this for initialization
    private void Update()
    {
        if (moving)
            if (!agent.pathPending && !agent.hasPath)
            {
                //CrowdSystem.instance.EnterClass(gameObject);
                BuildingManager.instance.selectedBuildings[currTarget].Selected();
            }
        UpdateSpeed(cs.secondsLeftInPeriod);
    }
}
