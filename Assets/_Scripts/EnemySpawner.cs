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
    [HorizontalGroup("G")]
    public float timeSec;
    [HorizontalGroup("G")]
    public float timeVarSec;
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

    [HideInInspector]
    public WaitForSeconds[] waits;
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
    //Vector3 nextSpawnPoint;
    float currentSpawnCD;
    PoolRefs poolRef;
    float spawnTimer = 0;
    float spawnTimerBegin = 0;
    //Rigidbody2D playerRB;
    PlayerMove playerMove;
    float minSpawnAngleFoward = 160;

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
            StartCoroutine(SpawnByTime(spawn));
        }

        foreach (var spawn in EnemiesToLoopSpawn)
        {
            StartCoroutine(SpawnLoop(spawn));
        }

        //playerRB = player.GetComponent<Rigidbody2D>();
        playerMove = player.GetComponent<PlayerMove>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        PlayerLastPos = player != null ? player.position : PlayerLastPos;

        if(spawnTimer >= currentSpawnCD && spawnTimerBegin >= timeToStartSpawning)
        {
            SpawnEnemy(GetNextSpawn());
            currentSpawnCD =Mathf.Abs(UnityEngine.Random.Range(baseSpawnCD - baseSpawnCD*(spawnCDVariationPerc/100), baseSpawnCD + baseSpawnCD*(spawnCDVariationPerc/100)));
            spawnTimer = 0;
        }

        spawnTimer += Time.deltaTime;
        spawnTimerBegin += Time.deltaTime;
    }

    public Vector3 GetSpawnPointAheadOfPlayer()
    {
        float playerSpeed = playerMove.PlayerVelocity.magnitude;
        float spawnAngle = 360;
        if (playerSpeed > 0.1f)
        {
            float speedProportion = playerSpeed / playerMove.MaxSpeed;
            if(speedProportion > 1) speedProportion = 1;

            spawnAngle = Mathf.Lerp(360, minSpawnAngleFoward, speedProportion);
        }
        else
            return GetSpawnPoint360();

        spawnAngle = UnityEngine.Random.Range(-spawnAngle*0.5f, spawnAngle*0.5f);
        spawnAngle = spawnAngle * Mathf.Deg2Rad;
        Vector2 nextSpawnDirection = playerMove.PlayerVelocity.normalized;
        nextSpawnDirection = new Vector2(
            nextSpawnDirection.x * Mathf.Cos(spawnAngle) - nextSpawnDirection.y * Mathf.Sin(spawnAngle),
            nextSpawnDirection.x * Mathf.Sin(spawnAngle) + nextSpawnDirection.y * Mathf.Cos(spawnAngle));
        nextSpawnDirection *= UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius);

        Vector3 playerPos = player != null ? player.position : PlayerLastPos;

        return nextSpawnDirection + (Vector2)playerPos;
    }

    public Vector3 GetSpawnPoint360()
    {
        Vector2 nextSpawnDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector3 playerPos = player != null ? player.position : PlayerLastPos;
        return (nextSpawnDirection * UnityEngine.Random.Range(noSpawnZoneRadius, spawnZoneRadius)) + (Vector2)playerPos;
    }

    public void SpawnEnemy(GameObject spawn)
    {
        GameObject enemy = poolRef.Poolers[spawn].GetPooledGameObject();
        if(enemy != null && enemy.GetComponent<EnemyHP>() && enemy.GetComponent<EnemyHP>().IsAsteroid)
            enemy.transform.position = GetSpawnPointAheadOfPlayer();
        else
            enemy.transform.position = GetSpawnPoint360();

        enemy.SetActive(true);
    }    
    public void SpawnEnemy(GameObject spawn, Vector3 spawnPoint)
    {
        GameObject enemy = poolRef.Poolers[spawn].GetPooledGameObject();
        enemy.transform.position = spawnPoint;
        enemy.SetActive(true);
    }

    public IEnumerator SpawnByTime(EnemiesToSpawnByTime spawn)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(spawn.timeSec - spawn.timeVarSec, spawn.timeSec + spawn.timeVarSec));

        //Instantiate(enemy, nextSpawnPoint + player.position, Quaternion.identity, this.transform);
        GameObject enemy = poolRef.Poolers[spawn.enemy].GetPooledGameObject();
        if (enemy != null && enemy.GetComponent<EnemyHP>() && enemy.GetComponent<EnemyHP>().IsAsteroid)
            enemy.transform.position = GetSpawnPointAheadOfPlayer();
        else
            enemy.transform.position = GetSpawnPoint360();
        enemy.SetActive(true);
    }

    public IEnumerator SpawnLoop(EnemiesToLoopSpawn loopSpawn)
    {
        loopSpawn.waits = new WaitForSeconds[5];
        for (int i = 0; i < 5; i++)
        {
            float waitTime = UnityEngine.Random.Range(loopSpawn.timeSec - loopSpawn.timeVarSec, loopSpawn.timeSec + loopSpawn.timeVarSec);
            waitTime = Mathf.Clamp(waitTime, 1, 9999999);
            loopSpawn.waits[i] = new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(loopSpawn.timeToStart);

        while (true)
        {
            GameObject enemy = poolRef.Poolers[loopSpawn.enemy].GetPooledGameObject();
            if (enemy != null && enemy.GetComponent<EnemyHP>() && enemy.GetComponent<EnemyHP>().IsAsteroid)
                enemy.transform.position = GetSpawnPointAheadOfPlayer();
            else
                enemy.transform.position = GetSpawnPoint360();
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