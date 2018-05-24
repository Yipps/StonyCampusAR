using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampusEventGUI : MonoBehaviour {
    CampusEvent campusEvent;
    GameObject icon;
    GameObject infoGUI;

	// Use this for initialization
	void Start () {
        campusEvent = GetComponent<CampusEventsController>().campusEvent;
        icon = transform.GetChild(0).gameObject;
        infoGUI = transform.GetChild(1).gameObject;
        Invoke("DisplayIcon", 3);
	}
	
	void DisplayIcon()
    {
        icon.SetActive(true);
    }

    public void ToggleInfoGUI()
    {
        icon.SetActive(!icon.activeSelf);
        infoGUI.SetActive(!infoGUI.activeSelf);
    }

    public void DisableGUI()
    {
        icon.SetActive(false);
        infoGUI.SetActive(false);
    }

}
