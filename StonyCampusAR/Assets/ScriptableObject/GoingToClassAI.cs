using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GoingToClassAI : CoreAI {

    StudentAIController controller;

    public override void Init(StudentAIController ai)
    {
        controller = ai;
        ai.agent.destination = RandBuilding().GetNavPos();
        ai.isActive = true;
    }

    public override void Think(StudentAIController ai)
    {
        if (controller.isActive)
            if (!controller.agent.pathPending && !controller.agent.hasPath)
                CrowdSystem.instance.EnterClass(controller.gameObject);
    }

    private Building RandBuilding()
    {
        GameObject[] buildings = BuildingManager.instance.buildingGameObjects;
        GameObject rand = buildings[Random.Range(0, buildings.Length)];
        return rand.GetComponent<Building>();
    }

    private void EnterClass()
    {
        controller.isActive = false;
        controller.agent.isStopped = true;
        controller.GetComponent<Renderer>().enabled = false;

    }

    private void ExitClass()
    {

    }

}
