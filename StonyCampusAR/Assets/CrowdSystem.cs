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

    private int currStudentCount;
    private List<GameObject> students;
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
        //StartDay();
	}

    public Vector3[] RandSchedule(int count)
    {
        List<Vector3> targets = new List<Vector3>();
        for (int x = 0; x < count; x++)
        {
            GameObject[] buildings = bm.buildingGameObjects;
            GameObject rand = buildings[Random.Range(0, buildings.Length)];
            NavMeshHit hit;
            NavMesh.SamplePosition(rand.transform.position, out hit, 10f, NavMesh.AllAreas);
            targets.Add(hit.position);
        }
        return targets.ToArray();
    }

    public void StartDay()
    {
        CreatePlayer();
        InvokeRepeating("SpawnStudent", 0f, 0.5f);
    }

    public void SpawnStudent()
    {
        //Instantiate student prefab
        Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
        GameObject _student = Instantiate(student, randSpawn.position, Quaternion.identity,transform);
        StudentAI _studentAI = _student.GetComponent<StudentAI>();
        _studentAI.agent.Warp(randSpawn.position);

        //Assign agent destinations
        Vector3[] schedule = RandSchedule(numDestinations);
        _studentAI.schedule = schedule;

        // Start moving
        _studentAI.Init();

        //Stop making students
        currStudentCount++;
        if (currStudentCount == studentCount)
        {
            CancelInvoke();
        }
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

    public void CreatePlayer()
    {
        Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
        player = Instantiate(student, randSpawn.position, Quaternion.identity, transform);

        StudentAI playerAI = player.GetComponent<StudentAI>();

        List<Vector3> schedule = new List<Vector3>();

        NavMeshHit hit;
        foreach (Building i in bm.selectedBuildings)
        {
            NavMesh.SamplePosition(i.transform.position, out hit, 10f, NavMesh.AllAreas);
            schedule.Add(hit.position);
        }

        playerAI.schedule = schedule.ToArray();

        player.GetComponent<Renderer>().material.color = Color.blue;

        playerAI.agent.Warp(randSpawn.position);

        playerAI.Init();
    }

    public void EndDay()
    {
        CancelInvoke();
        currStudentCount = 0;

        foreach (GameObject i in students)
        {
            students.Remove(i);
            GameObject.Destroy(i);
        }

        if (player != null)
        {
            GameObject.Destroy(player);
            player = null;
        }



    }
}
