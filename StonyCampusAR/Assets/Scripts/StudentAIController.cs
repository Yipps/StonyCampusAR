using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class StudentAIController : MonoBehaviour {
    
    public CoreAI ai;
    public NavMeshAgent agent;

    public Vector3 homePosition;
    public bool isOccupied;
    public int periodOccupied;
    public int currentIndex;

    private bool isTogglingOccupied;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        ai.Init(this);
    }

    void Update () {
        ai.Think(this);
	}

    public void ToggleOccupied(float i)
    {
        StartCoroutine(ToggleOccupiedCoroutine(i));
    }

    private IEnumerator ToggleOccupiedCoroutine(float i)
    {
        if (!isTogglingOccupied)
        {
            isTogglingOccupied = true;
            yield return new WaitForSeconds(i);
            isOccupied = !isOccupied;
            isTogglingOccupied = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (agent)
        Gizmos.DrawWireSphere(agent.destination, 2f);
    }

}
