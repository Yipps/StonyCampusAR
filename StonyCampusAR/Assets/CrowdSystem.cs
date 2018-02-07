using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdSystem : MonoBehaviour {

    public GameObject student;
    public Transform[] spawnLocations;
    public int studentCount;
    public int numDestinations;

    GameObject[] buildings;

    // Use this for initialization
    void Start () {
        buildings = BuildingManager.instance.buildingGameObjects;
        InitStudents();
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

    public void InitStudents()
    {
        for(int x = 0; x < studentCount; x++)
        {
            Transform randSpawn = spawnLocations[Random.Range(0, spawnLocations.Length)];
            GameObject _student = Instantiate(student, randSpawn.position, Quaternion.identity);
            StudentAI _studentAI = _student.GetComponent<StudentAI>();
            _studentAI.agent.Warp(randSpawn.position);
            Vector3[] schedule = RandSchedule(numDestinations);
            _studentAI.schedule = schedule;
            _studentAI.Init();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
