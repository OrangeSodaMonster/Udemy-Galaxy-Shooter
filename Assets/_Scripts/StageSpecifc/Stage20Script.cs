using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stage20Script : MonoBehaviour
{
    public LoopSpawns[] loopSpawnsGreen;
    public LoopSpawns[] loopSpawnsYellow;
    public LoopSpawns[] loopSpawnsOrange;
    [Space]
    [SerializeField] List<GameObject> shieldToDisable;
    [SerializeField] List<SpriteRenderer> shieldNodes;
    [SerializeField] Material conduitOffMat;
    [SerializeField] Color conduitOffColor;
    [SerializeField] UnityEvent onRedShieldDown = new();
    [Space]
    [SerializeField] List<GameObject> greenObjsToDisable;
    [SerializeField] GameObject conduitGreen;
    bool greenDisabled = false;
    [Space]
    [SerializeField] List<GameObject> yellowObjsToDisable;
    [SerializeField] GameObject conduitYellow;
    bool yellowDisabled = false;
    [Space]
    [SerializeField] List<GameObject> orangeObjsToDisable;
    [SerializeField] GameObject conduitOrange;
    bool orangeDisabled = false;

    //[HideInInspector] 
    public bool CanSpawnEnemies = true;

    EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = EnemySpawner.Instance;
        CanSpawnEnemies = true;

        for (int i = 0; i< loopSpawnsGreen.Length; i++)
        {
            loopSpawnsGreen[i].wait = UnityEngine.Random.Range(loopSpawnsGreen[i].timeSec - loopSpawnsGreen[i].timeVarSec, loopSpawnsGreen[i].timeSec + loopSpawnsGreen[i].timeVarSec);
            loopSpawnsGreen[i].wait *= .5f;
            loopSpawnsGreen[i].wait = Mathf.Clamp(loopSpawnsGreen[i].wait, 1, 9999999);
        }
        for (int i = 0; i< loopSpawnsYellow.Length; i++)
        {
            loopSpawnsYellow[i].wait = UnityEngine.Random.Range(loopSpawnsYellow[i].timeSec - loopSpawnsYellow[i].timeVarSec, loopSpawnsYellow[i].timeSec + loopSpawnsYellow[i].timeVarSec);
            loopSpawnsYellow[i].wait *= .5f;
            loopSpawnsYellow[i].wait = Mathf.Clamp(loopSpawnsYellow[i].wait, 1, 9999999);
        }
        for (int i = 0; i< loopSpawnsOrange.Length; i++)
        {
            loopSpawnsOrange[i].wait = UnityEngine.Random.Range(loopSpawnsOrange[i].timeSec - loopSpawnsOrange[i].timeVarSec, loopSpawnsOrange[i].timeSec + loopSpawnsOrange[i].timeVarSec);
            loopSpawnsOrange[i].wait *= .5f;
            loopSpawnsOrange[i].wait = Mathf.Clamp(loopSpawnsOrange[i].wait, 1, 9999999);
        }
    }

    private void Update()
    {
        if(!CanSpawnEnemies) return;

        if (!greenDisabled)
            LoopSpawnDealer(loopSpawnsGreen);
        if (!yellowDisabled)
            LoopSpawnDealer(loopSpawnsYellow);
        if (!orangeDisabled)
            LoopSpawnDealer(loopSpawnsOrange);
    }

    void LoopSpawnDealer(LoopSpawns[] loopSpawns)
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

    [Button, ButtonGroup("G")]
    public void DisableGreen()
    {
        greenDisabled = true;
        for (int i = 0; i < greenObjsToDisable.Count; i++)
        {
            greenObjsToDisable[i].SetActive(false);
        }
        DisableShield();

        ChangeConduity(conduitGreen);
    }

    [Button, ButtonGroup("G")]
    public void DisableYellow()
    {
        yellowDisabled = true;
        for (int i = 0; i < yellowObjsToDisable.Count; i++)
        {
            yellowObjsToDisable[i].SetActive(false);
        }
        DisableShield();

        ChangeConduity(conduitYellow);
    }

    [Button, ButtonGroup("G")]
    public void DisableOrange()
    {
        orangeDisabled = true;
        for (int i = 0; i < orangeObjsToDisable.Count; i++)
        {
            orangeObjsToDisable[i].SetActive(false);
        }
        DisableShield();

        ChangeConduity(conduitOrange);
    }

    [Button]
    public void DisableShield()
    {
        if (!(greenDisabled && yellowDisabled && orangeDisabled)) return;

        onRedShieldDown.Invoke();

        for (int i = 0; i < shieldToDisable.Count; i++)
        {
            shieldToDisable[i].SetActive(false);
        }

        for (int i = 0; i < shieldNodes.Count; i++)
        {
            shieldNodes[i].color = conduitOffColor;
            shieldNodes[i].material = conduitOffMat;
        }
    }

    void ChangeConduity(GameObject conduit)
    {
        conduit.GetComponent<SpriteRenderer>().material = conduitOffMat;
        conduit.GetComponent<SpriteRenderer>().color = conduitOffColor;
    }




}
