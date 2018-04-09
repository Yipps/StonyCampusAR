using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunTimeList<T> : ScriptableObject
{

    public List<T> list = new List<T>();

    public void AddBuilding(T t)
    {
        if (!list.Contains(t)) list.Add(t);
    }

    public void RemoveBuilding(T t)
    {
        if (list.Contains(t)) list.Remove(t);
    }
}
