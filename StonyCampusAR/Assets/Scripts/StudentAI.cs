using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentAI : MonoBehaviour {

    public CrowdSystem cs;
    public Vector3[] schedule;
    public bool[] hasClass;
    public int currTarget;

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
        agent.speed = cs.normalSpeed;
        normalSpeed = cs.normalSpeed;
        maxSpeed = cs.maxSpeed;
        agent.acceleration = cs.normalAcceleration;
        moving = true;
    }

    void Update() {
        if (moving)
            if (!agent.pathPending && !agent.hasPath)
                CrowdSystem.instance.EnterClass(gameObject);
        UpdateSpeed(cs.secondsLeftInPeriod);

    }

    //increase speed as 
    public void UpdateSpeed(float secondsLeftInPeriod)
    {
        float timeLeft = secondsLeftInPeriod / cs.secondsPerPeriod;
        agent.speed = normalSpeed + (maxSpeed - normalSpeed) * (1 - timeLeft);
        //if (timeLeft < .50)
        //    agent.speed = maxSpeed;
        //else if (timeLeft < .75)
        //    agent.speed = normalSpeed + (maxSpeed - normalSpeed) * 0.50f;
        //else if (timeLeft < .9)
        //    agent.speed =  normalSpeed + (maxSpeed - normalSpeed) * 0.25f;
        //Debug.Log(agent.speed);

        //agent.speed 

    }
}
