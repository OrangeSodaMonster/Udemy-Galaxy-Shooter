using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolRef : MonoBehaviour
{
    [SerializeField] MMSimpleObjectPooler poolerPrefab;
    [SerializeField] MMSimpleObjectPooler hpBarPool;
    public static MMSimpleObjectPooler s_hpBarPool;
    [SerializeField] MMSimpleObjectPooler projectilePool;
    public static MMSimpleObjectPooler s_projectilePool;

    EnemySpawner enemySpawn;
    public Dictionary<GameObject, MMSimpleObjectPooler> enemyPoolers = new();

    private void Awake()
    {
        if (s_hpBarPool == null)
            s_hpBarPool = hpBarPool;
        if (s_projectilePool == null)
            s_projectilePool = projectilePool;

        enemySpawn = GetComponent<EnemySpawner>();

        foreach(EnemiesToSpawn spawn in enemySpawn.EnemiesToSpawn)
        {
            if (!enemyPoolers.ContainsKey(spawn.enemy))
            {
                enemyPoolers.Add(spawn.enemy, Instantiate(poolerPrefab, transform));
                enemyPoolers[spawn.enemy].PoolSize = (int)Mathf.Ceil(spawn.spawnWeight * 2);
                enemyPoolers[spawn.enemy].GameObjectToPool = spawn.enemy;
            }
        }
        foreach (EnemiesToSpawnByTime spawn in enemySpawn.EnemiesToSpawnByTime)
        {
            if (!enemyPoolers.ContainsKey(spawn.enemy))
            {
                enemyPoolers.Add(spawn.enemy, Instantiate(poolerPrefab, transform));
                enemyPoolers[spawn.enemy].PoolSize = 2;
                enemyPoolers[spawn.enemy].GameObjectToPool = spawn.enemy;
            }
        }
    }
}