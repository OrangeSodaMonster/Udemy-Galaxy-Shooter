using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{    
    [SerializeField] UnityEvent onEnable;

    private void OnEnable()
    {
        onEnable?.Invoke();
    }
}