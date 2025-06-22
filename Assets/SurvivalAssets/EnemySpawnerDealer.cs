using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct EnemySpawnData
{
    [HorizontalGroup("0")]
    public float SpawnCd;
    [HorizontalGroup("0")]
    public float SpawnCdVarPerc;
    public EnemiesToSpawn[] EnemiesToSpawn;
    [GUIColor("lightgreen")]
    public EnemiesToSpawnByTime[] EnemiesToSpawnByTime;
    [GUIColor("lightyellow")]
    public EnemiesToLoopSpawn[] EnemiesToLoopSpawn;
    [GUIColor("cyan")]
    public RareSpawnsChances[] RareSpawns;
}

public class EnemySpawnerDealer : MonoBehaviour
{
    [FormerlySerializedAs("enemySpawnDatas")]
    [SerializeField] AnimationCurve spawnCdCurve = new();
    [SerializeField, ReadOnly] float currentSpawnCD;
    [SerializeField] List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();

    EnemySpawner enemySpawner;
    RareSpawnScript rareSpawner;
    SurvivalTimers timer;
    float minutesToLastStage = 15;
    float spawnValue = 0;

    private void Start()
    {
        UpdadeFirstSpawns();

        SurvivalManager.OnSectionChange.AddListener(ChangeSpawns);
        timer = FindObjectOfType<SurvivalTimers>();
    }

    private void Update()
    {
        spawnValue = timer.TotalTime/(15*60);
        currentSpawnCD = spawnCdCurve.Evaluate(spawnValue);

        enemySpawner.SetSpawnCD(currentSpawnCD);
    }

    [Button(), PropertyOrder(-1)]
    void UpdadeFirstSpawns()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        rareSpawner = FindObjectOfType<RareSpawnScript>();
        ChangeSpawnDealer(0);
        enemySpawner.SetSpawnCD(currentSpawnCD);
    }

    void ChangeSpawns()
    {
        ChangeSpawnDealer(SurvivalManager.CurrentSection);        
    }    

    void ChangeSpawnDealer(int i)
    {
        enemySpawner.SetEnemiesToSpawn(enemySpawnData[i].SpawnCdVarPerc, enemySpawnData[i].EnemiesToSpawn);
        enemySpawner.SetEnemiesToSpawnByTime(enemySpawnData[i].EnemiesToSpawnByTime);
        enemySpawner.SetEnemiesToLoopSpawn(enemySpawnData[i].EnemiesToLoopSpawn);
        rareSpawner.SetRareSpawns(enemySpawnData[i].RareSpawns);

        if (Application.isPlaying)
            enemySpawner.SetRoutines();
    }

    public void SetListSize(int size)
    {
        enemySpawnData.SetLength(size);
    }

    private void OnValidate()
    {
        for (int i = 0; i < enemySpawnData.Count; i++)
        {
            for (int j = 0; j < enemySpawnData[i].EnemiesToSpawn.Length; j++)
            {
                if (enemySpawnData[i].EnemiesToSpawn[j].enemy != null)
                {
                    enemySpawnData[i].EnemiesToSpawn[j].Name = enemySpawnData[i].EnemiesToSpawn[j].enemy.name;
                }
            }
            for (int j = 0; j < enemySpawnData[i].EnemiesToLoopSpawn.Length; j++)
            {
                if (enemySpawnData[i].EnemiesToLoopSpawn[j].enemy != null)
                {
                    enemySpawnData[i].EnemiesToLoopSpawn[j].Name = enemySpawnData[i].EnemiesToLoopSpawn[j].enemy.name;
                }
            }
            for (int j = 0; j < enemySpawnData[i].EnemiesToSpawnByTime.Length; j++)
            {
                if (enemySpawnData[i].EnemiesToSpawnByTime[j].enemy != null)
                {
                    enemySpawnData[i].EnemiesToSpawnByTime[j].Name = enemySpawnData[i].EnemiesToSpawnByTime[j].enemy.name;
                }
            }
            for (int j = 0; j < enemySpawnData[i].RareSpawns.Length; j++)
            {
                if (enemySpawnData[i].RareSpawns[j].RareSpawn != null)
                {
                    enemySpawnData[i].RareSpawns[j].Name = enemySpawnData[i].RareSpawns[j].RareSpawn.name;
                    enemySpawnData[i].RareSpawns[j].ChancePerSecond = enemySpawnData[i].RareSpawns[j].ChancePerMinute / 60;
                }
            }
        }        
    }
}
