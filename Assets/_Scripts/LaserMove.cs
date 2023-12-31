using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    private void OnEnable()
    {
        rb.velocity = (transform.up) * moveSpeed;
    }


    void FixedUpdate()
    {
        //transform.Translate(moveSpeed*Time.deltaTime*transform.up, Space.World);
        transform.up = rb.velocity;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BlackHolePull>() != null) return;

        DestroySequence();
    }

    public void DestroySequence()
    {
        Destroy(gameObject);
    }
}