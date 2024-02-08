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
    public Vector3 PlayerLastPos = new();
    float noSpawnZoneRadiusStatic;
    public float NoSpawnZoneRadius { get { return noSpawnZoneRadiusStatic; } }
    float spawnZoneRadiusStatic;
    public float SpawnZoneRadius { get { return spawnZoneRadiusStatic; } }

    float totalSpawnWeight = 0;
    Vector3 nextSpawnPoint;
    float timeSinceLastSpawn = float.MaxValue;
    float currentSpawnCD;
    EnemyPoolRef poolRef;

    public static EnemySpawner Instance;


    private void Awake()
    {      
        if(Instance == null)
        {
            Instance = this;
        }

        poolRef = GetComponent<EnemyPoolRef>();
        noSpawnZoneRadiusStatic = noSpawnZoneRadius;
        spawnZoneRadiusStatic = spawnZoneRadius;

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
            nextSpawnPoint = GetSpawnPoint();

            //Get Enemy to Spawn
            float randomSpawnValue = UnityEngine.Random.Range(0 + float.Epsilon, totalSpawnWeight - float.Epsilon);
            GameObject nextEnemytoSpawn = GetNextSpawn();
            
          
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

    public Vector3 GetSpawnPoint()
    {
        Vector2 nextSpawnDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        return nextSpawnDirection * UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius);
    }

    IEnumerator SpawnByTime(GameObject enemyToSpawn, float time)
    {
        yield return new WaitForSeconds(time);

        nextSpawnPoint = GetSpawnPoint();
        Vector3 playerPos = player != null ? player.position : PlayerLastPos;
        //Instantiate(enemy, nextSpawnPoint + player.position, Quaternion.identity, this.transform);
        GameObject enemy = poolRef.enemyPoolers[enemyToSpawn].GetPooledGameObject();
        enemy.transform.position = nextSpawnPoint + playerPos;
        enemy.SetActive(true);
    }

    GameObject GetNextSpawn()
    {
        float spawnValue = UnityEngine.Random.Range(0 + float.Epsilon, totalSpawnWeight - float.Epsilon);

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

    public void SpawnAsteroid(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply)
    {
        //GameObject newAsteroid = Instantiate(asteroidObject, position, Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward), enemyParentStatic);
        GameObject newAsteroid = poolRef.enemyPoolers[asteroidObject].GetPooledGameObject();
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

    public void SpawnAsteroidParented(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply, Transform parent)
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

    public void SpawnDrone(Vector3 position, GameObject drone)
    {
        if (poolRef.enemyPoolers.ContainsKey(drone))
        {
            GameObject spawn = poolRef.enemyPoolers[drone].GetPooledGameObject();
            spawn.transform.position = position;
            spawn.SetActive(true);
        }
        else
        {
            Debug.Log("Não há pool para esse drone");
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