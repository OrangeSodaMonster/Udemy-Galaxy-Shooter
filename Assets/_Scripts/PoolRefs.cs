using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjToPool
{
    public GameObject Obj;
    public int Count;
}

public class PoolRefs : MonoBehaviour
{
    [SerializeField] MMSimpleObjectPooler poolerPrefab;
    [SerializeField] MMSimpleObjectPooler hpBarPool;
    [Space]
    [SerializeField] List<ObjToPool> manualObjToMakePools;

    public static MMSimpleObjectPooler s_hpBarPool;
    //[SerializeField] MMSimpleObjectPooler projectilePool;
    //public static MMSimpleObjectPooler s_projectilePool;

    EnemySpawner enemySpawn;
    
    public Dictionary<GameObject, MMSimpleObjectPooler> Poolers = new();

    private void Awake()
    {
        if (s_hpBarPool == null)
            s_hpBarPool = hpBarPool;
        //if (s_projectilePool == null)
        //    s_projectilePool = projectilePool;

        enemySpawn = GetComponent<EnemySpawner>();

        foreach (EnemiesToSpawn spawn in enemySpawn.EnemiesToSpawn)
        {
            // Criar poolers para cada inimigo na lista de spawns
            if (!Poolers.ContainsKey(spawn.enemy))
            {
                Poolers.Add(spawn.enemy, Instantiate(poolerPrefab, transform));
                Poolers[spawn.enemy].PoolSize = (int)Mathf.Ceil(spawn.spawnWeight * 2) + 1;
                Poolers[spawn.enemy].GameObjectToPool = spawn.enemy;

                // criar poolers para drones que as naves inimigas invocam
                if (spawn.enemy.TryGetComponent(out SpawnDroneFromShip spawnDrone))
                {
                    GameObject drone = spawnDrone.DroneToSpawn;

                    if (!Poolers.ContainsKey(drone))
                    {
                        Poolers.Add(drone, Instantiate(poolerPrefab, transform));
                        Poolers[drone].PoolSize = (int)Mathf.Ceil(spawn.spawnWeight * 2) + 1;
                        Poolers[drone].GameObjectToPool = drone;
                    }
                }

                // pools para projectiles
                if (spawn.enemy.TryGetComponent(out EnemyProjectileShoot shooter))
                {
                    GameObject projectile = shooter.projectilePref;

                    if (!Poolers.ContainsKey(projectile))
                    {
                        Poolers.Add(projectile, Instantiate(poolerPrefab, transform));
                        Poolers[projectile].PoolSize = 6;
                        Poolers[projectile].GameObjectToPool = projectile;
                    }
                }

                if (spawn.enemy.TryGetComponent(out AsteroidSplit split))
                {
                    GameObject asteroid = split.AsteroidToSplitInto;

                    if (!Poolers.ContainsKey(asteroid))
                    {
                        Poolers.Add(asteroid, Instantiate(poolerPrefab, transform));
                        Poolers[asteroid].PoolSize = (int)spawn.spawnWeight * 3;
                        Poolers[asteroid].GameObjectToPool = asteroid;
                    }

                    if (asteroid.TryGetComponent(out AsteroidSplit split2))
                    {
                        GameObject asteroid2 = split2.AsteroidToSplitInto;

                        if (!Poolers.ContainsKey(asteroid2))
                        {
                            Poolers.Add(asteroid2, Instantiate(poolerPrefab, transform));
                            Poolers[asteroid2].PoolSize = (int)spawn.spawnWeight * 9;
                            Poolers[asteroid2].GameObjectToPool = asteroid2;
                        }
                    }
                }

            }
        }
        // Criar poolers para spawns cronometrados
        foreach (EnemiesToSpawnByTime spawn in enemySpawn.EnemiesToSpawnByTime)
        {
            if (!Poolers.ContainsKey(spawn.enemy))
            {
                Poolers.Add(spawn.enemy, Instantiate(poolerPrefab, transform));
                Poolers[spawn.enemy].PoolSize = 2;
                Poolers[spawn.enemy].GameObjectToPool = spawn.enemy;
            }

            if (spawn.enemy.TryGetComponent(out SpawnDroneFromShip spawnDrone))
            {
                GameObject drone = spawnDrone.DroneToSpawn;

                if (!Poolers.ContainsKey(drone))
                {
                    Poolers.Add(drone, Instantiate(poolerPrefab, transform));
                    Poolers[drone].PoolSize = 2;
                    Poolers[drone].GameObjectToPool = drone;
                }
            }

            if (spawn.enemy.TryGetComponent(out EnemyProjectileShoot shooter))
            {
                GameObject projectile = shooter.projectilePref;

                if (!Poolers.ContainsKey(projectile))
                {
                    Poolers.Add(projectile, Instantiate(poolerPrefab, transform));
                    Poolers[projectile].PoolSize = 3;
                    Poolers[projectile].GameObjectToPool = projectile;
                }
            }

            if (spawn.enemy.TryGetComponent(out AsteroidSplit split))
            {
                GameObject asteroid = split.AsteroidToSplitInto;

                if (!Poolers.ContainsKey(asteroid))
                {
                    Poolers.Add(asteroid, Instantiate(poolerPrefab, transform));
                    Poolers[asteroid].PoolSize = 3;
                    Poolers[asteroid].GameObjectToPool = asteroid;
                }

                if (asteroid.TryGetComponent(out AsteroidSplit split2))
                {
                    GameObject asteroid2 = split2.AsteroidToSplitInto;

                    if (!Poolers.ContainsKey(asteroid2))
                    {
                        Poolers.Add(asteroid2, Instantiate(poolerPrefab, transform));
                        Poolers[asteroid2].PoolSize = 9;
                        Poolers[asteroid2].GameObjectToPool = asteroid2;
                    }
                }
            }
        }
        // Criar poolers para spawns em loop

        foreach (EnemiesToLoopSpawn spawn in enemySpawn.EnemiesToLoopSpawn)
        {
            if (!Poolers.ContainsKey(spawn.enemy))
            {
                Poolers.Add(spawn.enemy, Instantiate(poolerPrefab, transform));
                Poolers[spawn.enemy].PoolSize = 2;
                Poolers[spawn.enemy].GameObjectToPool = spawn.enemy;
            }

            if (spawn.enemy.TryGetComponent(out SpawnDroneFromShip spawnDrone))
            {
                GameObject drone = spawnDrone.DroneToSpawn;

                if (!Poolers.ContainsKey(drone))
                {
                    Poolers.Add(drone, Instantiate(poolerPrefab, transform));
                    Poolers[drone].PoolSize = 2;
                    Poolers[drone].GameObjectToPool = drone;
                }
            }

            if (spawn.enemy.TryGetComponent(out EnemyProjectileShoot shooter))
            {
                GameObject projectile = shooter.projectilePref;

                if (!Poolers.ContainsKey(projectile))
                {
                    Poolers.Add(projectile, Instantiate(poolerPrefab, transform));
                    Poolers[projectile].PoolSize = 3;
                    Poolers[projectile].GameObjectToPool = projectile;
                }
            }

            if (spawn.enemy.TryGetComponent(out AsteroidSplit split))
            {
                GameObject asteroid = split.AsteroidToSplitInto;

                if (!Poolers.ContainsKey(asteroid))
                {
                    Poolers.Add(asteroid, Instantiate(poolerPrefab, transform));
                    Poolers[asteroid].PoolSize = 6;
                    Poolers[asteroid].GameObjectToPool = asteroid;
                }

                if (asteroid.TryGetComponent(out AsteroidSplit split2))
                {
                    GameObject asteroid2 = split2.AsteroidToSplitInto;

                    if (!Poolers.ContainsKey(asteroid2))
                    {
                        Poolers.Add(asteroid2, Instantiate(poolerPrefab, transform));
                        Poolers[asteroid2].PoolSize = 9;
                        Poolers[asteroid2].GameObjectToPool = asteroid2;
                    }
                }
            }
        }

        // Pooler manuais
        foreach (ObjToPool obj in manualObjToMakePools)
        {
            // Criar poolers para cada inimigo na lista de spawns
            if (!Poolers.ContainsKey(obj.Obj))
            {
                Poolers.Add(obj.Obj, Instantiate(poolerPrefab, transform));
                Poolers[obj.Obj].PoolSize = obj.Count;
                Poolers[obj.Obj].GameObjectToPool = obj.Obj;
            }
        }

        foreach (ObjToPool obj in manualObjToMakePools)
        {
            if (!Poolers.ContainsKey(obj.Obj))
            {
                Poolers.Add(obj.Obj, Instantiate(poolerPrefab, transform));
                Poolers[obj.Obj].PoolSize = obj.Count;
                Poolers[obj.Obj].GameObjectToPool = obj.Obj;
            }

            if (obj.Obj.TryGetComponent(out SpawnDroneFromShip spawnDrone))
            {
                GameObject drone = spawnDrone.DroneToSpawn;

                if (!Poolers.ContainsKey(drone))
                {
                    Poolers.Add(drone, Instantiate(poolerPrefab, transform));
                    Poolers[drone].PoolSize = obj.Count * 2;
                    Poolers[drone].GameObjectToPool = drone;
                }
            }

            if (obj.Obj.TryGetComponent(out EnemyProjectileShoot shooter))
            {
                GameObject projectile = shooter.projectilePref;

                if (!Poolers.ContainsKey(projectile))
                {
                    Poolers.Add(projectile, Instantiate(poolerPrefab, transform));
                    Poolers[projectile].PoolSize = obj.Count * 3;
                    Poolers[projectile].GameObjectToPool = projectile;
                }
            }

            if (obj.Obj.TryGetComponent(out AsteroidSplit split))
            {
                GameObject asteroid = split.AsteroidToSplitInto;

                if (!Poolers.ContainsKey(asteroid))
                {
                    Poolers.Add(asteroid, Instantiate(poolerPrefab, transform));
                    Poolers[asteroid].PoolSize = obj.Count * 3;
                    Poolers[asteroid].GameObjectToPool = asteroid;
                }

                if (asteroid.TryGetComponent(out AsteroidSplit split2))
                {
                    GameObject asteroid2 = split2.AsteroidToSplitInto;

                    if (!Poolers.ContainsKey(asteroid2))
                    {
                        Poolers.Add(asteroid2, Instantiate(poolerPrefab, transform));
                        Poolers[asteroid2].PoolSize = obj.Count * 9;
                        Poolers[asteroid2].GameObjectToPool = asteroid2;
                    }
                }
            }
        }
    }

    public void CreatePoolsForObject(GameObject obj, int size)
    {       
        if (!Poolers.ContainsKey(obj))
        {
            Poolers.Add(obj, Instantiate(poolerPrefab, transform));
            Poolers[obj].PoolSize = size;
            Poolers[obj].GameObjectToPool = obj;
            Poolers[obj].FillObjectPool();
        }

        if (obj.TryGetComponent(out SpawnDroneFromShip spawnDrone))
        {
            GameObject drone = spawnDrone.DroneToSpawn;

            if (!Poolers.ContainsKey(drone))
            {
                Poolers.Add(drone, Instantiate(poolerPrefab, transform));
                Poolers[drone].PoolSize = size + 1;
                Poolers[drone].GameObjectToPool = drone;
                Poolers[drone].FillObjectPool();
            }
        }

        if (obj.TryGetComponent(out EnemyProjectileShoot shooter))
        {
            GameObject projectile = shooter.projectilePref;

            if (!Poolers.ContainsKey(projectile))
            {
                Poolers.Add(projectile, Instantiate(poolerPrefab, transform));
                Poolers[projectile].PoolSize = size + 1;
                Poolers[projectile].GameObjectToPool = projectile;
                Poolers[projectile].FillObjectPool();
            }
        }
    }
}