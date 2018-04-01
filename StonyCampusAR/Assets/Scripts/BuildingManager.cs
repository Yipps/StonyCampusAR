using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance = null;

    public GameObject[] buildingGameObjects;

    public Dictionary<string, Building> buildingsDict;

    public List<Building> selectedBuildings;

    public Facility[] facilities;

    private List<string> departments;
    private List<string> organizations;

    public GameObject[] icons;

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
        //StartCoroutine(SpawnAllBuildings());
    }

    private void OnEnable()
    {
        EventManager.StartListening("BuildingSelected", BuildingSelected);
        EventManager.StartListening("OpenBuildingInfo", OpenBuildingInfo);
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
        buildingsDict = new Dictionary<string, Building>();
        selectedBuildings = new List<Building>();

        foreach (GameObject i in buildingGameObjects)
        {
            i.transform.GetChild(0).transform.localScale = Vector3.zero;

            Building building = i.AddComponent<Building>();
            Animator animator = i.transform.GetChild(0).gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load("Animations/spawnBuildingOverride") as RuntimeAnimatorController;
            building.animator = animator;
            buildingsDict.Add(building.name, building);
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
            if(buildingsDict.TryGetValue(i.buildingName, out building))
            {
                building.buildingName = i.buildingName;
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

            if(buildingsDict.TryGetValue(i.building,out building)){
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
        GameObject orgIcon = FindIcon(org);
        Building[] selected = FilterBuildingsByOrganization(org);

        foreach(Building i in selected)
        {
            i.ToggleIcon(orgIcon);
        }
    }

    //Helper method, returns an array of buildings containing a facilities of a certain organization
    public Building[] FilterBuildingsByOrganization(string org)
    {
        List<Building> filteredBuildings = new List<Building>();

        Building building = null;

        
        foreach (Facility i in facilities)
        {
            //Check facilities where org name are equal
            if(i.organization == org)
            {
                //check if the facility building exists
                if (buildingsDict.TryGetValue(i.building, out building))
                {
                    //check if the building is already in the list
                    if (!filteredBuildings.Contains(building))
                        filteredBuildings.Add(building);
                }
            }
        }

        return filteredBuildings.ToArray();
    }

    private GameObject FindIcon(string name)
    {
        foreach (GameObject i in icons)
            if (i.name == name)
                return i;

        Debug.Log("Icon " + name + " not found");
        return null;
    }

    private void OpenBuildingInfo(GameObject buildingGameobject)
    {
        Building building = buildingGameobject.GetComponentInParent<Building>();
        GameObject buildingInfo = Instantiate(Resources.Load("Prefabs/BuildingInfoWindow") as GameObject, GameObject.Find("Canvas").transform);
        buildingInfo.GetComponent<BuildingInfoGUI>().LoadInfo(building);
    }

    public static GameObject FindNearestBuilding(Vector3 targetPosition)
    {
        GameObject nearest = new GameObject();
        GameObject[] BuildingGameObjects = GameObject.FindGameObjectsWithTag("CampusBuildings");

        float closestDist = Mathf.Infinity;
        foreach (GameObject i in BuildingGameObjects)
        {
            Vector3 direction = targetPosition - i.transform.position;
            float dist = direction.sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                nearest = i;
            }
        }
        return nearest;
    }

    public IEnumerator SpawnAllBuildings()
    {
        yield return new WaitForSeconds(2f);
        foreach(GameObject i in buildingGameObjects)
        {
            i.GetComponent<Building>().SpawnAnimation();
            yield return new WaitForSeconds(0.3f);
        }
    }
}
