using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyByDistance : MonoBehaviour
{
    Transform player;
    float deSpawnZoneRadius;
    WaitForSeconds wait = new WaitForSeconds(.5f);
    Vector3 playerPos = new();

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMove>()?.transform;
    }

    void OnEnable()
    {
        if (GameStatus.IsGameover || GameStatus.IsStageClear) player = null;
        deSpawnZoneRadius = EnemySpawner.Instance.SpawnZoneRadius * 1.5f;
        deSpawnZoneRadius *= deSpawnZoneRadius;

        StartCoroutine(DistanceCheckFrequency());
    }

    IEnumerator DistanceCheckFrequency()
    {
        while (true)
        {
            yield return wait;

            playerPos = player != null ? player.position : EnemySpawner.Instance.PlayerLastPos;
            if (Vector2.SqrMagnitude(transform.position - playerPos) > deSpawnZoneRadius)
            {
                gameObject.SetActive(false);
            }
        }
    }
}