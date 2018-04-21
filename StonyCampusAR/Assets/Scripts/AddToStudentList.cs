using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToStudentList : MonoBehaviour {
    public StudentRuntimeList studentList;


    void OnEnabled()
    {
        studentList.list.Add(gameObject);
    }
}
