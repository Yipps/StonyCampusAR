using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBuildingsGUI : MonoBehaviour {

    public SelectedBuildingsList selectedBuildings;
    public CurrentDay currentDay;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateSelectedBuildingText()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Building i in selectedBuildings.list)
        {
            //Create empty gameObject
            GameObject building = new GameObject("Building");
            building.transform.parent = transform;
            building.name = i.name;

            //Add Text component
            Text text = building.AddComponent<Text>();
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.fontSize = 20;
            text.color = Color.black;
            text.text = i.name;
            text.alignment = TextAnchor.MiddleCenter;

            //Set height of text UI object
            building.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 45);
        }
    }

    public void UpdateCurrentTarget()
    {
        int index = currentDay.currentPeriod;

        //Only update style if player has target for current period
        if (index < selectedBuildings.list.Count)
        {
            Text text = transform.GetChild(index).GetComponent<Text>();
            text.fontStyle = FontStyle.Bold;
            text.color = Color.green;


        }

        //Set Prev text style
        if (index != 0 && index <= selectedBuildings.list.Count)
        {
            Text prevText = transform.GetChild(index - 1).GetComponent<Text>();
            prevText.fontStyle = FontStyle.Normal;
            prevText.color = Color.black;
        }



    }

    public void log()
    {
        Debug.Log("Why was this event raised");
    }

}
