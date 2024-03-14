using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage6Specifcs : MonoBehaviour
{
    [SerializeField] EnemiesToLoopSpawn postObjectiveSpawns1;
    [SerializeField] EnemiesToLoopSpawn postObjectiveSpawns2;
    [SerializeField] EnemiesToSpawnByTime timedSpawn;

    EnemySpawner enemySpawner;
    PoolRefs poolRefs;
    RareSpawnScript rareSpawner;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        poolRefs = FindObjectOfType<PoolRefs>();
        rareSpawner = FindObjectOfType<RareSpawnScript>();

        poolRefs.CreatePoolsForObject(postObjectiveSpawns1.enemy, 12);
        poolRefs.CreatePoolsForObject(postObjectiveSpawns2.enemy, 7);
        poolRefs.CreatePoolsForObject(timedSpawn.enemy, 1);

        StartCoroutine(SpawnByTime(timedSpawn));
    }

    public void StartPostObjective()
    {
        StartCoroutine(enemySpawner.SpawnLoop(postObjectiveSpawns1));
        StartCoroutine(enemySpawner.SpawnLoop(postObjectiveSpawns2));
        Debug.Log("POST OBJECTIVE");
    }

    public IEnumerator SpawnByTime(EnemiesToSpawnByTime spawn)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(spawn.timeSec - spawn.timeVarSec, spawn.timeSec + spawn.timeVarSec));

        rareSpawner.SpawnRare(spawn.enemy);

        ////Instantiate(enemy, nextSpawnPoint + player.position, Quaternion.identity, this.transform);
        //GameObject enemy = poolRefs.Poolers[spawn.enemy].GetPooledGameObject();
        //enemy.transform.position = enemySpawner.GetSpawnPoint();
        //enemy.SetActive(true);
    }
}