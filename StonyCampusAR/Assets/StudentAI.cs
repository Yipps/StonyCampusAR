using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentAI : MonoBehaviour {

    private CrowdSystem cs;
    public Vector3[] schedule;
    public bool[] hasClass;
    private int currTarget;

    public NavMeshAgent agent;
    public bool moving;

    public float maxSpeed;
    public float normalSpeed;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cs = CrowdSystem.instance;
    }

    public void Init() {
        currTarget = 0;
        agent.destination = schedule[currTarget];
        //agent.speed = cs.normalSpeed;
        //agent.acceleration = cs.normalSpeed;
        moving = true;
    }

    void Update() {
        if (moving)
            if (!agent.pathPending && !agent.hasPath)
                CrowdSystem.instance.EnterClass(gameObject);
        //UpdateSpeed(cs.secondsLeftInPeriod);

    }

    public void NextTarget()
    { 
        currTarget++;
        Debug.Log(currTarget);
        if (currTarget == schedule.Length)
        {
            Object.Destroy(gameObject);
        }else
        agent.destination = schedule[currTarget];
    }

    //increase speed as 
    public void UpdateSpeed(float secondsLeftInPeriod)
    {
        float timeLeft = secondsLeftInPeriod / cs.secondsPerPeriod;

        if (timeLeft < .9)
            agent.speed =  normalSpeed + (maxSpeed - normalSpeed) * 0.25f;
        else if (timeLeft < .8)
            agent.speed = normalSpeed + (maxSpeed - normalSpeed) * 0.50f;
        else if (timeLeft < .75)
            agent.speed = maxSpeed;
        //agent.speed 
        
    }
}
