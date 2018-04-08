using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour
{
    // The game event instance to register to.
    public GameEvent GameEvent;
    // The unity event responce created for the event.
    public UnityEvent Response;

    public void OnEnabled()
    {
        GameEvent.RegisterListerner(this);
    }

    public void OnDisabled()
    {
        GameEvent.UnregisterListener(this);
    }

    public void RaiseEvent()
    {
        Response.Invoke();
    }
}
