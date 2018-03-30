using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatLongMapping : MonoBehaviour {

    public Vector2[] coordinates;
    public GameObject[] gizmos;
    public float xMax = 43f;
    public float yMax = 36f;

    float longMin = -73.12729f;
    float longMax = -73.11886f;
    float latMin = 40.91123f;
    float latMax = 40.91647f;




    // Use this for initialization
    void Start () {
        coordinates = new Vector2[4];
        //gizmos = new GameObject[4];
    }

    private void Update()
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            Vector3 postion = PingMap(coordinates[i]);
            gizmos[i].transform.localPosition = postion;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (GameObject i in gizmos)
        {
            Gizmos.DrawWireSphere(i.transform.position,10);
        }
            
            
    }

    public Vector3 PingMap(Vector2 coord)
    {
        float normLat = 2*(coord.y - latMin) / (latMax - latMin)-1;
        float normLong = 2*(coord.x - longMin) / (longMax - longMin)-1;

        float x = normLong * xMax;
        float y = normLat * yMax;

        return new Vector3(x, 3, y);

    }
}
