using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance = null;

    public GameObject[] buildingGameObjects;

    private Dictionary<string, Building> buildings;

    public List<Building> selectedBuildings;

    public Facility[] facilities;

    private List<string> departments;
    private List<string> organizations;

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
        Building building = buildingObject.GetComponentInParent<Building>();

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
        organizations = new List<string>();
        departments = new List<string>();
        buildings = new Dictionary<string, Building>();
        selectedBuildings = new List<Building>();

        foreach (GameObject i in buildingGameObjects)
        {
            Building building = i.AddComponent<Building>();
            buildings.Add(building.name, building);
        }

        LoadBuildingData();

        LoadFacilityData();
    }

    private void LoadBuildingData()
    {
        TextAsset buildingJson = Resources.Load<TextAsset>("CampusData");
        
        BuildingData[] buildingData = JsonConvert.DeserializeObject<BuildingData[]>(buildingJson.text);

        Building building = null;
        foreach (BuildingData i in buildingData)
        {
            if(buildings.TryGetValue(i.buildingName, out building))
            {
                building.name = i.buildingName;
            }
            else
            {
                Debug.Log("Could not find building " + i.buildingName + " while loading building data");
            }

        }
    }

    private void LoadFacilityData()
    {
        TextAsset facilityJson = Resources.Load<TextAsset>("FacilityData");
        facilities = JsonConvert.DeserializeObject<Facility[]>(facilityJson.text);

        Building building;

        foreach(Facility i in facilities)
        {
            if (!organizations.Contains(i.organization))
                organizations.Add(i.organization);

            if(buildings.TryGetValue(i.building,out building)){
                building.facilities.Add(i);
            }
            else
            {
                Debug.Log("Could not find building " + i.building + " to add facility " + i.name + " while adding facility data");
            }
        }
    }

    public void HighlightOrganization(string org)
    {
        Building[] selected = FilterBuildings(org);
        foreach(Building i in selected)
        {
            i.Selected();
        }
    }

    public Building[] FilterBuildings(string org)
    {
        List<Building> filteredBuildings = new List<Building>();

        Building building = null;

        foreach (Facility i in facilities)
        {
            if(i.organization == org)
            {
                if (buildings.TryGetValue(i.building, out building))
                {
                    filteredBuildings.Add(building);
                }
            }
        }

        return filteredBuildings.ToArray();
    }

}
