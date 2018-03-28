using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NavigationControl : MonoBehaviour
{

    public static NavigationControl instance = null;
    public List<Vector3> waypoints;
    public List<int> waypoint_buildingIndexes;
    public bool isSpawnMovable = true;
    public Transform spawnPoint;
    private LineRenderer renderedPath;
    private float touchHoldTimer = 0;
    private RaycastHit hit;
    private Touch touch;
    private bool isHeld;
    

    void Awake()
    {
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

                waypoint_buildingIndexes.Add(waypoints.Count - 1);

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
        hit = new RaycastHit();
        if (spawnPoint == null) spawnPoint = new GameObject().transform;
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

        if (CrowdSystem.instance.isDayStarted)
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

        renderedPath.positionCount = updatedPath.Count;
        for (int i = 0; i < updatedPath.Count; i++)
        {
            renderedPath.SetPosition(i, updatedPath[i]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(spawnPoint.position, 5f);
    }

    private void ListenToClicks()
    {
        if (Input.touchCount > 0 && !IsPointerOverUIObject())
        {
            if (isSpawnMovable)
            {
                Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    spawnPoint.Translate(touch.deltaPosition.x * 0.1f, 0, touch.deltaPosition.y * 0.1f, Space.World);
                    
                    Debug.Log(touch.deltaPosition.x * 0.5f);
                }
            }
            else
            {
                touch = Input.GetTouch(0);
                touchHoldTimer += Input.GetTouch(0).deltaTime;

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    Physics.Raycast(ray, out hit);
                    isHeld = false;
                }
                if (touchHoldTimer > 1f)
                {
                    touchHoldTimer = 0;
                    isHeld = true;
                    ProcessRaycast(hit, true);
                    touch.phase = TouchPhase.Ended;
                }
                else if (touch.phase == TouchPhase.Ended && !isHeld)
                {
                    touchHoldTimer = 0;
                    ProcessRaycast(hit, false);
                }
            }
        }
    }

    public void ProcessRaycast(RaycastHit hit, bool isHolding)
    {

        if (hit.transform.parent.tag != "CampusBuildings")
            return;

        if (isHolding)
            EventManager.TriggerEvent("OpenBuildingInfo", hit.transform.gameObject);
        else
            EventManager.TriggerEvent("BuildingSelected", hit.transform.gameObject);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
