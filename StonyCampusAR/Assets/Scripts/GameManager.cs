using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase { Initalizing, Planning, Simulating };

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    BuildingManager _buildingManager;
    CrowdSystem _crowdSystem;
    NavigationControl _navControl;
    Gps _gps;

    public bool skipIntro;
    public GamePhase gamePhase;

    public bool isFirstTargetFound;
    public bool isTouchHoldTutDone;
    public bool isTouchTutDone;
    public bool isSwipeTutDone;

    private bool hasPlayedIntro;
    private bool hasSpawnedCampus;

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
    }

    void Start () {
        Init();
    }

    //Triggered by vuforia tracking found
    public IEnumerator StartIntro()
    {
        //Welcome Animation
        GameObject[] animations = GameObject.FindGameObjectsWithTag("IntroAnimation");
        yield return new WaitForSeconds(1f);
        animations[0].GetComponent<Animator>().SetTrigger("spawn");
        yield return new WaitForSeconds(3f);
        animations[1].GetComponent<Animator>().SetTrigger("spawn");
        yield return new WaitForSeconds(3f);
        hasPlayedIntro = true;

    }

    private void Init()
    {
        gamePhase = GamePhase.Initalizing;
        _buildingManager = BuildingManager.instance;
        _crowdSystem = CrowdSystem.instance;
        _navControl = NavigationControl.instance;
        _gps = GetComponent<Gps>();

        if (skipIntro)
        {
            gamePhase = GamePhase.Planning;
            hasPlayedIntro = true;
            isTouchHoldTutDone = true;
        }
    }
	
    //TODO If location is found check if location is on campus
    private void InitSpawnSelection(bool isValid)
    {
        if (isValid)
        {
            GameObject spawnPointer = GameObject.Instantiate(Resources.Load("Prefabs/SpawnPointer") as GameObject, transform.GetChild(0));
            Vector3 pos = _gps.PingMap();
            //TEST
            pos = _gps.PingMapTest(-73.124995f, 40.915595f);
            //TEST
            spawnPointer.transform.localPosition = pos;
            _navControl.isSpawnMovable = true;
            _navControl.spawnPoint = spawnPointer.transform;

            GameObject closestBuilding = BuildingManager.FindNearestBuilding(spawnPointer.transform.position);
            closestBuilding.GetComponent<Building>().SpawnAnimation();
            GameObject.Instantiate(Resources.Load("Prefabs/Tutorial/TouchAndHold"), GameObject.Find("Canvas").transform);
        }
        else
        {

        }

        gamePhase = GamePhase.Planning;
    }

    public void ProcessLocationStatus()
    {
        if (_gps.gpsStatus == GpsStatus.Succeed)
        {
            InitSpawnSelection(true);
        }
        if (_gps.gpsStatus == GpsStatus.Disabled || _gps.gpsStatus == GpsStatus.Failed)
        {
            InitSpawnSelection(false);
        }
    }

	// Update is called once per frame
	void Update () {

        if (gamePhase == GamePhase.Initalizing && hasPlayedIntro)
            ProcessLocationStatus();
        if (isTouchHoldTutDone && !hasSpawnedCampus)
            StartCoroutine(_buildingManager.SpawnAllBuildings());
		
	}
}
