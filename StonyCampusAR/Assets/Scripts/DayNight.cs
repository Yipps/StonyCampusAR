using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {


    public float speed;
    public GameObject light;
    public GameObject sun;
    public GameObject moon;

    public float distanceOffset;


	// Use this for initialization
	void Start () {
        sun.transform.Translate(new Vector3(distanceOffset,0,0));
        moon.transform.Translate(new Vector3(distanceOffset, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
    
        transform.Rotate(0, 0, speed * Time.deltaTime);
	}
}
