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
    public Transform[] targetLocations;

    private int studentCounter;

    private void Start()
    {
        campusEvent.eventPositions = targetLocations;
        campusEvent.startPeriod = currentDay.currentPeriod;

        InvokeRepeating("SpawnEventStudents", 0f, 1f);
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

        studentCounter++;
        if (studentCounter == campusEvent.maxNumOfStudents)
            CancelInvoke();
    }

    private void OnDisable()
    {
        campusEvent.eventPositions = null;
    }

}
