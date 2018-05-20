using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawSchedulePath : MonoBehaviour {

    public SelectedBuildingsList selectedBuildings;
    public CurrentDay currentday;
    public PlayerData playerData;
    [HideInInspector] public bool isPlayerActive;
    private LineRenderer renderedPath;

    private List<Vector3> lineRenderPositions;
    private List<int> indexiesOfBuildings;
    
    private void Start()
    {
        selectedBuildings = Resources.Load("Runtime Data/SelectedBuildingsList") as SelectedBuildingsList;
        renderedPath = GetComponent<LineRenderer>();
        lineRenderPositions = new List<Vector3>();
        indexiesOfBuildings = new List<int>();
    }

    private void Update()
    {
        if (currentday.currentPeriod == currentday.maxPeriods || currentday.currentPeriod + 1 > selectedBuildings.list.Count)
            isPlayerActive = false;

        if(isPlayerActive)
            RedrawPath();
    }

    public void ComputePath()
    {
        //If only one building is selected no path rendering is needed
        
        //Used to calculate renderline positions with navmesh
        NavMeshPath navPath = new NavMeshPath();

        //Reset the position and indexies list to be recalculated
        lineRenderPositions.Clear();
        indexiesOfBuildings.Clear();

        //First building is at index 0
        //indexiesOfBuildings.Add(0);

        //Start path from spawn Pointer, calculate the path with navmesh and add list of corners to path list
        NavMeshHit initSpawnSample;
        NavMeshHit firstBuildingSample;
        NavMesh.SamplePosition(playerData.playerPointer.transform.position, out initSpawnSample, 10f, NavMesh.AllAreas);
        NavMesh.SamplePosition(selectedBuildings.list[0].transform.position, out firstBuildingSample, 10f, NavMesh.AllAreas);
        NavMesh.CalculatePath(initSpawnSample.position, firstBuildingSample.position, NavMesh.AllAreas, navPath);
        lineRenderPositions.AddRange(navPath.corners);
        indexiesOfBuildings.Add(lineRenderPositions.Count - 1);

        //Generate path between every 2 consecutive buildings and add it the list of render positions
        if (selectedBuildings.list.Count != 0)
        {
            for (int i = 0; i < selectedBuildings.list.Count - 1; i++)
            {
                Vector3 currentBuildingPos = selectedBuildings.list[i].transform.position;
                Vector3 nextBuildingPos = selectedBuildings.list[i + 1].transform.position;

                NavMeshHit currentSample;
                NavMeshHit nextSample;
                NavMesh.SamplePosition(currentBuildingPos, out currentSample, 10f, NavMesh.AllAreas);
                NavMesh.SamplePosition(nextBuildingPos, out nextSample, 10f, NavMesh.AllAreas);

                NavMesh.CalculatePath(currentSample.position, nextSample.position, NavMesh.AllAreas, navPath);
                lineRenderPositions.AddRange(navPath.corners);

                indexiesOfBuildings.Add(lineRenderPositions.Count - 1);
            }
        }

        //Set the list of render positions to the linerenderer to be shown
        renderedPath.positionCount = lineRenderPositions.Count;
        for (int i = 0; i < lineRenderPositions.Count; i++)
        {
            renderedPath.SetPosition(i, lineRenderPositions[i]);
        }
        
    }

    private void RedrawPath()
    {
        if (playerData.playerAgent == null)
        {
            Debug.Log("Agent not found for drawing path");
            isPlayerActive = false;
            return;
        }else if (playerData.playerAgent.pathPending)
        {
            return;
        }

        NavMeshPath playerPath = playerData.playerAgent.path;

        List<Vector3> updatedPath = new List<Vector3>();
        updatedPath.AddRange(playerPath.corners);

        int waypointIndex = indexiesOfBuildings[currentday.currentPeriod];

        updatedPath.AddRange(lineRenderPositions.GetRange(waypointIndex, lineRenderPositions.Count - waypointIndex));

        renderedPath.positionCount = updatedPath.Count;
        for (int i = 0; i < updatedPath.Count; i++)
        {
            renderedPath.SetPosition(i, updatedPath[i]);
        }
    }

}
