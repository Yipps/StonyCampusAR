using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusEventsController : MonoBehaviour
{
    public CampusEvent campusEvent;

    private void Start()
    {
        campusEvent.eventPositions = GetComponentsInChildren<Transform>();
    }

    private void SpawnEventStudents()
    {

    }



}
