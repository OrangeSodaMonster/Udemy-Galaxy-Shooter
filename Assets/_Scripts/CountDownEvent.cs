using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountDownEvent : MonoBehaviour
{
	[SerializeField] float timeUntilCall;

	public UnityEvent OnTimeUp = new();

    private void Start()
    {
        Invoke(nameof(CallEvent), timeUntilCall);
    }

    void CallEvent()
    {
        OnTimeUp?.Invoke();
    }
}