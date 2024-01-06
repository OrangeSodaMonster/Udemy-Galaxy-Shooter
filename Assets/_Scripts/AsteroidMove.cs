using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    [SerializeField] bool drawGizmos = false;
    public Vector3 MoveDirection = Vector3.zero;
    public float MoveSpeed = 0;

    [SerializeField] float baseSpeed = 3;
    [SerializeField] float speedVariationPerc = 25;
    [SerializeField] float moveDirVariation = 0.1f;

    Transform player;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerMove>().transform;

        if (MoveSpeed == 0)
            MoveSpeed = Mathf.Abs(Random.Range(baseSpeed - baseSpeed*(speedVariationPerc/100), baseSpeed + baseSpeed*(speedVariationPerc/100)));
        //print(baseSpeed);

        if (player != null & MoveDirection == Vector3.zero)
        {
            MoveDirection = player.position - transform.position;
            MoveDirection = MoveDirection.normalized;

            MoveDirection += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * moveDirVariation;
            MoveDirection = MoveDirection.normalized;
        }
        else if (MoveDirection == Vector3.zero)
            MoveDirection = new Vector3(Random.value, Random.value, 0).normalized;

        rb.velocity = (MoveDirection) * MoveSpeed;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawLine(transform.position, transform.position + (MoveDirection * 100));
        }
    }
}