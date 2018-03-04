
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdSystem : MonoBehaviour {

    public static CrowdSystem instance = null;

    public GameObject student;
    public Transform[] spawnLocations;

    public int studentCount;
    public int numDestinations;
    public float secondsInDay;
    public int maxNumPeriods;

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

    private bool isCurrentlySpawning;

    private int currStudentCount;
    private List<GameObject> studentsInClass;
    private GameObject player;

    BuildingManager bm;

    
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

            //Check if its next period
            if (currentPeriod != Mathf.FloorToInt(currentSeconds / secondsPerPeriod))
            {
                //next period
                currentPeriod++;
                secondsLeftInPeriod = secondsPerPeriod;
                EndClasses();
            }

            //Is spawning in a coroutine/ Is the period less than x% over?/ is the day started
            if (!isCurrentlySpawning && secondsLeftInPeriod < .75 * secondsPerPeriod)
            {
                StartCoroutine(SpawnStudent());
            }
        } 
    }

    public void StartDay()
    {
        isDayStarted = true;
        CreatePlayer();
        //InvokeRepeating("SpawnStudent", 0f, 0.5f);
    }

    public IEnumerator SpawnStudent()
    {
        isCurrentlySpawning = true;
        yield return new WaitForSeconds(1);

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
        isCurrentlySpawning = false;
    }
	
    public IEnumerator ToggleStudent(GameObject student, float time)
    {
        student.SetActive(false);
        yield return new WaitForSeconds(5f);
        student.SetActive(true);
        student.GetComponent<StudentAI>().NextTarget();
    }

    public void StartToggleStudent(GameObject student, float time)
    {
        StartCoroutine(ToggleStudent(student, time));
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
            stu.agent.SetDestination(stu.schedule[currentPeriod]);
        }
        studentsInClass.Clear();
    }

    public void CreatePlayer()
    {
        Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
        player = Instantiate(student, randSpawn.position, Quaternion.identity, transform);

        StudentAI playerAI = player.GetComponent<StudentAI>();

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
                BuildingManager.instance.buildings.TryGetValue("Library", out target);
                schedule[i] = target.GetNavPos();
            }
        }
        studentAI.schedule = schedule;
        studentAI.hasClass = hasClass;
    }
}
