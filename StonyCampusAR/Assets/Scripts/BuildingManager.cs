using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : MonoBehaviour {

    GameObject[] buildings;

    public GameObject originBuilding;
    public GameObject destinationBuilding;

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        EventManager.StartListening("BuildingSelected", BuildingSelected);
    }

    private void OnDisable()
    {

    }

    void BuildingSelected(GameObject building)
    {
        originBuilding = building;
    }



}
