using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWanderingAI : AddToList
{
    public WanderingStudentsRuntimeList studentList;

    public override void OnEnable()
    {
        studentList.list.Add(gameObject);
    }

    public override void OnDisable()
    {
        studentList.list.Remove(gameObject);
    }
}
