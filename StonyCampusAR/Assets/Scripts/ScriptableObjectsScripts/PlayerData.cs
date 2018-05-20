using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "playerData")]
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
