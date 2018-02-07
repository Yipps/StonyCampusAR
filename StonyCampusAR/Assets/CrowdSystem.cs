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

    GameObject[] buildings;

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
        buildings = BuildingManager.instance.buildingGameObjects;
        Init();
	}

    public Vector3[] RandSchedule(int count)
    {
        List<Vector3> targets = new List<Vector3>();
        for (int x = 0; x < count; x++)
        {
            GameObject rand = buildings[Random.Range(0, buildings.Length)];
            NavMeshHit hit;
            NavMesh.SamplePosition(rand.transform.position, out hit, 10f, NavMesh.AllAreas);
            targets.Add(hit.position);
        }
        return targets.ToArray();
    }

    public void Init()
    {
        InvokeRepeating("SpawnStudent", 0f, 0.5f);
    }

    public void SpawnStudent()
    {
        Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
        GameObject _student = Instantiate(student, randSpawn.position, Quaternion.identity);
        StudentAI _studentAI = _student.GetComponent<StudentAI>();
        _studentAI.agent.Warp(randSpawn.position);
        Vector3[] schedule = RandSchedule(numDestinations);
        _studentAI.schedule = schedule;
        _studentAI.Init();
        currStudentCount++;
        if (currStudentCount == studentCount)
        {
            CancelInvoke();
            Debug.Log(currStudentCount);
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

    // Update is called once per frame
    void Update () {
		
	}
}
