using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour {

    public Camera cam;
    public NavMeshAgent nav;


	// Use this for initialization
	void Start () {
        cam = Camera.main;
        //nav = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log(ray.ToString());
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.transform)
            {
                EventManager.TriggerEvent("BuildingSelected", hit.transform.gameObject);
            }

            if (Physics.Raycast(ray,out hit))
            {
                Debug.Log("Ray cast");
                nav.SetDestination(hit.point);
            }
        }
	}
}
