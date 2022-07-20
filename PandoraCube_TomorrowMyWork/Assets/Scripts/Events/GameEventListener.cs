using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent EventSO;
    public UnityEvent Response;

    private void OnEnable()
    {
        EventSO.AddListener(this);
    }

    private void OnDisable()
    {
        EventSO.RemoveListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
