﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour {

    public Camera cam;
    public NavMeshAgent nav;


	// Use this for initialization
	void Start () {
        cam = Camera.main;
        //nav = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame

}
