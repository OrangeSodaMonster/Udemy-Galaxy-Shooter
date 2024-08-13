using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LoopSpawns
{
    public GameObject enemy;
    [HorizontalGroup("G")]
    public float timeSec;
    [HorizontalGroup("G")]
    public float timeVarSec;

    [HorizontalGroup("H")]
    public float wait;
    [HorizontalGroup("H")]
    public float waitTimer;
}

public class Stage19Specific : MonoBehaviour
{
    [SerializeField] float percToReduceSpawnCD = 15;
    public LoopSpawns[] loopSpawns;

    EnemySpawner enemySpawner;

    void Start()
    {
        enemySpawner = EnemySpawner.Instance;

        for (int i = 0; i< loopSpawns.Length; i++)
        {
            loopSpawns[i].wait = UnityEngine.Random.Range(loopSpawns[i].timeSec - loopSpawns[i].timeVarSec, loopSpawns[i].timeSec + loopSpawns[i].timeVarSec);
            loopSpawns[i].wait *= .5f;
            loopSpawns[i].wait = Mathf.Clamp(loopSpawns[i].wait, 1, 9999999);
        }
    }

    private void Update()
    {
        for (int i = 0; i< loopSpawns.Length; i++)
        {
            if (loopSpawns[i].waitTimer >= loopSpawns[i].wait)
            {
                enemySpawner.SpawnEnemy(loopSpawns[i].enemy);

                loopSpawns[i].wait = UnityEngine.Random.Range(loopSpawns[i].timeSec - loopSpawns[i].timeVarSec, loopSpawns[i].timeSec + loopSpawns[i].timeVarSec);
                loopSpawns[i].wait = Mathf.Clamp(loopSpawns[i].wait, 1, 9999999);

                loopSpawns[i].waitTimer = 0;
            }

            loopSpawns[i].waitTimer += Time.deltaTime;
        }
    }



    [Button]
    public void IncreaseEnemySpawnRate()
    {
        for (int i = 0; i< loopSpawns.Length; i++)
        {
            loopSpawns[i].timeSec -= loopSpawns[i].timeSec * percToReduceSpawnCD * 0.01f;

            loopSpawns[i].wait = UnityEngine.Random.Range(loopSpawns[i].timeSec - loopSpawns[i].timeVarSec, loopSpawns[i].timeSec + loopSpawns[i].timeVarSec);
            loopSpawns[i].wait *= .5f;
            loopSpawns[i].wait = Mathf.Clamp(loopSpawns[i].wait, 1, 9999999);
        }
    }

   
}
