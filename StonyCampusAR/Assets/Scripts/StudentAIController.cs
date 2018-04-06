using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class StudentAIController : MonoBehaviour {
    public CoreAI ai;
    public NavMeshAgent agent;
    public bool isOccupied;
    public Vector3 homePosition;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        ai.Init(this);
    }

    void Update () {
        ai.Think(this);
        //remainDist = agent.remainingDistance;
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(agent.destination, 4f);
    }
}
