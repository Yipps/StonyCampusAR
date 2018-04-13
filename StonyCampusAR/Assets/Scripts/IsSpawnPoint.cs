using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSpawnPoint : MonoBehaviour
{
    public SpawnLocationList list;

    private void OnEnable()
    {
        list.AddItem(transform);
    }

    private void OnDisable()
    {
        list.RemoveItem(transform);
    }
}
