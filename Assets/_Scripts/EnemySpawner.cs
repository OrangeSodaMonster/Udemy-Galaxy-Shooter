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

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float noSpawnZoneRadius;   
    [SerializeField] float spawnZoneRadius;
    [SerializeField] Transform player;
    [SerializeField] float baseSpawnCD = 1;
    [SerializeField] float spawnCDVariationPerc = 50f;
    [SerializeField] EnemiesToSpawn[] enemiesToSpawn;
    public EnemiesToSpawn[] EnemiesToSpawn => enemiesToSpawn;
    [SerializeField] EnemiesToSpawnByTime[] enemiesToSpawnByTime;
    public EnemiesToSpawnByTime[] EnemiesToSpawnByTime => enemiesToSpawnByTime;

    float totalSpawnWeight = 0;
    Vector2 nextSpawnDirection;
    Vector3 nextSpawnPoint;
    float timeSinceLastSpawn = float.MaxValue;
    float currentSpawnCD;
    public static Vector3 PlayerLastPos = new();

    static float noSpawnZoneRadiusStatic;
    public static float NoSpawnZoneRadius { get { return noSpawnZoneRadiusStatic; } }
    static float spawnZoneRadiusStatic;
    public static float SpawnZoneRadius { get { return spawnZoneRadiusStatic; } }

    EnemyPoolRef poolRef;
    static EnemyPoolRef s_poolRef;

    private void Awake()
    {      
        poolRef = GetComponent<EnemyPoolRef>();
        noSpawnZoneRadiusStatic = noSpawnZoneRadius;
        spawnZoneRadiusStatic = spawnZoneRadius;

        currentSpawnCD = baseSpawnCD;
        s_poolRef = poolRef;
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
            float randomSpawnValue = UnityEngine.Random.Range(0 + float.Epsilon, totalSpawnWeight - float.Epsilon);
            GameObject nextEnemytoSpawn = GetNextSpawn(randomSpawnValue);
            
          
            Vector3 playerPos = player != null ? player.position : PlayerLastPos;

            GameObject enemy = poolRef.enemyPoolers[nextEnemytoSpawn].GetPooledGameObject();
            enemy.transform.position = nextSpawnPoint + playerPos;
            enemy.SetActive(true);

            timeSinceLastSpawn = 0;
            currentSpawnCD =Mathf.Abs(UnityEngine.Random.Range(baseSpawnCD - baseSpawnCD*(spawnCDVariationPerc/100), baseSpawnCD + baseSpawnCD*(spawnCDVariationPerc/100)));
        }
        timeSinceLastSpawn += Time.deltaTime;

        PlayerLastPos = player != null ? player.position : PlayerLastPos;
    }

    IEnumerator SpawnByTime(GameObject enemyToSpawn, float time)
    {
        yield return new WaitForSeconds(time);

        nextSpawnPoint = nextSpawnDirection * UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius);
        Vector3 playerPos = player != null ? player.position : PlayerLastPos;
        //Instantiate(enemy, nextSpawnPoint + player.position, Quaternion.identity, this.transform);
        GameObject enemy = poolRef.enemyPoolers[enemyToSpawn].GetPooledGameObject();
        enemy.transform.position = nextSpawnPoint + playerPos;
        enemy.SetActive(true);
    }

    GameObject GetNextSpawn(float spawnValue)
    {
        GameObject nextSpawn = null;
        foreach(EnemiesToSpawn enemy in EnemiesToSpawn)
        {
            if (spawnValue <= enemy.spawnWeight)
            {
                nextSpawn = enemy.enemy;
                break;                
            }
            else
                spawnValue -= enemy.spawnWeight;
        }
        return nextSpawn;
    }

    public static void SpawnAsteroid(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply)
    {
        //GameObject newAsteroid = Instantiate(asteroidObject, position, Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward), enemyParentStatic);
        GameObject newAsteroid = s_poolRef.enemyPoolers[asteroidObject].GetPooledGameObject();
        newAsteroid.transform.SetPositionAndRotation(position, Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward));
        newAsteroid.SetActive(true);

        if (newAsteroid.TryGetComponent(out AsteroidMove asteroidMove))
        {
            asteroidMove.SetVelocity(speed, moveDirection);
        }
        if(damageToApply > 0 && newAsteroid.TryGetComponent(out EnemyHP asteroidHP))
        {
            asteroidHP.ChangeHP(-Mathf.Abs(damageToApply));
        }
    }

    public static void SpawnAsteroidParented(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply, Transform parent)
    {
        GameObject newAsteroid = Instantiate(asteroidObject, position, Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward), parent);
        if (newAsteroid.TryGetComponent(out AsteroidMove asteroidMove))
        {
            asteroidMove.SetVelocity(speed, moveDirection);
        }
        if (damageToApply > 0 && newAsteroid.TryGetComponent(out EnemyHP asteroidHP))
        {
            asteroidHP.ChangeHP(-Mathf.Abs(damageToApply));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(player == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, noSpawnZoneRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnZoneRadius);
    }
}