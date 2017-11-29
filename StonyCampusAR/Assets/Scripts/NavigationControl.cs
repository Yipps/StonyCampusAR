﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationControl : MonoBehaviour {

    public static NavigationControl instance = null;

    public NavMeshAgent nav;
    public List<Vector3> waypoints;

    private LineRenderer renderedPath;
    

	
	void Awake () {
        if (instance == null)   
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        Init();
    }
	
    public void ComputePath(List<Buildings> selectedBuildings)
    {
        if (selectedBuildings.Count < 1)
        {
            renderedPath.positionCount = 0;
        }
        else
        {
            NavMeshPath navPath = new NavMeshPath();
            waypoints.Clear();

            for (int i = 0; i < selectedBuildings.Count - 1; i++)
            {
                Vector3 currentBuildingPos = selectedBuildings[i].transform.position;
                Vector3 nextBuildingPos = selectedBuildings[i + 1].transform.position;

                NavMeshHit currentSample;
                NavMeshHit nextSample;
                NavMesh.SamplePosition(currentBuildingPos, out currentSample, 10f, NavMesh.AllAreas);
                NavMesh.SamplePosition(nextBuildingPos, out nextSample, 10f, NavMesh.AllAreas);

                NavMesh.CalculatePath(currentSample.position, nextSample.position, NavMesh.AllAreas, navPath);
                waypoints.AddRange(navPath.corners);

            }


            renderedPath.positionCount = waypoints.Count;
            for (int i = 0; i < waypoints.Count; i++)
            {
                renderedPath.SetPosition(i, waypoints[i]);
            }
        }
    }

    void Init()
    {
        waypoints = new List<Vector3>();
        renderedPath = this.GetComponent<LineRenderer>();
        if (renderedPath == null)
        {
            renderedPath = this.gameObject.AddComponent<LineRenderer>();
            renderedPath.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            renderedPath.positionCount = 0;
        }
        nav.isStopped = true;
    }

    private void Update()
    {
        ListenToClicks();
    }
    

    private void ListenToClicks()
    {

        for (int i = 0; i < Input.touchCount; ++i)
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                Debug.Log(ray.ToString());
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                if (hit.transform && hit.transform.tag != "Road")
                {
                    EventManager.TriggerEvent("BuildingSelected", hit.transform.gameObject);
                }
            }


    }
}
