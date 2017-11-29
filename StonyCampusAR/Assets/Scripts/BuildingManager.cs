using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance = null;

    public GameObject[] buildingGameObjects;

    public List <Buildings> selectedBuildings;

    private void Awake()
    {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        Init();
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

        if (building.Selected())
        { 
        selectedBuildings.Add(building);
        }
        else
        {
            selectedBuildings.Remove(building);
        }

        NavigationControl.instance.ComputePath(selectedBuildings);
    }

    void Init()
    {
        selectedBuildings = new List<Buildings>();
        for (int i = 0; i < buildingGameObjects.Length; i++)
        {
            buildingGameObjects[i].AddComponent<Buildings>();
        }
    }



}
