using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {


    public float speed;
    public GameObject light;
    public GameObject sun;
    public GameObject moon;
    public bool isEnabled;
    public float distanceOffset;


    // Use this for initialization
    private void Start()
    {
        sun.SetActive(false);
        moon.SetActive(false);
    }

    public void StartDay () {
        sun.SetActive(true);
        moon.SetActive(true);
        isEnabled = true;
        sun.transform.Translate(new Vector3(distanceOffset,0,0));
        moon.transform.Translate(new Vector3(distanceOffset, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
        if (isEnabled)
            transform.Rotate(0, 0, speed * Time.deltaTime);
	}
}
