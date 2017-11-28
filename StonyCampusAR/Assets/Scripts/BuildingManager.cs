using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : MonoBehaviour {
    
    public GameObject[] buildingGameObjects;

    List <Buildings> selectedBuildings;

    private void Awake()
    {
        selectedBuildings = new List<Buildings>();
        for (int i = 0; i < buildingGameObjects.Length; i++)
        {
            buildingGameObjects[i].AddComponent<Buildings>();
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("BuildingSelected", BuildingSelected);
    }

    private void OnDisable()
    {

    }

    void BuildingSelected(GameObject buildingObject)
    {
        Buildings building = buildingObject.GetComponentInParent<Buildings>();
        selectedBuildings.Add(building);
        building.Selected();
    }



}
