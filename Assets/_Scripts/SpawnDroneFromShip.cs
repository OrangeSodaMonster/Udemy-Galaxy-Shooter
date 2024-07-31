using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDroneFromShip : MonoBehaviour
{
    public float BaseSpawnCD = 9;
    public float SpawnCDVariation = 1f;
    [SerializeField] GameObject droneToSpawn;

    WaitForSeconds wait;
    public GameObject DroneToSpawn => droneToSpawn;

    float spawnCD;
    float stopSpawnDistance;
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        stopSpawnDistance = EnemySpawner.Instance.SpawnZoneRadius * 1.5f;
        stopSpawnDistance *= stopSpawnDistance;
    }

    void OnEnable()
    {
        spawnCD = BaseSpawnCD;
        wait = new WaitForSeconds(spawnCD);

        StartCoroutine(SpawnDroneRoutine());
    }

    void Update()
    {
        if (GameStatus.IsGameover || GameStatus.IsStageClear)
            StopAllCoroutines();
    }


    IEnumerator SpawnDroneRoutine()
    {
        while (true)
        {
            yield return wait;

            if (Vector2.SqrMagnitude(player.position - transform.position) < stopSpawnDistance)
            {
                EnemySpawner.Instance.SpawnDrone(transform.position, droneToSpawn);
                AudioManager.Instance.DroneSpawnSound.PlayFeedbacks();
                spawnCD = Random.Range(-SpawnCDVariation, SpawnCDVariation) + BaseSpawnCD;
            }
            
        }
    }
}