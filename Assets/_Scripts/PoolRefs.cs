using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[Serializable]
public struct ObjToPool
{
    [HorizontalGroup("0", 0.12f), PreviewField(38, Alignment = ObjectFieldAlignment.Left), HideLabel]
    public GameObject Obj;
    [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), ReadOnly]
    public string Name;
    [VerticalGroup("0/1"), LabelWidth(40)]
    public int Count;
}

public class PoolRefs : MonoBehaviour
{
    [SerializeField] MMSimpleObjectPooler poolerPrefab;
    [SerializeField] MMSimpleObjectPooler hpBarPool;
    [Space]
    [SerializeField] List<ObjToPool> manualObjToMakePools;
    public List<ObjToPool> ManualObjToMakePools { get { return manualObjToMakePools; } set { manualObjToMakePools = value; } }

    public static MMSimpleObjectPooler s_hpBarPool;

    EnemySpawner enemySpawn;
    
    public Dictionary<GameObject, MMSimpleObjectPooler> Poolers = new();

    private void Awake()
    {
        if (s_hpBarPool == null)
            s_hpBarPool = hpBarPool;

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
                CreatePoolsForObject(obj.Obj, obj.Count);
            }
        }
    }

    public void CreatePoolsForObject(GameObject obj, int size, bool canExpand = true)
    {       
        if (!Poolers.ContainsKey(obj))
        {
            Poolers.Add(obj, Instantiate(poolerPrefab, transform));
            Poolers[obj].PoolSize = size;
            Poolers[obj].GameObjectToPool = obj;
            Poolers[obj].FillObjectPool();
            Poolers[obj].PoolCanExpand = canExpand;
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

        if (obj.TryGetComponent(out AsteroidSplit split))
        {
            GameObject asteroid = split.AsteroidToSplitInto;

            if (!Poolers.ContainsKey(asteroid))
            {
                Poolers.Add(asteroid, Instantiate(poolerPrefab, transform));
                Poolers[asteroid].PoolSize = size * 3;
                Poolers[asteroid].GameObjectToPool = asteroid;
                Poolers[asteroid].FillObjectPool();
            }

            if (asteroid.TryGetComponent(out AsteroidSplit split2))
            {
                GameObject asteroid2 = split2.AsteroidToSplitInto;

                if (!Poolers.ContainsKey(asteroid2))
                {
                    Poolers.Add(asteroid2, Instantiate(poolerPrefab, transform));
                    Poolers[asteroid2].PoolSize = size * 9;
                    Poolers[asteroid2].GameObjectToPool = asteroid2;
                    Poolers[asteroid2].FillObjectPool();
                }
            }
        }
    }

    private void OnValidate()
    {
        for(int i = 0; i < manualObjToMakePools.Count; i++)
        {
            if (manualObjToMakePools[i].Obj != null)
            {
                ObjToPool otp = manualObjToMakePools[i];
                otp.Name = otp.Obj.name;
                manualObjToMakePools[i] = otp;
            }                
        }
    }
}