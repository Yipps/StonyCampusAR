using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerData : ScriptableObject {

    public GameObject playerPointer;
    public NavMeshAgent playerAgent;
    public Transform playerTransform;

    private void OnDisable()
    {
        playerAgent = null;
        playerTransform = null;
        playerPointer = null;
    }
}
