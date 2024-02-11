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
            // Criar poolers para cada inimigo na lista de spawns
            if (!enemyPoolers.ContainsKey(spawn.enemy))
            {
                enemyPoolers.Add(spawn.enemy, Instantiate(poolerPrefab, transform));
                enemyPoolers[spawn.enemy].PoolSize = (int)Mathf.Ceil(spawn.spawnWeight * 2) + 1;
                enemyPoolers[spawn.enemy].GameObjectToPool = spawn.enemy;

                // criar poolers para drones que as naves inimigas invocam
                if (spawn.enemy.TryGetComponent(out SpawnDroneFromShip spawnDrone))
                {
                    GameObject drone = spawnDrone.DroneToSpawn;

                    enemyPoolers.Add(drone, Instantiate(poolerPrefab, transform));
                    enemyPoolers[drone].PoolSize = (int)Mathf.Ceil(spawn.spawnWeight * 2) + 1;
                    enemyPoolers[drone].GameObjectToPool = drone;
                }
            }
        }
        // Criar poolers para spawns cronometrados
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