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
        _student.GetComponent<Renderer>().material.color = Color.cyan;

        //Setup StudentAIController
        EventCoreAI ai = campusEvent.ai;
        _student.GetComponent<StudentAIController>().ai = ai;
        ai.campusEvent = campusEvent;

        if (campusEvent.agentSpeed != 0)
            _student.GetComponent<NavMeshAgent>().speed = campusEvent.agentSpeed;

        _student.GetComponent<StudentAIController>().homePosition = homeSpawn;
        
        _student.GetComponent<NavMeshAgent>().Warp(randSpawn);
        _student.GetComponent<StudentAIController>().enabled = true;
        
    }

    private void OnDisable()
    {
        campusEvent.eventPositions = null;
    }

    private void Update()
    {
        if (IsCampusEventOver())
        {
            GetComponent<CampusEventGUI>().DisableGUI();
            //Once all students are gone, destroy event
            if (campusEvent.currentNumOfStudents == 0)
                Destroy(gameObject);
        }
        else
        {
            //Spawn until max num of students reached for event
            if (campusEvent.currentNumOfStudents < campusEvent.maxNumOfStudents)
            {
                campusEvent.currentNumOfStudents++;
                SpawnEventStudents();
            }
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

    private bool IsCampusEventOver()
    {
       
        if (campusEvent.startPeriod + campusEvent.numOfPeriods <= currentDay.currentPeriod || currentDay.currentPeriod == currentDay.maxPeriods)
        {
            return true;
        }
        return false;
    }


}
