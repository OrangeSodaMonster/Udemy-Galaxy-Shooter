using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float life = 7f;

    Rigidbody2D rb;
    Vector3 defaultScale = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();       
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        rb.velocity = (transform.up) * moveSpeed;
        transform.localScale = defaultScale;

        StartCoroutine(LaserLifeTime());
    }


    void FixedUpdate()
    {
        //transform.Translate(moveSpeed*Time.deltaTime*transform.up, Space.World);
        transform.up = rb.velocity;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BlackHolePull>() != null || collision.GetComponent<BlackHoleHorizon>() != null) return;

        DestroySequence();
    }

    public void DestroySequence()
    {
        gameObject.SetActive(false);
    }

    IEnumerator LaserLifeTime()
    {
        yield return new WaitForSeconds(life);

        DestroySequence();
    }
}