using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5Script : MonoBehaviour
{
	[SerializeField] GameObject bossPref;
	[SerializeField] EnemiesToLoopSpawn postObjectiveSpawns;
    [SerializeField] bool spawnEnemiesFromStart = false;

    EnemySpawner enemySpawner;    
    PoolRefs poolRefs;    

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        poolRefs = FindObjectOfType<PoolRefs>();
        poolRefs.CreatePoolsForObject(bossPref, 1);
        poolRefs.CreatePoolsForObject(postObjectiveSpawns.enemy, 2);

        if (spawnEnemiesFromStart)
        {
            StartPostObjective();
        }
    }

    public void SpawnBoss()
    {
        Debug.Log(poolRefs.Poolers[bossPref]);
        GameObject boss = poolRefs.Poolers[bossPref].GetPooledGameObject();
        boss.transform.position = enemySpawner.GetSpawnPoint360();
        boss.SetActive(true);
    }

    public void StartPostObjective()
    {
        StartCoroutine(enemySpawner.SpawnLoop(postObjectiveSpawns));
        Debug.Log("POST OBJECTIVE");
    }


}