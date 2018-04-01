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

    bool hasPlayedIntro;
    public GamePhase gamePhase;

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

        StartCoroutine(StartIntro());
        

    }

    private IEnumerator StartIntro()
    {
        //Welcome Animation

        gamePhase = GamePhase.Initalizing;
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
        _buildingManager = BuildingManager.instance;
        _crowdSystem = CrowdSystem.instance;
        _navControl = NavigationControl.instance;
        _gps = GetComponent<Gps>();
    }
	
    private void InitSpawnSelection()
    {
        gamePhase = GamePhase.Planning;
        Vector3 pos = _gps.PingMap();
        Debug.Log(pos);
        GameObject spawnPointer = GameObject.Instantiate(Resources.Load("Prefabs/SpawnPointer") as GameObject,transform.GetChild(0));
        spawnPointer.transform.localPosition = pos;
        //spawnPointer.transform.position = pos;
        _navControl.isSpawnMovable = true;
        _navControl.spawnPoint = spawnPointer.transform;
        StartCoroutine(_buildingManager.SpawnAllBuildings());

    }

	// Update is called once per frame
	void Update () {
        if (gamePhase == GamePhase.Planning && hasPlayedIntro && _navControl.spawnPoint == null)
        {
            InitSpawnSelection();
        }
		
	}

    //IEnumerator PlayIntro()
    //{
    //    GameObject[] animations = GameObject.FindGameObjectsWithTag("IntroAnimation");
    //    yield return new WaitForSeconds(1f);
    //    animations[0].GetComponent<Animator>().SetTrigger("spawn");
    //    yield return new WaitForSeconds(3f);
    //    animations[1].GetComponent<Animator>().SetTrigger("spawn");
    //    yield return new WaitForSeconds(3f);
    //    StartCoroutine(_buildingManager.SpawnAllBuildings());

    //}

    
}
