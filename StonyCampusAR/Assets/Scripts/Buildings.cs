using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildings:MonoBehaviour
{

    public bool isSelected;
    MeshRenderer renderer;

    private void Start()
    {
        isSelected = false;
        renderer = this.GetComponentInChildren<MeshRenderer>();

    }

    public void Selected()
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

    }

}