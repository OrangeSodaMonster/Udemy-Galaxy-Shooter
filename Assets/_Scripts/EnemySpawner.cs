using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct EnemiesToSpawn
{
    [HorizontalGroup("0", 0.12f), PreviewField(38, Alignment = ObjectFieldAlignment.Left), HideLabel]
    public GameObject enemy;
    [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), ReadOnly]
    public string Name;
    [VerticalGroup("0/1"), LabelWidth(40)]
    public float spawnWeight;
}
[Serializable]
public struct EnemiesToSpawnByTime
{
    [HorizontalGroup("0", 0.12f), PreviewField(38, Alignment = ObjectFieldAlignment.Left), HideLabel]
    public GameObject enemy;
    [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), ReadOnly]
    public string Name;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("Time")]
    public float timeSec;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("Var"), Tooltip("timeVarSec")]
    public float timeVarSec;
}
[Serializable]
public struct EnemiesToLoopSpawn
{
    [HorizontalGroup("0", 0.12f), PreviewField(38, Alignment = ObjectFieldAlignment.Left), HideLabel]
    public GameObject enemy;
    [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), ReadOnly]
    public string Name;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("Time")]
    public float timeSec;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("Var")]
    public float timeVarSec;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("Start"), Tooltip("timeVarSec")]
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
    float currentSpawnCD;
    PoolRefs poolRef;
    float spawnTimer = 0;
    float spawnTimerBegin = 0;
    PlayerMove playerMove;

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
        SetRoutines();

        playerMove = player.GetComponent<PlayerMove>();
    }

    Coroutine TimeSpawnRoutine;
    Coroutine LoopSpawnRoutine;
    public void SetRoutines()
    {
        totalSpawnWeight = 0;
        if(TimeSpawnRoutine != null)
            StopCoroutine(TimeSpawnRoutine);
        if(LoopSpawnRoutine != null)
            StopCoroutine(LoopSpawnRoutine);

        //foreach (var enemy in enemiesToSpawn)
        for(int i = 0; i < enemiesToSpawn.Length; i++)
        {
            totalSpawnWeight += enemiesToSpawn[i].spawnWeight;
        }

        //foreach (var spawn in enemiesToSpawnByTime)
        for(int i = 0; i < enemiesToSpawnByTime.Length; i++)
        {
            TimeSpawnRoutine = StartCoroutine(SpawnByTime(enemiesToSpawnByTime[i]));
        }

        //foreach (var spawn in EnemiesToLoopSpawn)
        for(int i = 0; i < EnemiesToLoopSpawn.Length; i++)
        {
            LoopSpawnRoutine = StartCoroutine(SpawnLoop(EnemiesToLoopSpawn[i]));
        }
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


    float minSpawnAngleFoward = 100;
    float chanceOfSpawn360 = 20;
    public Vector3 GetSpawnPointAheadOfPlayer()
    {
        if(UnityEngine.Random.Range(0f,100f) < chanceOfSpawn360)
        {
            return GetSpawnPoint360();
        }

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

    public void SetEnemiesToSpawn(float spawnCD, float spawnCDVar, EnemiesToSpawn[] enemiesToSpawn)
    {
        this.baseSpawnCD = spawnCD;
        this.spawnCDVariationPerc = spawnCDVar;
        this.enemiesToSpawn = enemiesToSpawn;
    }

    public void SetEnemiesToSpawnByTime(EnemiesToSpawnByTime[] enemiesToSpawnByTime)
    {
        this.enemiesToSpawnByTime = enemiesToSpawnByTime;
    }
    public void SetEnemiesToLoopSpawn(EnemiesToLoopSpawn[] enemiesToLoopSpawn)
    {
        this.EnemiesToLoopSpawn = enemiesToLoopSpawn;
    }

    private void OnValidate()
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (enemiesToSpawn[i].enemy != null)
            {
                enemiesToSpawn[i].Name = enemiesToSpawn[i].enemy.name;
            }
        }
        for (int i = 0; i < enemiesToSpawnByTime.Length; i++)
        {
            if (enemiesToSpawnByTime[i].enemy != null)
            {
                enemiesToSpawnByTime[i].Name = enemiesToSpawnByTime[i].enemy.name;
            }
        }
        for (int i = 0; i < EnemiesToLoopSpawn.Length; i++)
        {
            if (EnemiesToLoopSpawn[i].enemy != null)
            {
                EnemiesToLoopSpawn[i].Name = EnemiesToLoopSpawn[i].enemy.name;
            }
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