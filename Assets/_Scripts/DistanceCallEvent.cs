using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceCallEvent : MonoBehaviour
{
	[SerializeField] float distanceToCallEvent;
	public UnityEvent OnDistanceCheck;

    float distanceSqr;
    Transform player;
    bool eventCalled;
    private void Start()
    {
        distanceSqr = distanceToCallEvent * distanceToCallEvent;
        player = FindObjectOfType<PlayerMove>().transform;
    }

    private void Update()
    {
        if (eventCalled) return;

        if(Vector2.SqrMagnitude(transform.position - player.position) <= distanceSqr)
        {
            OnDistanceCheck?.Invoke();
            eventCalled = true;

            Debug.Log("DISTANCE CALL");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceToCallEvent);
    }
}