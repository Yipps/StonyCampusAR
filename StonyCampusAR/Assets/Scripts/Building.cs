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

    //private Dictionary<String,GameObject> icons;
    private List<GameObject> icons;
    private float spacing = 3;
    


    private MeshRenderer renderer;

    private void Awake()
    {
        isSelected = false;
        renderer = this.GetComponentInChildren<MeshRenderer>();
        facilities = new List<Facility>();
        //icons = new Dictionary<string, GameObject>();
        icons = new List<GameObject>();
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

    //public void ShowFacility(GameObject icon)
    //{
    //    Instantiate(icon, this.transform.position + new Vector3(0,10,0), Quaternion.identity);
    //}

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
}



