using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CrowdSimulation/CurrentDay")]
public class CurrentDay : ScriptableObject {
    public int maxPeriods;
    public int currentPeriod;
}
