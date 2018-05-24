using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControls : MonoBehaviour {

    public GameObject menuButton;
    public GameObject menuPanel;
    public GameObject startDay;
    public GameObject buildSchedule;
    public SelectedBuildingsList selectedBuildings;

    public void OpenMenu()
    {
        menuButton.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        menuButton.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void FilterOrganizations(string org)
    {
        BuildingManager.instance.HighlightOrganization(org);
    }

    public void EnableStartDay()
    {
        if (IsStartDayValid())
            startDay.SetActive(true);
    }

    public void EnableBuildSchedule()
    {
        buildSchedule.SetActive(true);
    }

    private bool IsStartDayValid()
    {
        if (selectedBuildings.list.Count > 0)
            return true;
        return false;
    }

    



}
