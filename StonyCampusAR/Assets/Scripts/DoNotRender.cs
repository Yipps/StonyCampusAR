using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotRender : MonoBehaviour {

    public MeshRenderer render;

	// Use this for initialization
	void Start () {
        StartCoroutine(HideMesh());
    }
	
	// Update is called once per frame
	void Update () {
        
		
	}

    IEnumerator HideMesh()
    {
        yield return null;
        render.enabled = false;
    }
}
