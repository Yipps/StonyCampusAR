using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class StudentAIController : MonoBehaviour {
    
    public CoreAI ai;
    public NavMeshAgent agent;

    [HideInInspector] public Vector3 homePosition;
    [HideInInspector] public bool isOccupied;
    [HideInInspector] public int periodOccupied;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        ai.Init(this);
    }

    void Update () {
        ai.Think(this);
	}

    private void OnDrawGizmosSelected()
    {
        if (agent)
        Gizmos.DrawWireSphere(agent.destination, 4f);
    }

}
