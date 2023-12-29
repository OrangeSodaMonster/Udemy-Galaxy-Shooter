using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour
{
    [SerializeField] float baseSpeed = 4;
    [SerializeField] float speedVariationPerc = 10;

    Transform player;
    float MoveSpeed = 0;

    void Start()
    {
        if (MoveSpeed == 0)
            MoveSpeed = Mathf.Abs(Random.Range(baseSpeed - baseSpeed*(speedVariationPerc/100), baseSpeed + baseSpeed*(speedVariationPerc/100)));

        player = FindAnyObjectByType<PlayerMove>().transform;
    }

    private void Update()
    {
        transform.up = player.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.position, MoveSpeed * Time.deltaTime);
    }
}