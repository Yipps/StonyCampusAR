using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building:MonoBehaviour
{
    public string buildingName;
    public string description;
    public bool isSelected;
    public List<Facility> facilities;

    private MeshRenderer renderer;

    private void Awake()
    {
        isSelected = false;
        renderer = this.GetComponentInChildren<MeshRenderer>();
        facilities = new List<Facility>();
    }

    public bool Selected()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            renderer.material.color = Color.green;
        }
        else
        {
            renderer.material.color = Color.white;
        }
        return isSelected;
    }
}



