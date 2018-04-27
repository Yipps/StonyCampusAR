using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

    public float speed = 5;
    [Range(0,2)]
    public int axis = 1;
	
	// Update is called once per frame
	void Update () {
        if(axis == 0)
            transform.Rotate(speed * Time.deltaTime, 0, 0);
        else if (axis == 1)
            transform.Rotate(0, speed * Time.deltaTime, 0);
        else if  (axis == 2)
            transform.Rotate(0, 0, speed * Time.deltaTime);

    }
}
