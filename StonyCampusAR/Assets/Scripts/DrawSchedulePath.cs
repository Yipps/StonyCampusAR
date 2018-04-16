using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawSchedulePath : MonoBehaviour {

    public SelectedBuildingsList selectedBuildings;
    //private NavMeshAgent agent;
    public LineRenderer renderedPath;
    public List<Vector3> waypoints;
    public List<int> waypoint_buildingIndexes;

    private void Start()
    {
        //agent = this.GetComponent<NavMeshAgent>();
        selectedBuildings = Resources.Load("Runtime Data/SelectedBuildingsList") as SelectedBuildingsList;
        renderedPath = GetComponent<LineRenderer>();
        waypoints = new List<Vector3>();
        waypoint_buildingIndexes = new List<int>();
    }

    public void ComputePath()
    {
        Debug.Log("Computer Path");
        if (selectedBuildings.list.Count < 1)
        {
            renderedPath.positionCount = 0;
        }
        else
        {
            NavMeshPath navPath = new NavMeshPath();
            waypoints.Clear();
            waypoint_buildingIndexes.Clear();
            waypoint_buildingIndexes.Add(0);

            for (int i = 0; i < selectedBuildings.list.Count - 1; i++)
            {
                Vector3 currentBuildingPos = selectedBuildings.list[i].transform.position;
                Vector3 nextBuildingPos = selectedBuildings.list[i + 1].transform.position;

                NavMeshHit currentSample;
                NavMeshHit nextSample;
                NavMesh.SamplePosition(currentBuildingPos, out currentSample, 10f, NavMesh.AllAreas);
                NavMesh.SamplePosition(nextBuildingPos, out nextSample, 10f, NavMesh.AllAreas);

                NavMesh.CalculatePath(currentSample.position, nextSample.position, NavMesh.AllAreas, navPath);
                waypoints.AddRange(navPath.corners);

                waypoint_buildingIndexes.Add(waypoints.Count - 1);

            }
            renderedPath.positionCount = waypoints.Count;
            for (int i = 0; i < waypoints.Count; i++)
            {
                renderedPath.SetPosition(i, waypoints[i]);
            }
        }
    }

    //private void RedrawPath()
    //{

    //    if (agent.pathPending)
    //        return;

    //    NavMeshPath playerPath = agent.path;

    //    List<Vector3> updatedPath = new List<Vector3>();
    //    updatedPath.AddRange(playerPath.corners);

    //    //int waypointIndex = waypoint_buildingIndexes[playerAI.currTarget];
    //    int waypointIndex = 0;

    //    updatedPath.AddRange(waypoints.GetRange(waypointIndex, waypoints.Count - waypointIndex));

    //    renderedPath.positionCount = updatedPath.Count;
    //    for (int i = 0; i < updatedPath.Count; i++)
    //    {
    //        renderedPath.SetPosition(i, updatedPath[i]);
    //    }
    //}
}
