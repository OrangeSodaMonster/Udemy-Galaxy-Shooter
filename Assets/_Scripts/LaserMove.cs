using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    void FixedUpdate()
    {
        transform.Translate(moveSpeed*Time.deltaTime*transform.up, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroySequence();
    }

    public void DestroySequence()
    {
        Destroy(gameObject);
    }
}