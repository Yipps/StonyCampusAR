
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdSystem : MonoBehaviour
{
    public GameObject student;
    public SpawnLocationList spawnLocations;
   
    public CurrentDay currDay;
    public PlayerData playerData;
    public GoingToClassStudentsRuntimeList goingToClassList;
    public WanderingStudentsRuntimeList wanderingStudentList;
    public SelectedBuildingsList selectedBuildings;
    public float secondsPerPeriod;
    public int maxNumPeriods;
    public float spawnDelayInSeconds;

    public int maxClassStudents;
    public int maxWanderingStudents;
    [HideInInspector] public Transform playerSpawn;
    [HideInInspector] public bool isDayStarted;
    [HideInInspector] public float currentSeconds;
    [HideInInspector] public float secondsLeftInPeriod;

    private bool isInSpawningCoroutine;

    private CoreAI goToClassAI;
    private CoreAI wanderAI;
    private CoreAI playerAI;
    private GameEvent periodEndedEvent;
    private GameEvent dayHasEndedEvent;

    BuildingManager bm;
    private bool isSpawning;


    void Start()
    {
        bm = BuildingManager.instance;
        wanderAI = Resources.Load("PluggableAI/Wander") as WanderingAI;
        playerAI = Resources.Load("PluggableAI/Player") as PlayerAI;
        goToClassAI = Resources.Load("PluggableAI/GoingToClass") as GoingToClassAI;
        periodEndedEvent = Resources.Load("GameEvents/PeriodEnded") as GameEvent;
        dayHasEndedEvent = Resources.Load("GameEvents/dayHasEnded") as GameEvent;

        //ScriptableObjects don't reset
        currDay.currentPeriod = 0;
    }

    private void Update()
    {
        if (isDayStarted)
        {
            SpawnAllStudents();
            ProcessTime();
        }
    }

    public void StartNextPeriod()
    {
        secondsLeftInPeriod = secondsPerPeriod;
        isSpawning = true;
    }

    public void StartDay()
    {
        InitDay();
        isDayStarted = true;
        isSpawning = true;
        SpawnPlayer(playerAI);
    }

    public IEnumerator SpawnStudent(CoreAI ai)
    {

        isInSpawningCoroutine = true;
        
        //Instantiate student prefab
        Vector3 randSpawn = spawnLocations.list[Random.Range(0, spawnLocations.list.Count)].position;
        Vector3 homeSpawn = spawnLocations.list[Random.Range(0, spawnLocations.list.Count)].position;

        GameObject _student = Instantiate(student, transform);

        _student.GetComponent<NavMeshAgent>().Warp(randSpawn);
        _student.GetComponent<StudentAIController>().homePosition = homeSpawn;
        _student.GetComponent<StudentAIController>().ai = ai;

        _student.GetComponent<StudentAIController>().enabled = true;

        yield return new WaitForSeconds(spawnDelayInSeconds);
        isInSpawningCoroutine = false;
    }

    public void SpawnPlayer(CoreAI ai)
    {
        Vector3 playerSpawnPosition = new Vector3(playerData.playerPointer.transform.position.x, 0.1f, playerData.playerPointer.transform.position.z);

        //Instantiate gameobject
        GameObject player = Instantiate(student, playerSpawnPosition, Quaternion.identity);

        //Setup line renderer reference
        DrawSchedulePath pathDrawer = transform.parent.GetComponentInChildren<DrawSchedulePath>();
        pathDrawer.isPlayerActive = true;
        //Set scriptable object play data
        playerData.playerPointer.transform.SetParent(player.transform);
        playerData.playerAgent = player.GetComponent<NavMeshAgent>();
        //Initalize AI component of prefab
        StudentAIController playerAIControl = player.GetComponent<StudentAIController>();
        playerAIControl.ai = ai;
        playerAIControl.homePosition = spawnLocations.list[Random.Range(0, spawnLocations.list.Count)].position;
        //Position and enable player
        player.GetComponent<NavMeshAgent>().Warp(playerSpawnPosition);
        playerAIControl.enabled = true;
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(0, Screen.height - 100, 400, 100), "Day Start = " + isDayStarted.ToString());
        GUI.Label(new Rect(0, Screen.height - 80, 400, 100), "Is Spawning: " + isSpawning.ToString());
        GUI.Label(new Rect(0, Screen.height - 40, 400, 100), "Current Period: " + currDay.currentPeriod);
        GUI.Label(new Rect(0, Screen.height - 20, 400, 100), "Time until next Period: " + secondsLeftInPeriod);
    }

    private void EndDay()
    {
        isSpawning = false;
        isDayStarted = false;
        Debug.Log("DayHasEnded Event Raised");
        dayHasEndedEvent.Raise();
    }

    private void InitDay()
    {
        DestroyAllStudents();
        maxNumPeriods = selectedBuildings.list.Count;
        //secondsPerPeriod = secondsInDay / maxNumPeriods;
        secondsLeftInPeriod = secondsPerPeriod;
        currDay.maxPeriods = maxNumPeriods;
        currDay.currentPeriod = 0;
        currentSeconds = 0;
    }

    private void DestroyAllStudents()
    {
        //Destory all active students and clear list
        foreach (GameObject i in wanderingStudentList.list)
            Destroy(i);
        wanderingStudentList.list.Clear();

        foreach (GameObject i in goingToClassList.list)
            Destroy(i);
        goingToClassList.list.Clear();
    }

    void SpawnAllStudents()
    {
        if (!isInSpawningCoroutine)
        {
            if (goingToClassList.list.Count < maxClassStudents && isSpawning)
                StartCoroutine(SpawnStudent(goToClassAI));

            if (wanderingStudentList.list.Count < maxWanderingStudents)
                StartCoroutine(SpawnStudent(wanderAI));
        }
    }

    void ProcessTime()
    {
        //Update time
        currentSeconds += Time.deltaTime;
        secondsLeftInPeriod -= Time.deltaTime;

        if (secondsLeftInPeriod < 0.40 * secondsPerPeriod || currDay.currentPeriod == currDay.maxPeriods)
            isSpawning = false;

        //Check if its next period
        if (currDay.currentPeriod != Mathf.FloorToInt(currentSeconds / secondsPerPeriod))
        {
            currDay.currentPeriod = currDay.currentPeriod + 1;
            periodEndedEvent.Raise();
            if (currDay.currentPeriod == currDay.maxPeriods)
                EndDay();
            else
                StartNextPeriod();

        }
    }
}
