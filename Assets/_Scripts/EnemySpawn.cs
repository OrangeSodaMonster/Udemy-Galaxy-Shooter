using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct EnemiesToSpawn
{
    public GameObject enemy;
    public float spawnWeight;
}
[Serializable]
public struct EnemiesToSpawnByTime
{
    public GameObject enemy;
    public float timeSec;
}

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] float noSpawnZoneRadius;   
    [SerializeField] float spawnZoneRadius;
    [SerializeField] Transform player;
    [SerializeField] float baseSpawnCD = 1;
    [SerializeField] float spawnCDVariationPerc = 50f;
    [SerializeField] EnemiesToSpawn[] enemiesToSpawn;
    [SerializeField] EnemiesToSpawnByTime[] enemiesToSpawnByTime;

    float totalSpawnWeight = 0;
    Vector2 nextSpawnDirection;
    Vector3 nextSpawnPoint;
    float timeSinceLastSpawn = float.MaxValue;
    float currentSpawnCD;
    static Transform enemyParentStatic;
    public static Vector3 PlayerLastPos = new();

    static float noSpawnZoneRadiusStatic;
    public static float NoSpawnZoneRadius { get { return noSpawnZoneRadiusStatic; } }
    static float spawnZoneRadiusStatic;
    public static float SpawnZoneRadius { get { return spawnZoneRadiusStatic; } }

    private void Awake()
    {
        noSpawnZoneRadiusStatic = noSpawnZoneRadius;
        spawnZoneRadiusStatic = spawnZoneRadius;
        enemyParentStatic = this.transform;

        currentSpawnCD = baseSpawnCD;
    }

    private void Start()
    {
        foreach (var enemy in enemiesToSpawn)
        {
            totalSpawnWeight += enemy.spawnWeight;
        }

        foreach (var spawn in enemiesToSpawnByTime)
        {
            StartCoroutine(SpawnByTime(spawn.enemy, spawn.timeSec));
        }
    }

    void Update()
    {
        if (timeSinceLastSpawn > currentSpawnCD) 
        {
            nextSpawnDirection = new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            nextSpawnPoint = nextSpawnDirection * UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius);

            //Get Enemy to Spawn
            float randomSpawnValue = UnityEngine.Random.Range(0, totalSpawnWeight);
            GameObject nextEnemytoSpawn = null;
            
            int i = 0;
            while (nextEnemytoSpawn == null) 
            {
                if (randomSpawnValue <= enemiesToSpawn[i].spawnWeight)
                    { nextEnemytoSpawn = enemiesToSpawn[i].enemy; }
                else
                {
                    randomSpawnValue -= enemiesToSpawn[i].spawnWeight;
                    i++;
                }
                if (player == null && nextEnemytoSpawn?.GetComponent<DroneMove>() != null || nextEnemytoSpawn?.GetComponent<EnemyShipMove>() != null)
                    nextEnemytoSpawn = null;
            }
            Vector3 playerPos = player != null ? player.position : PlayerLastPos;


            Instantiate(nextEnemytoSpawn, nextSpawnPoint + playerPos, Quaternion.identity, this.transform);
            timeSinceLastSpawn = 0;
            currentSpawnCD =Mathf.Abs(UnityEngine.Random.Range(baseSpawnCD - baseSpawnCD*(spawnCDVariationPerc/100), baseSpawnCD + baseSpawnCD*(spawnCDVariationPerc/100)));
        }
        timeSinceLastSpawn += Time.deltaTime;

        PlayerLastPos = player != null ? player.position : PlayerLastPos;
    }

    IEnumerator SpawnByTime(GameObject enemy, float time)
    {
        yield return new WaitForSeconds(time);

        nextSpawnPoint = nextSpawnDirection * UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius);
        Instantiate(enemy, nextSpawnPoint + player.position, Quaternion.identity, this.transform);
    }

    public static void SpawnAsteroid(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply)
    {
        GameObject newAsteroid = Instantiate(asteroidObject, position, Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward), enemyParentStatic);
        if(newAsteroid.TryGetComponent(out AsteroidMove asteroidMove))
        {
            asteroidMove.MoveDirection = moveDirection;
            asteroidMove.MoveSpeed = speed;
        }
        if(damageToApply > 0 && newAsteroid.TryGetComponent(out EnemyHP asteroidHP))
        {
            asteroidHP.OnBirthDamage = damageToApply;
        }
    }
    public static void SpawnAsteroid(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply, Transform parent)
    {
        GameObject newAsteroid = Instantiate(asteroidObject, position, Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward), parent);
        if (newAsteroid.TryGetComponent(out AsteroidMove asteroidMove))
        {
            asteroidMove.MoveDirection = moveDirection;
            asteroidMove.MoveSpeed = speed;
        }
        if (damageToApply > 0 && newAsteroid.TryGetComponent(out EnemyHP asteroidHP))
        {
            asteroidHP.OnBirthDamage = damageToApply;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, noSpawnZoneRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnZoneRadius);
    }
}