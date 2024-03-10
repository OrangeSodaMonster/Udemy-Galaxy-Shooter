using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDroneFromShip : MonoBehaviour
{
    [HideInInspector] public float BaseSpawnCD = 9;
    [HideInInspector] public float SpawnCDVariation = 1f;
    [SerializeField] GameObject droneToSpawn;
    public GameObject DroneToSpawn => droneToSpawn;

    float spawnCD;

    void OnEnable()
    {
        spawnCD = BaseSpawnCD;

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
            yield return new WaitForSeconds(spawnCD);

            EnemySpawner.Instance.SpawnDrone(transform.position, droneToSpawn);

            spawnCD = Random.Range(-SpawnCDVariation, SpawnCDVariation) + BaseSpawnCD;

            AudioManager.Instance.DroneSpawnSound.PlayFeedbacks();
        }
    }
}