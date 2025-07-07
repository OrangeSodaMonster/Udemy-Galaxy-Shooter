using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public class EventEnemySpawnScript : MonoBehaviour
{
    const float labelWidht = 135;

    [SerializeField] List<GameObject> spawns;
    [HorizontalGroup("StartTimer"), LabelWidth(labelWidht)]
    [SerializeField] float timeToStartSpawn = 0;
    [HorizontalGroup("StartTimer"), Tooltip("If > 0, spawn time random between Time and Time+add"), LabelWidth(labelWidht*0.5f), Range(0,10)]
    [SerializeField] float addRandomTimeToSpawn = 0;
    //[LabelWidth(labelWidht), Tooltip("Time added to start time after spawn is called, for chained sub spawns"), Range(0, 10)]
    //[HorizontalGroup("Calls"), SerializeField] float addedTimeAfterSpawn = 0;
    [LabelWidth(labelWidht), Tooltip("Use direction from event call")]
    [HorizontalGroup("Calls"), SerializeField] bool useDirectionFromCall = false;
    [LabelWidth(labelWidht)]
    [SerializeField] bool spawnAheadOfPlayer = false;
    [LabelWidth(labelWidht)]
    [SerializeField] bool spawnAtOnce = true;    
    [HorizontalGroup("Timed"), HideIf("spawnAtOnce"), LabelWidth(labelWidht)]
    [SerializeField] float spawnDuration;
    [Tooltip("Or follow list order")]
    [HorizontalGroup("Timed"), HideIf("spawnAtOnce"), LabelWidth(labelWidht)]
    [SerializeField] bool spawnInRandomOrder;
    [Tooltip("Spawn From the same direction if timed")]
    [HorizontalGroup("Close"), LabelWidth(labelWidht)]
    [SerializeField] bool spawnClose;
    [HorizontalGroup("Close"), ShowIf("@spawnClose && spawnAtOnce"), LabelWidth(labelWidht*0.5f)]
    [SerializeField, Range (0,3)] float spawnRadius = 2.5f;
    [SerializeField, ReadOnly] List<GameObject> allDeadCheckList = new();

    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)] int timesToCallStart = 1;
    [FoldoutGroup("Events")][Tooltip("Called when spawn is called")]
    public UnityEvent<Vector2> OnStartSpawn;
    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)] int timesToCallSpawn = 1;
    [FoldoutGroup("Events")][Tooltip("Called each individual spawn")]
    public UnityEvent<Vector2> OnSpawn;
    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called"), HideIf("spawnAtOnce")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)] int timesToCallStop = 1;
    [FoldoutGroup("Events"), HideIf("spawnAtOnce")][Tooltip("Called when timed spawns end")]
    public UnityEvent<Vector2> OnStopSpawnTimer;
    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)] int timesToCallAllDead = 1;
    [FoldoutGroup("Events")]
    [Tooltip("Called when all spawned enemies are disabled")]
    public UnityEvent<Vector2> OnAllDead;

    EnemySpawner spawner;
    Vector2 spawnDir = Vector2.zero;
    bool canCheckAllDead;

    private void Start()
    {
        spawner = EnemySpawner.Instance;
    }

    private void Update()
    {
        if (canCheckAllDead && allDeadCheckList.Count > 0)
        {
            bool allDead = true;
            for (int i = 0; i < allDeadCheckList.Count; i++)
            {
                if (allDeadCheckList[i].activeInHierarchy)
                    allDead = false;
            }

            if (allDead)
            {
                for (int i = 0; i < timesToCallAllDead; i++)
                {
                    OnAllDead.Invoke(spawnDir);
                    canCheckAllDead = false;
                    allDeadCheckList.Clear();
                }
            }
        }
    }

    IEnumerator EnableCheckAll()
    {
        yield return null;

        canCheckAllDead = true;
    }

    [Button("Spawn", ButtonSizes.Large), PropertyOrder(-1), GUIColor("lightblue")]
    public void CallSpawn(Vector2 dir)
    {
        if(dir != null) spawnDir = dir;        
        StartCoroutine(Spawn());
    }
    public void CallSpawn()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float startWaitTime = UnityEngine.Random.Range(timeToStartSpawn, timeToStartSpawn+addRandomTimeToSpawn);
        //timeToStartSpawn += addedTimeAfterSpawn;
        yield return new WaitForSeconds(startWaitTime);        

        if (spawnAtOnce || spawnDuration == 0)
            SpawnAtOnce();
        else if (spawnInRandomOrder)
            StartCoroutine(SpawnTimedRandom());
        else
            StartCoroutine(SpawnTimedInOrder());
    }

    void SpawnObject(GameObject obj)
    {
        GameObject spawnedObj = spawner.SpawnEnemy(obj);
        allDeadCheckList.Add(spawnedObj);
    }
    void SpawnObject(GameObject obj, Vector3 pos)
    {
        GameObject spawnedObj = spawner.SpawnEnemy(obj, pos);
        allDeadCheckList.Add(spawnedObj);
    }
    void SpawnObject(GameObject obj, Vector2 dir)
    {
        GameObject spawnedObj = spawner.SpawnEnemy(obj, dir);
        allDeadCheckList.Add(spawnedObj);
    }
    void SpawnObject(GameObject obj, bool spawnAhead)
    {
        GameObject spawnedObj = spawner.SpawnEnemy(obj, spawnAhead);
        allDeadCheckList.Add(spawnedObj);
    }

    void SpawnAtOnce()
    {
        if (!spawnClose)
        {
            for (int i = 0; i < timesToCallStart; i++)
                OnStartSpawn.Invoke(Vector2.zero);

            for (int i = 0; i < spawns.Count; i++)
            {
                SpawnObject(spawns[i], spawnAheadOfPlayer);
                for (int j = 0; j < timesToCallSpawn; j++)
                    OnSpawn.Invoke(Vector2.zero);
            }
        }            
        else // Spawn Close
        {
            Vector3 pos = Vector3.zero;

            if(useDirectionFromCall && spawnDir != Vector2.zero)
                pos = spawner.GetSpawnPoint(spawnDir);
            else if (spawnAheadOfPlayer)
                pos = spawner.GetSpawnPointAheadOfPlayer();
            else if (!spawnAheadOfPlayer)
                pos = spawner.GetSpawnPoint360();

            Vector2 direction = (pos - spawner.Player.position).normalized;
            for (int i = 0; i < timesToCallStart; i++)
                OnStartSpawn.Invoke(direction);

            Vector3 spawnPos = pos;
            for (int i = 0; i < spawns.Count; i++)
            {
                Vector2 randomInCircle = Random.insideUnitCircle;
                spawnPos = pos + new Vector3(randomInCircle.x, randomInCircle.y, 0) * spawnRadius;
                SpawnObject(spawns[i], spawnPos);
                for (int j = 0; j < timesToCallSpawn; j++)
                    OnSpawn.Invoke(direction);
            }
            spawnDir = direction;
        }
        StartCoroutine(EnableCheckAll());
    }

    IEnumerator SpawnTimedInOrder()
    {
        float timeBetween = spawnDuration / spawns.Count;
        WaitForSeconds wait = new WaitForSeconds(timeBetween);

        Vector2 direction = Vector3.zero;
        for (int i = 0; i < timesToCallStart; i++)
            OnStartSpawn.Invoke(direction);

        if (!spawnClose)
        {
            for (int i = 0; i < spawns.Count; i++)
            {
                SpawnObject(spawns[i], spawnAheadOfPlayer);
                for (int j = 0; j < timesToCallSpawn; j++)
                    OnSpawn.Invoke(direction);
                yield return wait;
            }
        }
        else // Spawn Close
        {
            if (useDirectionFromCall && spawnDir != Vector2.zero)
                direction = spawnDir.normalized;
            else if (spawnAheadOfPlayer)
                direction = (spawner.GetSpawnPointAheadOfPlayer() - spawner.Player.position).normalized;
            else if (!spawnAheadOfPlayer)
                direction = (spawner.GetSpawnPoint360() - spawner.Player.position).normalized;

            for (int i = 0; i < spawns.Count; i++)
            {
                SpawnObject(spawns[i], direction);
                for (int j = 0; j < timesToCallSpawn; j++)
                    OnSpawn.Invoke(direction);
                yield return wait;
            }
            spawnDir = direction;
        }
        for (int j = 0; j < timesToCallStop; j++)
        {
            OnStopSpawnTimer.Invoke(direction);
        }
        StartCoroutine(EnableCheckAll());
    }
    IEnumerator SpawnTimedRandom()
    {
        List<GameObject> shuffledSpawns = Embaralhar(spawns);
        float timeBetween = spawnDuration / spawns.Count;
        WaitForSeconds wait = new WaitForSeconds(timeBetween);

        Vector2 direction = Vector3.zero;
        for (int i = 0; i < timesToCallStart; i++)
            OnStartSpawn.Invoke(direction);

        if (!spawnClose)
        {
            for (int i = 0; i < spawns.Count; i++)
            {
                SpawnObject(shuffledSpawns[i], spawnAheadOfPlayer);
                for (int j = 0; j < timesToCallSpawn; j++)
                    OnSpawn.Invoke(direction);
                yield return wait;
            }
        }
        else // Spawn Close
        {
            if (useDirectionFromCall && spawnDir != Vector2.zero)
                direction = spawnDir.normalized;
            else if (spawnAheadOfPlayer)
                direction = (spawner.GetSpawnPointAheadOfPlayer() - spawner.Player.position).normalized;
            else if (!spawnAheadOfPlayer)
                direction = (spawner.GetSpawnPoint360() - spawner.Player.position).normalized;

            for (int i = 0; i < spawns.Count; i++)
            {
                SpawnObject(shuffledSpawns[i], direction);
                for (int j = 0; j < timesToCallSpawn; j++)
                    OnSpawn.Invoke(direction);
                yield return wait;
            }
            spawnDir = direction;
        }
        for (int j = 0; j < timesToCallStop; j++)
        {
            OnStopSpawnTimer.Invoke(direction);
        }

        StartCoroutine(EnableCheckAll());

        List<GameObject> Embaralhar(List<GameObject> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1); // Inclui o último índice
                (list[k], list[n]) = (list[n], list[k]);
            }
            return list;
        }
    }

}
