using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControls : MonoBehaviour {

    public GameObject MenuButton;
    public GameObject MenuPanel;


    void OpenMenu()
    {
        MenuButton.SetActive(false);
        MenuPanel.SetActive(true);
    }

    void CloseMenu()
    {
        MenuButton.SetActive(true);
        MenuPanel.SetActive(false);
    }

    void FilterOrganizations(string org)
    {
        BuildingManager.instance.HighlightOrganization(org);
    }


}
