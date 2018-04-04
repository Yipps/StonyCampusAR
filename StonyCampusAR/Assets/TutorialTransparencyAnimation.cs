using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTransparencyAnimation : MonoBehaviour {

    public enum Tutorial {Touch,Hold,Swipe};

    public float duration;
    public Image image;
    public Tutorial tutorialType;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        LerpAlpha();
        if (tutorialType == Tutorial.Hold && GameManager.instance.isTouchHoldTutDone)
        {
            Debug.Log("Destroy tut");
            Destroy(transform.parent.gameObject);
        }
            
        else if (tutorialType == Tutorial.Swipe && GameManager.instance.isSwipeTutDone)
            Destroy(transform.parent.gameObject);
        else if (tutorialType == Tutorial.Touch && GameManager.instance.isTouchTutDone)
            Destroy(transform.parent.gameObject);

	}

    void LerpAlpha()
    {
        float frac = (Mathf.Sin(Time.time * 4)/2 + 0.5f);
        Color color = image.color;
        color.a = frac;
        image.color = color;
    }
}
