using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentAI : MonoBehaviour {

    public Vector3[] schedule;
    public NavMeshAgent agent;
    public bool moving;

    private int currTarget;

    // Use this for initialization
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Init() {
        currTarget = 0;
        agent.destination = schedule[currTarget];
        moving = true;
    }

    // Update is called once per frame
    void Update() {
        if (moving)
        
            if (!agent.pathPending && !agent.hasPath)
                CrowdSystem.instance.StartToggleStudent(gameObject, 3f);
        
    }

    public void NextTarget()
    { 
        currTarget++;
        Debug.Log(currTarget);
        if (currTarget == schedule.Length)
        {
            Debug.Log("die");
            Object.Destroy(gameObject);
        }
            
        agent.destination = schedule[currTarget];
    }
}
