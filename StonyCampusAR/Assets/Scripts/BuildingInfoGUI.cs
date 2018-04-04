using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoGUI : MonoBehaviour {

    public Image[] images;
    public Text title;
    public Text description;

	public void LoadInfo(Building building)
    {
        Debug.Log(building.name);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Campus Images/Buildings/" + building.buildingName);
        Debug.Log(sprites.Length);
        title.text = building.buildingName;

        for (int i = 0; i < sprites.Length; i++)
        {
            images[i].sprite = sprites[i];
        }
    }

    public void CloseInfoWindow()
    {
        Destroy(gameObject);
        GameManager.instance.isTouchHoldTutDone = true;
    }
}
