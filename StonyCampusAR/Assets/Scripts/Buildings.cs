using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildings:MonoBehaviour
{

    public bool isSelected;
    private MeshRenderer renderer;

    private void Start()
    {
        isSelected = false;
        renderer = this.GetComponentInChildren<MeshRenderer>();

    }

    public bool Selected()
    {

        isSelected = !isSelected;
        if (isSelected)
        {
            Color color = renderer.material.color;
            color.a = 1f;
            renderer.material.color = new Color(.13f,.75f,.62f,.8f);
        }
        else
        {
            Color color = renderer.material.color;
            color.a = 0.68f;
            renderer.material.color = new Color(.16f,.50f,.70f,.68f);
        }
        return isSelected;

    }

}