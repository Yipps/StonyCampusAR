using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovementTest : MonoBehaviour {


    NavMeshAgent agent;
    public Transform destination;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
