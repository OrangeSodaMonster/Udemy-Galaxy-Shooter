using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHolePull : MonoBehaviour
{
    [SerializeField] float pullMaxForce;
    [SerializeField] AnimationCurve pullCurve;
	List<Rigidbody2D> objsToPull = new List<Rigidbody2D>();
    float radius;

    private void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
        radius *= transform.lossyScale.x;
    }

    void FixedUpdate()
    {
        foreach (var rb in objsToPull)
        {
            if (rb == null)
            {
                objsToPull.Remove(rb);
                return;
            }            

            Vector2 direction = (transform.position - rb.transform.position).normalized;

            float pullForce = Vector2.Distance((Vector2)rb.transform.position, (Vector2)transform.position);
            pullForce = Mathf.Clamp(pullForce / radius, 0, 1);
            pullForce = pullMaxForce * pullCurve.Evaluate(pullForce);

            rb.AddForce(pullForce * direction, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.TryGetComponent(out Rigidbody2D collRB))
            objsToPull.Add(collRB);

       if (collision.TryGetComponent(out CollectiblesPickUps cp))
        {
            collRB.isKinematic = false;
            collRB.interpolation = RigidbodyInterpolation2D.Extrapolate;
            collRB.velocity = cp.CurrentMoveSpeed * cp.MoveDir;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D collRB) && objsToPull.Contains(collRB))      
            objsToPull.Remove(collRB);

        if (collision.GetComponent<CollectiblesPickUps>() != null)
        {
            collRB.isKinematic = true;
            collRB.interpolation = RigidbodyInterpolation2D.None;
        }
    }
}