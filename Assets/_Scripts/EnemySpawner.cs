using Sirenix.OdinInspector;
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
[Serializable]
public struct EnemiesToLoopSpawn
{
    public GameObject enemy;
    [HorizontalGroup("G")]
    public float timeSec;
    [HorizontalGroup("G")]
    public float timeVarSec;
    [HorizontalGroup("G")]
    public float timeToStart;

    [HideInInspector] public WaitForSeconds[] waits;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float noSpawnZoneRadius;   
    [SerializeField] float spawnZoneRadius;
    [SerializeField] Transform player;
    [SerializeField] float baseSpawnCD = 1;
    [SerializeField] float spawnCDVariationPerc = 50f;
    [SerializeField] float timeToStartSpawning = 3f;
    [SerializeField] EnemiesToSpawn[] enemiesToSpawn;
    public EnemiesToSpawn[] EnemiesToSpawn => enemiesToSpawn;
    [SerializeField] EnemiesToSpawnByTime[] enemiesToSpawnByTime;
    public EnemiesToSpawnByTime[] EnemiesToSpawnByTime => enemiesToSpawnByTime;
    public EnemiesToLoopSpawn[] EnemiesToLoopSpawn;
    [HideInInspector] public Vector3 PlayerLastPos = new();
    float noSpawnZoneRadiusStatic;
    public float NoSpawnZoneRadius { get { return noSpawnZoneRadiusStatic; } }
    float spawnZoneRadiusStatic;
    public float SpawnZoneRadius { get { return spawnZoneRadiusStatic; } }

    float totalSpawnWeight = 0;
    Vector3 nextSpawnPoint;
    float currentSpawnCD;
    PoolRefs poolRef;

    public static EnemySpawner Instance;

    private void Awake()
    {      
        if(Instance == null)
        {
            Instance = this;
        }

        poolRef = GetComponent<PoolRefs>();
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

        foreach (var spawn in EnemiesToLoopSpawn)
        {
            StartCoroutine(SpawnLoop(spawn));
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RandomSpawnRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        PlayerLastPos = player != null ? player.position : PlayerLastPos;
    }

    public Vector3 GetSpawnPoint()
    {
        Vector2 nextSpawnDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector3 playerPos = player != null ? player.position : PlayerLastPos;
        return (nextSpawnDirection * UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius)) + (Vector2)playerPos;
    }

    IEnumerator RandomSpawnRoutine()
    {
        yield return new WaitForSeconds(timeToStartSpawning);

        while (true)
        {
            GameObject nextEnemytoSpawn = GetNextSpawn();

            GameObject enemy = poolRef.Poolers[nextEnemytoSpawn].GetPooledGameObject();
            enemy.transform.position = GetSpawnPoint();
            enemy.SetActive(true);

            currentSpawnCD =Mathf.Abs(UnityEngine.Random.Range(baseSpawnCD - baseSpawnCD*(spawnCDVariationPerc/100), baseSpawnCD + baseSpawnCD*(spawnCDVariationPerc/100)));
            yield return new WaitForSeconds(currentSpawnCD);
        }
    }

    public IEnumerator SpawnByTime(GameObject enemyToSpawn, float time)
    {
        yield return new WaitForSeconds(time);

        //Instantiate(enemy, nextSpawnPoint + player.position, Quaternion.identity, this.transform);
        GameObject enemy = poolRef.Poolers[enemyToSpawn].GetPooledGameObject();
        enemy.transform.position = GetSpawnPoint();
        enemy.SetActive(true);
    }

    public IEnumerator SpawnLoop(EnemiesToLoopSpawn loopSpawn)
    {
        loopSpawn.waits = new WaitForSeconds[5];
        for(int i = 0; i < 5; i++)
        {
            float waitTime = UnityEngine.Random.Range(loopSpawn.timeSec - loopSpawn.timeVarSec, loopSpawn.timeSec + loopSpawn.timeVarSec);
            loopSpawn.waits[i] = new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(loopSpawn.timeToStart);

        while (true)
        {
            GameObject enemy = poolRef.Poolers[loopSpawn.enemy].GetPooledGameObject();
            enemy.transform.position = GetSpawnPoint();
            enemy.SetActive(true);

            int RandomIndex = UnityEngine.Random.Range(0, 5);
            yield return loopSpawn.waits[RandomIndex];
        }    
    }

    GameObject GetNextSpawn()
    {
        float spawnValue = UnityEngine.Random.Range(0 + float.Epsilon, totalSpawnWeight - float.Epsilon);

        GameObject nextSpawn = null;
        //foreach(EnemiesToSpawn enemy in EnemiesToSpawn)
        for (int i = 0; i < EnemiesToSpawn.Length; i++)
        {
            if (spawnValue <= EnemiesToSpawn[i].spawnWeight)
            {
                nextSpawn = EnemiesToSpawn[i].enemy;
                break;                
            }
            else
                spawnValue -= EnemiesToSpawn[i].spawnWeight;
        }
        return nextSpawn;
    }

    public void SpawnAsteroid(GameObject asteroidObject, Vector3 position, Vector3 moveDirection, float speed, int damageToApply)
    {
        GameObject newAsteroid = poolRef.Poolers[asteroidObject].GetPooledGameObject();
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
        if (poolRef.Poolers.ContainsKey(drone))
        {
            GameObject spawn = poolRef.Poolers[drone].GetPooledGameObject();
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