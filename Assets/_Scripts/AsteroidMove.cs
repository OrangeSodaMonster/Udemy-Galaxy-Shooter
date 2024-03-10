using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    [SerializeField] bool drawGizmos = false;
    public Vector3 MoveDirection = Vector3.zero;
    public float MoveSpeed = 0;

    [HideInInspector] public float BaseSpeed = 3;
    [HideInInspector] public float SpeedVariationPerc = 25;
    [SerializeField] float moveDirVariation = 0.1f;

    Transform player;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMove>()?.transform;
    }

    void OnEnable()
    {
        MoveSpeed = Mathf.Abs(Random.Range(BaseSpeed - BaseSpeed*(SpeedVariationPerc/100), BaseSpeed + BaseSpeed*(SpeedVariationPerc/100)));

        if (!GameStatus.IsGameover && player != null)
        {
            MoveDirection = player.position - transform.position;
            MoveDirection = MoveDirection.normalized;

            MoveDirection += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * moveDirVariation;
            MoveDirection = MoveDirection.normalized;
        }
        else 
            MoveDirection = (EnemySpawner.Instance.PlayerLastPos - transform.position).normalized;

        rb.velocity = (MoveDirection) * MoveSpeed;
    }

    public void SetVelocity(float speed, Vector2 direction)
    {
        MoveSpeed = speed;
        MoveDirection = direction;
        rb.velocity = direction * speed;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawLine(transform.position, transform.position + (MoveDirection * 100));
        }
    }
}