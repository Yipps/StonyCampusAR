using UnityEngine;
using System.Collections;

public class SunRotate : MonoBehaviour
{



    public Transform player;

    public GameObject sun;
    public GameObject moon;

    public float radius = 6;


    // implementing minecraft PC defaults
    public const float daytimeRLSeconds = 1.0f * 60;
    public const float duskRLSeconds = 0.5f * 60;
    public const float nighttimeRLSeconds = 1.0f * 60;
    public const float sunsetRLSeconds = 0.5f * 60;
    public const float gameDayRLSeconds = daytimeRLSeconds + duskRLSeconds + nighttimeRLSeconds + sunsetRLSeconds;

    public const float startOfDaytime = 0.05f;
    public const float startOfDusk = daytimeRLSeconds / gameDayRLSeconds;
    public const float startOfNighttime = startOfDusk + duskRLSeconds / gameDayRLSeconds;
    public const float startOfSunset = startOfNighttime + nighttimeRLSeconds / gameDayRLSeconds;


    void Start()
    {

    }
    private float timeRT = 0;
    public float TimeOfDay // game time 0 .. 1
    {
        get { return timeRT / gameDayRLSeconds; }
        set { timeRT = value * gameDayRLSeconds; }
    }

    void Update()
    {
        timeRT = (timeRT + Time.deltaTime) % gameDayRLSeconds;
        float sunangle = TimeOfDay * 360;
        float moonangle = TimeOfDay * 360 + 180;
        Vector3 midpoint = new Vector3(0, 10, 0);
        sun.transform.position = midpoint + Quaternion.Euler(0, 0, sunangle) * (radius * Vector3.right);
        sun.transform.LookAt(midpoint);
        moon.transform.position = midpoint + Quaternion.Euler(0, 0, moonangle) * (radius * Vector3.right);
        moon.transform.LookAt(midpoint);
    }
    void OnGUI()
    {
        Rect rect = new Rect(10, 10, 120, 20);
        GUI.Label(rect, "time: " + TimeOfDay); rect.y += 20;
        GUI.Label(rect, "timeRT: " + timeRT);
        rect = new Rect(120, 10, 200, 10);
        TimeOfDay = GUI.HorizontalSlider(rect, TimeOfDay, 0, 1);
    }
}