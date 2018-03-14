
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdSystem : MonoBehaviour {
    public static CrowdSystem instance = null;

    public GameObject student;
    public Transform[] spawnLocations;

    public GameObject player;

    public int studentCount;
    public int numDestinations;
    public float secondsInDay;
    public int maxNumPeriods;
    public float spawnDelayInSeconds;

    public float normalSpeed;
    public float normalAcceleration;

    public float maxSpeed;
    public float maxAcceleration;

    public int currentPeriod;
    public bool isDayStarted;
    
    [HideInInspector]
    public float currentSeconds;
    public float secondsLeftInPeriod;
    public float secondsPerPeriod;

    private bool isInSpawningCoroutine;

    private int currStudentCount;
    private List<GameObject> studentsInClass;

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

    void Start () {
        currStudentCount = 0;
        bm = BuildingManager.instance;
        studentsInClass = new List<GameObject>();
        secondsPerPeriod = secondsInDay / maxNumPeriods;
        secondsLeftInPeriod = secondsPerPeriod;
        //StartDay();
    }

    private void Update()
    {
        if (isDayStarted)
        {
            //Update time
            currentSeconds += Time.deltaTime;
            secondsLeftInPeriod -= Time.deltaTime;

            if (secondsLeftInPeriod < 0.40 * secondsPerPeriod)
                isSpawning = false;

            //Check if its next period
            if (currentPeriod != Mathf.FloorToInt(currentSeconds / secondsPerPeriod))
            {
                //next period
                currentPeriod++;
                secondsLeftInPeriod = secondsPerPeriod;
                isSpawning = true;
                EndClasses();
            }

            if (!isInSpawningCoroutine && isSpawning)
            {
                StartCoroutine(SpawnStudent());
            }
        } 
    }

    public void StartDay()
    {
        isDayStarted = true;
        isSpawning = true;
        CreatePlayer();
        //InvokeRepeating("SpawnStudent", 0f, 0.5f);
    }

    public IEnumerator SpawnStudent()
    {
        isInSpawningCoroutine = true;
        yield return new WaitForSeconds(spawnDelayInSeconds);

        //Instantiate student prefab
        Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
        GameObject _student = Instantiate(student, randSpawn.position, Quaternion.identity,transform);
        StudentAI _studentAI = _student.GetComponent<StudentAI>();
        _studentAI.agent.Warp(randSpawn.position);

        //Assign agent destinations
        InitSchedule(_student);

        // Start moving
        _studentAI.Init();

        //Stop making students
        currStudentCount++;
        isInSpawningCoroutine = false;
    }
	
    public void EnterClass(GameObject student)
    {
        studentsInClass.Add(student);
        student.SetActive(false);
    }

    public void EndClasses()
    {
        foreach(GameObject i in studentsInClass)
        {
            i.SetActive(true);
            StudentAI stu = i.GetComponent<StudentAI>();
            stu.currTarget++;
            stu.agent.SetDestination(stu.schedule[currentPeriod]);
        }
        studentsInClass.Clear();
    }

    public void CreatePlayer()
    {
        Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
        player = Instantiate(student, randSpawn.position, Quaternion.identity, transform);


        //HACK HACK HACK
        StudentAI playerAI = player.GetComponent<StudentAI>();
        Destroy(playerAI);

        playerAI = player.AddComponent<PlayerAI>();

        List<Vector3> schedule = new List<Vector3>();
        foreach (Building i in bm.selectedBuildings)
        {
            schedule.Add(i.GetNavPos());
        }


        playerAI.schedule = schedule.ToArray();

        player.GetComponent<Renderer>().material.color = Color.blue;

        playerAI.agent.Warp(randSpawn.position);

        playerAI.Init();
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(0, Screen.height - 100, 400, 100),"Day Start = " + isDayStarted.ToString());
        GUI.Label(new Rect(0, Screen.height - 80, 400, 100), "Is Spawning: " + isSpawning.ToString());
        GUI.Label(new Rect(0, Screen.height - 60, 400, 100), "#Students: " + currStudentCount);
        GUI.Label(new Rect(0, Screen.height - 40, 400, 100), "Current Period: " + currentPeriod);
        GUI.Label(new Rect(0, Screen.height - 20, 400, 100), "Time until next Period: " + secondsLeftInPeriod);
    }

    public StudentAI GetPlayerAI()
    {
        return player.GetComponent<StudentAI>();
    }

    public NavMeshAgent GetPlayerAgent()
    {
        return player.GetComponent<NavMeshAgent>();
    }

    //public void EndDay()
    //{
    //    CancelInvoke();
    //    currStudentCount = 0;

    //    foreach (GameObject i in students)
    //    {
    //        students.Remove(i);
    //        GameObject.Destroy(i);
    //    }

    //    if (player != null)
    //    {
    //        GameObject.Destroy(player);
    //        player = null;
    //    }



    //}

    public Building RandBuilding()
    {

        GameObject[] buildings = bm.buildingGameObjects;
        GameObject rand = buildings[Random.Range(0, buildings.Length)];
        return rand.GetComponent<Building>();
    }

    public void InitSchedule(GameObject student)
    {
        bool[] hasClass = new bool[maxNumPeriods];
        Vector3[] schedule = new Vector3[maxNumPeriods];

        StudentAI studentAI = student.GetComponent<StudentAI>();

        if (studentAI == null)
            studentAI = student.AddComponent<StudentAI>();

        for(int i = currentPeriod; i < hasClass.Length; i++)
            hasClass[i] = (Random.value < 0.65);

        for (int i = currentPeriod; i < hasClass.Length; i++)
        {
            //If student has class during this period
            if (hasClass[i])
            {
                schedule[i] = RandBuilding().GetNavPos();
                //If student doesn't have class for 2 periods, go home
            }
            else if (i == maxNumPeriods - 2)
            {
                if (!hasClass[i + 1])
                {
                    schedule[i] = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
                }
            }
            else
            {
                Building target;
                BuildingManager.instance.buildingsDict.TryGetValue("Library", out target);
                schedule[i] = target.GetNavPos();
            }
        }
        studentAI.schedule = schedule;
        studentAI.hasClass = hasClass;
    }
}
