
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdSystem : MonoBehaviour
{
    public GameObject student;
    public SpawnLocationList spawnLocations;
    public Transform playerSpawn;
    public CurrentDay currDay;
    public StudentRuntimeList studentList;

    public int studentCount;
    public float secondsInDay;
    public int maxNumPeriods;
    public float spawnDelayInSeconds;

    public bool isDayStarted;

    [HideInInspector] public float currentSeconds;
    [HideInInspector] public float secondsLeftInPeriod;
    [HideInInspector] public float secondsPerPeriod;

    private bool isInSpawningCoroutine;

    private int currStudentCount;

    private CoreAI goToClassAI;
    private CoreAI wanderAI;
    private CoreAI playerAI;

    BuildingManager bm;
    private bool isSpawning;

    void Start()
    {
        bm = BuildingManager.instance;
        wanderAI = Resources.Load("PluggableAI/Wander") as WanderingAI;
        playerAI = Resources.Load("PluggableAI/Player") as PlayerAI;
        goToClassAI = Resources.Load("PluggableAI/GoingToClass") as GoingToClassAI;
    }

    private void Update()
    {
        if (isDayStarted)
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
                if (currDay.currentPeriod == currDay.maxPeriods)
                    EndDay();
                else
                    StartNextPeriod();
                
            }
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
        GameObject player = Instantiate(student, transform);
        StudentAIController playerAIControl = player.GetComponent<StudentAIController>();
        playerAIControl.ai = ai;
        playerAIControl.homePosition = spawnLocations.list[Random.Range(0, spawnLocations.list.Count)].position;

        DrawSchedulePath pathDrawer = transform.parent.GetComponentInChildren<DrawSchedulePath>();
        pathDrawer.agent = player.GetComponent<NavMeshAgent>();
        pathDrawer.isPlayerActive = true;

        playerAIControl.enabled = true;
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(0, Screen.height - 100, 400, 100), "Day Start = " + isDayStarted.ToString());
        GUI.Label(new Rect(0, Screen.height - 80, 400, 100), "Is Spawning: " + isSpawning.ToString());
        GUI.Label(new Rect(0, Screen.height - 60, 400, 100), "#Students: " + currStudentCount);
        GUI.Label(new Rect(0, Screen.height - 40, 400, 100), "Current Period: " + currDay.currentPeriod);
        GUI.Label(new Rect(0, Screen.height - 20, 400, 100), "Time until next Period: " + secondsLeftInPeriod);
    }

    private void EndDay()
    {
        isSpawning = false;
        isDayStarted = false;

        //Destory all active students and clear list
        foreach (GameObject i in studentList.list)
        {
            Destroy(i);
        }
        studentList.list.Clear();
    }

    private void InitDay()
    {
        currStudentCount = 0;
        secondsPerPeriod = secondsInDay / maxNumPeriods;
        secondsLeftInPeriod = secondsPerPeriod;
        currDay.maxPeriods = maxNumPeriods;
        currDay.currentPeriod = 0;
        currentSeconds = 0;
    }
}
