using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreAI : ScriptableObject {

    public virtual void Init(StudentAIController ai) { }
    public abstract void Think(StudentAIController ai);
}
