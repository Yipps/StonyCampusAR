
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdSystem : MonoBehaviour
{
    public static CrowdSystem instance = null;

    public GameObject student;
    public Transform[] spawnLocations;
    public Transform playerSpawn;
    public CurrentDay currDay;


    public int studentCount;
    public float secondsInDay;
    public int maxNumPeriods;
    public float spawnDelayInSeconds;

    public int currentPeriod;
    public bool isDayStarted;

    [HideInInspector]
    public float currentSeconds;
    public float secondsLeftInPeriod;
    public float secondsPerPeriod;

    private bool isInSpawningCoroutine;

    private int currStudentCount;

    private CoreAI goToClass;
    private CoreAI wanderer;
    private CoreAI player;

    BuildingManager bm;
    private bool isSpawning;

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

    void Start()
    {
        currStudentCount = 0;
        bm = BuildingManager.instance;
        secondsPerPeriod = secondsInDay / maxNumPeriods;
        secondsLeftInPeriod = secondsPerPeriod;
        currDay.maxPeriods = maxNumPeriods;
        currDay.currentPeriod = 0;
        //StartDay();
    }

    private void Update()
    {
        if (isDayStarted)
        {
            //Update time
            currentSeconds += Time.deltaTime;
            secondsLeftInPeriod -= Time.deltaTime;

            if (secondsLeftInPeriod < 0.40 * secondsPerPeriod || maxNumPeriods == currentPeriod)
                isSpawning = false;

            //Check if its next period
            if (currentPeriod != Mathf.FloorToInt(currentSeconds / secondsPerPeriod))
            {
                //next period
                currentPeriod++;
                secondsLeftInPeriod = secondsPerPeriod;
                isSpawning = true;
                currDay.currentPeriod = currentPeriod;
            }

            if (!isInSpawningCoroutine && isSpawning)
            {
                //StartCoroutine(SpawnStudent());
            }
        }
    }

    public void StartDay()
    {
        isDayStarted = true;
        isSpawning = true;
        //StartCoroutine(SpawnStudent())
    }

    public IEnumerator SpawnStudent(CoreAI ai)
    {
        isInSpawningCoroutine = true;
        yield return new WaitForSeconds(spawnDelayInSeconds);

        //Instantiate student prefab
        Vector3 randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
        Vector3 homeSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)].position;

        GameObject _student = Instantiate(student, transform);

        _student.GetComponent<NavMeshAgent>().Warp(randSpawn);
        _student.GetComponent<StudentAIController>().homePosition = homeSpawn;
        _student.GetComponent<StudentAIController>().ai = ai;

        _student.GetComponent<StudentAIController>().enabled = true;
        //Stop making students
        currStudentCount++;
        isInSpawningCoroutine = false;
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(0, Screen.height - 100, 400, 100), "Day Start = " + isDayStarted.ToString());
        GUI.Label(new Rect(0, Screen.height - 80, 400, 100), "Is Spawning: " + isSpawning.ToString());
        GUI.Label(new Rect(0, Screen.height - 60, 400, 100), "#Students: " + currStudentCount);
        GUI.Label(new Rect(0, Screen.height - 40, 400, 100), "Current Period: " + currentPeriod);
        GUI.Label(new Rect(0, Screen.height - 20, 400, 100), "Time until next Period: " + secondsLeftInPeriod);
    }

}
