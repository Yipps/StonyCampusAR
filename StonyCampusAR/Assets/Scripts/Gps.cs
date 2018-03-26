﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gps : MonoBehaviour {
    public float latitude;
    public float longitude;
    public bool hasCoordinates;
    bool isUnityRemote = true;

    private float xMax = 43f;
    private float yMax = 36f;
    private float longMin = -73.12729f;
    private float longMax = -73.11886f;
    private float latMin = 40.91123f;
    private float latMax = 40.91647f;


    private void Start()
    {
        StartCoroutine(StartLocationService(50, 600));
    }

    private void Update()
    {

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 500, 20), "Coordinates:" + latitude + "," + longitude);
    }

    private IEnumerator StartLocationService(float desiredAccuracyInMeters, float updateDistanceInMeters)
    {
        // Wait until the editor and unity remote are connected before starting a location service
        if (isUnityRemote)
        {
            yield return new WaitForSeconds(5);
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("No locations enabled in the device");
            yield break;
        }

        // Start service before querying location
        Input.location.Start(desiredAccuracyInMeters,updateDistanceInMeters);

        if (isUnityRemote)
        {
            yield return new WaitForSeconds(5);
        }

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Service didn't initialize in 20 seconds");
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        // Access granted and location value could be retrieved
        else
        {
            hasCoordinates = true;
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            InitYouAreHere();
        }
    }

    public Vector3 PingMap()
    {
        float normLat = 2 * (latitude - latMin) / (latMax - latMin) - 1;
        float normLong = 2 * (longitude - longMin) / (longMax - longMin) - 1;

        float x = normLong * xMax;
        float z = normLat * yMax;

        return new Vector3(x, 0, z);
    }

    public void InitYouAreHere()
    {
        if (!hasCoordinates)
            return;

        Vector3 pos = PingMap();

        GameObject userLocation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        userLocation.transform.position = pos;
    }
}
