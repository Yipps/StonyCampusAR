﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NavigationControlPC : MonoBehaviour
{
    public GameEvent buildingSelected;
    public static NavigationControlPC instance = null;
    public bool isBuildingsSelectable;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
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
            if(Physics.Raycast(ray, out hit))
                ProcessRaycast(hit, false);
        }
    }

    public void ProcessRaycast(RaycastHit hit, bool isHolding)
    {
        Debug.Log(hit.transform.gameObject.name);
        if (hit.transform.parent.tag == "CampusBuildings")
        {
            if (isHolding)
                EventManager.TriggerEvent("OpenBuildingInfo", hit.transform.gameObject);
            else if (isBuildingsSelectable)
            {
                Debug.Log("Building is selected");
                EventManager.TriggerEvent("BuildingSelected", hit.transform.gameObject);
                buildingSelected.Raise();
            }
        }
        
        if (hit.transform.tag == "CampusEventGUI")
        {
            hit.transform.parent.GetComponent<CampusEventGUI>().ToggleInfoGUI();
        }
        return;
       
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
