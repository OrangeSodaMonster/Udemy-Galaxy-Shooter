using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDroneFromShip : MonoBehaviour
{
    [SerializeField] float baseSpawnCD = 9;
    [SerializeField] float spawnCDVariation = 1f;
    [SerializeField] GameObject droneToSpawn;
    public GameObject DroneToSpawn => droneToSpawn;

    float spawnCD;

    void OnEnable()
    {
        spawnCD = baseSpawnCD;

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

            spawnCD = Random.Range(-spawnCDVariation, spawnCDVariation) + baseSpawnCD;

            AudioManager.Instance.DroneSpawnSound.PlayFeedbacks();
        }
    }
}