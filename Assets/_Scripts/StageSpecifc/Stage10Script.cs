using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10Script : MonoBehaviour
{
	[SerializeField] GameObject firstSpawns;
	[SerializeField] int firstSpawnsNumber;
	[Space]
    [SerializeField] GameObject secondSpawns;
    [SerializeField] int secondSpawnsNumber;
    [Space]
    [SerializeField] GameObject thirdSpawns;
    [SerializeField] int thirdSpawnsNumber;
    [Space]
    [SerializeField] GameObject fourthSpawns;
    [SerializeField] int fourthSpawnsNumber;
    [SerializeField] GameObject bossPrefab;
    [Space]
    [SerializeField] int objsComplete;

    EnemySpawner enemySpawner;
    PoolRefs poolRefs;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        poolRefs = FindObjectOfType<PoolRefs>();

        poolRefs.CreatePoolsForObject(firstSpawns, firstSpawnsNumber);
        poolRefs.CreatePoolsForObject(secondSpawns, secondSpawnsNumber);
        poolRefs.CreatePoolsForObject(thirdSpawns, thirdSpawnsNumber);
        poolRefs.CreatePoolsForObject(fourthSpawns, fourthSpawnsNumber);
        poolRefs.CreatePoolsForObject(bossPrefab, 1);
    }

    public void IncreaseObjCount()
    {
        objsComplete++;

        switch (objsComplete)
        {
            case 1:
                CallFirstSpawn(); break;
            case 2:
                CallSecondSpawn(); break;
            case 3:
                CallThirdSpawn(); break;
            case 4:
                CallFourthSpawn(); break;
            default: break;
        }
    }

    void SpawnWave(GameObject enemy, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject spawn = poolRefs.Poolers[enemy].GetPooledGameObject();
            spawn.transform.position = enemySpawner.GetSpawnPoint360();
            spawn.SetActive(true);
        }
    }

    void CallFirstSpawn()
    {
        SpawnWave(firstSpawns, firstSpawnsNumber);
    }

    void CallSecondSpawn()
    {
        SpawnWave(secondSpawns, secondSpawnsNumber);
    }

    void CallThirdSpawn()
    {
        SpawnWave(thirdSpawns, thirdSpawnsNumber);
    }

    void CallFourthSpawn()
    {
        SpawnWave(fourthSpawns, fourthSpawnsNumber);

        GameObject boss = poolRefs.Poolers[bossPrefab].GetPooledGameObject();
        boss.transform.position = enemySpawner.GetSpawnPoint360();
        boss.SetActive(true);
    }


}