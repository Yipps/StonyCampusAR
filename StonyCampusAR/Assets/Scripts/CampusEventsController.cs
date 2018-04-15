using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CampusEventsController : MonoBehaviour
{
    public CampusEvent campusEvent;
    public CurrentDay currentDay;
    public SpawnLocationList spawnLocations;
    public GameObject student;


    private void Start()
    {
        InitTargetPositions();
        campusEvent.startPeriod = currentDay.currentPeriod;
    }

    private void SpawnEventStudents()
    {
        Vector3 randSpawn = spawnLocations.list[Random.Range(0, spawnLocations.list.Count)].position;
        Vector3 homeSpawn = spawnLocations.list[Random.Range(0, spawnLocations.list.Count)].position;
        GameObject _student = Instantiate(student, transform);
        _student.GetComponent<StudentAIController>().ai = campusEvent.ai;

        //EventCoreAI ai;
        //if (_student.GetComponent<StudentAIController>().ai is EventCoreAI)
        //{
        //    ai = (EventCoreAI)_student.GetComponent<StudentAIController>().ai;
        //}
            
        EventCoreAI ai = (EventCoreAI)_student.GetComponent<StudentAIController>().ai;

        _student.GetComponent<NavMeshAgent>().Warp(randSpawn);
        _student.GetComponent<StudentAIController>().homePosition = homeSpawn;
        ai.campusEvent = campusEvent;
        _student.GetComponent<StudentAIController>().enabled = true;
        
    }

    private void OnDisable()
    {
        campusEvent.eventPositions = null;
    }

    private void Update()
    {
        if (campusEvent.currentNumOfStudents < campusEvent.maxNumOfStudents)
        {
            campusEvent.currentNumOfStudents++;
            Invoke("SpawnEventStudents", 0.5f);
        }
    }

    private void InitTargetPositions()
    {
        List<Transform> positions = new List<Transform>();

        foreach(Transform i in transform)
        {
            positions.Add(i);
        }

        campusEvent.eventPositions = positions.ToArray();
    }

    private void OnDrawGizmos()
    {
        foreach (Transform i in transform)
            Gizmos.DrawWireCube(i.position, new Vector3(2f,2f,2f));
    }

}
