using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class animationControl : MonoBehaviour {

    public bool isWaiting;
    Animator animator;


	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        isWaiting = false;
	}
	
    IEnumerator Playloop()
    {
        Debug.Log("Loop");
        isWaiting = true;
        animator.SetTrigger("spawn");
        yield return new WaitForSeconds(3);
        isWaiting = false;
       

    }

	// Update is called once per frame
	void Update () {
        if (!isWaiting)
            StartCoroutine(Playloop());
    }
}
