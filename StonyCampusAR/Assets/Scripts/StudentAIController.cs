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
    [HideInInspector] public int currentIndex;

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
        yield return new WaitForSeconds(i);
        isOccupied = !isOccupied;
    }

    private void OnDrawGizmosSelected()
    {
        if (agent)
        Gizmos.DrawWireSphere(agent.destination, 4f);
    }

}
