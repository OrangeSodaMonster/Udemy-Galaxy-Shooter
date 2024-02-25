using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDroneMove : MonoBehaviour
{
    [SerializeField] float baseSpeed = 4;
    [SerializeField] float speedVariationPerc = 10;

    Transform player;
    float MoveSpeed = 0;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (MoveSpeed == 0)
            MoveSpeed = Mathf.Abs(Random.Range(baseSpeed - baseSpeed*(speedVariationPerc/100), baseSpeed + baseSpeed*(speedVariationPerc/100)));

        player = FindAnyObjectByType<PlayerMove>()?.transform;
    }

    private void FixedUpdate()
    {
        //Vector3 playerPos = player != null ? player.position : EnemySpawn.PlayerLastPos;
        if(!GameStatus.IsGameover && !GameStatus.IsStageClear && player != null)
            rb.rotation = Vector2.SignedAngle(Vector2.up, player.position - transform.position);

        Vector2 velocity = transform.InverseTransformDirection(rb.velocity);
        velocity.y = MoveSpeed;

        if (velocity.x >= float.Epsilon)
            velocity.x = Mathf.Clamp(velocity.x - baseSpeed * 0.2f * Time.fixedDeltaTime, 0, baseSpeed * 0.5f);
        else if (velocity.x <= float.Epsilon)
            velocity.x = Mathf.Clamp(velocity.x + baseSpeed * 0.2f * Time.fixedDeltaTime, -baseSpeed * 0.5f, 0);

        rb.velocity = transform.TransformDirection(velocity);
    }
}