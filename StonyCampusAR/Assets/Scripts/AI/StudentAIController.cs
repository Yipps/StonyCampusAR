using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentAIController : MonoBehaviour {

    public CoreAI ai;
    public NavMeshAgent agent;
    public bool isActive;


    private void OnEnable()
    {
        ai.Init(this);
    }

    void Update () {
        ai.Think(this);
	}
}
