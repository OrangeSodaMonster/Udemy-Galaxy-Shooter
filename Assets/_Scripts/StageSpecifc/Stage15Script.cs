using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage15Script : MonoBehaviour
{
    [SerializeField] GameObject orangeShip;
    [SerializeField] int numOrange = 1;
    [SerializeField] GameObject yellowShip;
    [SerializeField] int numYellow = 2;
    [SerializeField] GameObject greenShip;
    [SerializeField] int numGreen = 2;
    [Space]
    [SerializeField] float spawnInterval = 15;
    [SerializeField] float spawnVariation = 2;
    [SerializeField] float spawnRadius = 2;

    EnemySpawner enemySpawner;
    float spawnTimer = 0;
    float currentSpawnTime;

    private void Start()
    {
        enemySpawner = EnemySpawner.Instance;

        currentSpawnTime = Random.Range(spawnInterval - spawnVariation, spawnInterval + spawnVariation);
    }

    private void Update()
    {
        if(spawnTimer >= currentSpawnTime)
        {
            spawnTimer = 0;
            currentSpawnTime = Random.Range(spawnInterval - spawnVariation, spawnInterval + spawnVariation);
            SpawnSquadron();
        }

        spawnTimer += Time.deltaTime;
    }

    [Button]
    public void SpawnSquadron()
    {
        Vector3 spawnPos = enemySpawner.GetSpawnPoint();
        
        for (int i = 0; i < numOrange; i++)
        {
            enemySpawner.SpawnEnemy(orangeShip, Random.onUnitSphere * spawnRadius + spawnPos);
        }

        for (int i = 0; i < numYellow; i++)
        {
            enemySpawner.SpawnEnemy(yellowShip, Random.onUnitSphere * spawnRadius + spawnPos);
        }

        for (int i = 0; i < numGreen; i++)
        {
            enemySpawner.SpawnEnemy(greenShip, Random.onUnitSphere * spawnRadius + spawnPos);
        }
    }

}
