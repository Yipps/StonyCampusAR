using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance = null;

    public GameObject[] buildingGameObjects;

    private List<Buildings> buildings;

    public List<Buildings> selectedBuildings;

    public Facilities[] facilities;

    public List<string> departments;
    public List<string> organizations;

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
        organizations = new List<string>();
        departments = new List<string>();
        buildings = new List<Buildings>();
        selectedBuildings = new List<Buildings>();
        for (int i = 0; i < buildingGameObjects.Length; i++)
        {
            buildings.Add(buildingGameObjects[i].AddComponent<Buildings>());
        }

        

        LoadBuildingData();
    }

    private void LoadBuildingData()
    {
        TextAsset buildingJson = Resources.Load<TextAsset>("CampusData");
        TextAsset facilityJson = Resources.Load<TextAsset>("FacilityData");
        BuildingData[] buildingData = JsonConvert.DeserializeObject<BuildingData[]>(buildingJson.text);
        facilities = JsonConvert.DeserializeObject<Facilities[]>(facilityJson.text);

        Debug.Log(facilities.Length);
        ///Inefficent
        foreach (BuildingData i in buildingData)
        {
            
            GameObject building = GameObject.Find(i.buildingName);
            building.GetComponent<Buildings>().buildingName = i.buildingName;
            //building.GetComponent<Buildings>().facilities.Add(new Facilities("etasdaw"));
        }
        foreach (Facilities i in facilities)
        {
            if (!organizations.Contains(i.organization))
                organizations.Add(i.organization);

            foreach (Buildings x in buildings)
            {
                if (x.buildingName == i.building)
                {
                    Debug.Log(x.buildingName + " : " + i.name);
                    List<Facilities> test = x.facilities;
                    test.Add(i);
                }

            }
        }
    }

    public void HighlightOrganization(string org)
    {
        foreach (Buildings x in buildings)
        {
            foreach(Facilities y in x.facilities)
            {
                if (org == y.organization)
                    x.Selected();
            }
        }
            
    }

}
