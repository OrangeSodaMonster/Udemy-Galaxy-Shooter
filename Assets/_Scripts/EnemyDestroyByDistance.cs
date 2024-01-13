using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyByDistance : MonoBehaviour
{

    Transform player;
    float deSpawnZoneRadius;

    void OnEnable()
    {
        player = FindAnyObjectByType<PlayerMove>()?.transform;
        deSpawnZoneRadius = EnemySpawner.SpawnZoneRadius * 1.5f;
        deSpawnZoneRadius *= deSpawnZoneRadius;

        StartCoroutine(DistanceCheckFrequency());
    }

    IEnumerator DistanceCheckFrequency()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);


            Vector3 playerPos = player != null ? player.position : EnemySpawner.PlayerLastPos;
            if (Vector2.SqrMagnitude(transform.position - playerPos) > deSpawnZoneRadius)
            {
                gameObject.SetActive(false);
            }
        }
    }
}