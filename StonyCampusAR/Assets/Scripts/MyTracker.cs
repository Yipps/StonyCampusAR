using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTracker : DefaultTrackableEventHandler {


    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        GameObject nearest = FindNearestBuilding();
        //nearest.GetComponentInChildren<Building>().DisplayDave();
        Debug.Log(nearest.name);
    }

    private GameObject FindNearestBuilding()
    {
        GameObject nearest = new GameObject();
        GameObject[] BuildingGameObjects = GameObject.FindGameObjectsWithTag("CampusBuildings");

        float closestDist = Mathf.Infinity;
        foreach (GameObject i in BuildingGameObjects)
        {
            Vector3 direction = this.transform.position - i.transform.position;
            float dist = direction.sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                nearest = i;
            }
        }
        Debug.Log(this.transform.position);
        Debug.Log(nearest.transform.position);
        return nearest;
    }
}
