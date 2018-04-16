using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NavigationControlPC : MonoBehaviour
{
    public GameEvent buildingSelected;
    public static NavigationControlPC instance = null;
    public List<Vector3> waypoints;
    public List<int> waypoint_buildingIndexes;
    public bool isSpawnMovable;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        Init();
    }


    void Init()
    {
        waypoints = new List<Vector3>();
        waypoint_buildingIndexes = new List<int>();
    }

    private void Update()
    {
        ListenToClicks();
    }

    
    private void ListenToClicks()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            ProcessRaycast(hit, false);
        }
    }

    public void ProcessRaycast(RaycastHit hit, bool isHolding)
    {

        if (hit.transform.parent.tag != "CampusBuildings")
            return;

        if (isHolding)
            EventManager.TriggerEvent("OpenBuildingInfo", hit.transform.gameObject);
        else
        {
            EventManager.TriggerEvent("BuildingSelected", hit.transform.gameObject);
            buildingSelected.Raise();
            Debug.Log("Event is raised");
        }
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
