using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnter;

    void OnTriggerEntered2D(Collider col)
    {
        if (onTriggerEnter != null) onTriggerEnter.Invoke(col);
    }
}