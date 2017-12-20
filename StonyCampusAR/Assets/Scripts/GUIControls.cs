using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControls : MonoBehaviour {

    public GameObject MenuButton;
    public GameObject MenuPanel;


    public void OpenMenu()
    {
        MenuButton.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        MenuButton.SetActive(true);
        MenuPanel.SetActive(false);
    }

    public void FilterOrganizations(string org)
    {
        BuildingManager.instance.HighlightOrganization(org);
    }


}
