using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationControl : MonoBehaviour {

    public static NavigationControl instance = null;
    public List<Vector3> waypoints;
    public List<int> waypoint_buildingIndexes;
    private LineRenderer renderedPath;

	
	void Awake () {
        if (instance == null)   
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        Init();
    }
	
    public void ComputePath(List<Building> selectedBuildings)
    {
        if (selectedBuildings.Count < 1)
        {
            renderedPath.positionCount = 0;
        }
        else
        {
            NavMeshPath navPath = new NavMeshPath();
            waypoints.Clear();
            waypoint_buildingIndexes.Clear();
            waypoint_buildingIndexes.Add(0);

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

                waypoint_buildingIndexes.Add(waypoints.Count-1);

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
        waypoint_buildingIndexes = new List<int>();
        renderedPath = this.GetComponent<LineRenderer>();
        if (renderedPath == null)
        {
            renderedPath = this.gameObject.AddComponent<LineRenderer>();
            renderedPath.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            renderedPath.positionCount = 0;
        }
    }

    private void Update()
    {
        ListenToClicks();

        if (CrowdSystem.instance.isDayStarted )
            RedrawPath();
    }

    private void RedrawPath()
    {
        StudentAI playerAI = CrowdSystem.instance.GetPlayerAI();

        if (playerAI.agent.pathPending)
            return;

        NavMeshPath playerPath = CrowdSystem.instance.GetPlayerAgent().path;

        List<Vector3> updatedPath = new List<Vector3>();
        updatedPath.AddRange(playerPath.corners);

        int waypointIndex = waypoint_buildingIndexes[playerAI.currTarget];

        updatedPath.AddRange(waypoints.GetRange(waypointIndex, waypoints.Count - waypointIndex));
        //for (int x = waypoint_buildingIndexes[playerAI.currTarget]; x < renderedPath.positionCount; x++)
        //{
        //    updatedPath.Add(renderedPath.GetPosition(x));
        //}

        renderedPath.positionCount = updatedPath.Count;
        for (int i = 0; i < updatedPath.Count; i++)
        {
            renderedPath.SetPosition(i, updatedPath[i]);
        }
    }
    

    private void ListenToClicks()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Can't select buildings when day is started
            if (!CrowdSystem.instance.isDayStarted)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}
