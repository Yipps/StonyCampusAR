using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGoingToClassAI : AddToList
{
    public GoingToClassStudentsRuntimeList studentList;

    public override void OnEnable()
    {
        studentList.list.Add(gameObject);
    }

    public override void OnDisable()
    {
        studentList.list.Remove(gameObject);
    }
}
