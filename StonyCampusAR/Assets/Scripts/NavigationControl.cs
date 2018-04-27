using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NavigationControl : MonoBehaviour
{
    public GameEvent buildingSelected;
    public static NavigationControl instance = null;
    public bool isBuildingsSelectable { set; get; }
    private float touchHoldTimer = 0;
    private RaycastHit hit;
    private bool isHeld;
    

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

    }

    private void Update()
    {
        ListenToClicks();
    }


    private void ListenToClicks()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            touchHoldTimer += Input.GetTouch(0).deltaTime;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                    isHeld = false;
                else
                    return;
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

    public void ProcessRaycast(RaycastHit hit, bool isHolding)
    {
        Debug.Log("Processing Raycast, isHolding :" + isHolding);

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
