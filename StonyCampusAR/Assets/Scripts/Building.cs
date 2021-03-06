﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Building:MonoBehaviour
{
    public string buildingName;
    public string description;
    public bool isSelected;
    public List<Facility> facilities;
    private List<GameObject> icons;
    private float spacing = 3;
    private MeshRenderer renderer;
    private SelectedBuildingsList selectedBuildingsList;

    public Animator animator;

    private void Awake()
    {
        isSelected = false;
        renderer = this.GetComponentInChildren<MeshRenderer>();
        facilities = new List<Facility>();
        icons = new List<GameObject>();
        selectedBuildingsList = Resources.Load("Runtime Data/SelectedBuildingsList") as SelectedBuildingsList;
    }

    public bool Selected()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            renderer.material.color = Color.green;
            selectedBuildingsList.AddItem(this);
        }
        else
        {
            renderer.material.color = Color.white;
            selectedBuildingsList.RemoveItem(this);
        }
        return isSelected;
    }

    public void ToggleIcon(GameObject icon)
    {


        GameObject _icon = null;
        Boolean isFound = false;
        float offset = 8 + icons.Count * spacing;
        

        foreach (GameObject i in icons)
        {
            //exists in the list
            if (i.name == icon.name)
            {
                _icon = i;
                isFound = true;
                break;
            }
        }

        if (isFound)
        {
            icons.Remove(_icon);
            Destroy(_icon);
            UpdateIconPosition();
        }
        else
        {
            _icon = Instantiate(icon, transform.position + new Vector3(0, offset, 0), Quaternion.identity);
            _icon.name = icon.name;
            icons.Add(_icon);
        }
    }

    private void UpdateIconPosition()
    {
        foreach(GameObject i in icons)
        {
            float offset = 8 + (icons.Count - 1) * spacing;
            Vector3 updatedPosition = new Vector3(i.transform.position.x, offset, i.transform.position.z);
            i.transform.position = updatedPosition;
        }
    }

    public Vector3 GetNavPos()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
        return hit.position;
    }

    public void SpawnAnimation()
    {
        animator.SetTrigger("spawn");
    }

    private void OnDisable()
    {
        selectedBuildingsList.RemoveItem(this);
    }
}



