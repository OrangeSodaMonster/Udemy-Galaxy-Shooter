using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHolePull : MonoBehaviour
{
    [SerializeField] float pullMaxForce;
    [SerializeField] AnimationCurve pullCurve;
    [field: SerializeField] public float CollectiblePullForce = 7;
    [field: SerializeField] public float TimeToMaxPullCollec = 3;
    [SerializeField] List<Rigidbody2D> objsToIgnore;

	List<Rigidbody2D> objsToPull = new List<Rigidbody2D>();
    float radius;

    private void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
        radius *= transform.lossyScale.x;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < objsToPull.Count; i++)
        //foreach (var rb in objsToPull)
        {
            if (objsToPull[i] == null)
            {
                objsToPull.Remove(objsToPull[i]);
                return;
            }

            if (objsToIgnore.Contains(objsToPull[i])) continue;

            Vector2 direction = (transform.position - objsToPull[i].transform.position).normalized;

            float pullForce = Vector2.Distance((Vector2)objsToPull[i].transform.position, (Vector2)transform.position);
            pullForce = Mathf.Clamp(pullForce / radius, 0, 1);
            pullForce = pullMaxForce * pullCurve.Evaluate(pullForce);

            objsToPull[i].AddForce(pullForce * direction, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.TryGetComponent(out Rigidbody2D collRB) && !collRB.isKinematic)
            objsToPull.Add(collRB);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D collRB) && objsToPull.Contains(collRB))      
            objsToPull.Remove(collRB);
    }
}