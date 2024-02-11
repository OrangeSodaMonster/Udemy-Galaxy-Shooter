using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RareSpawnScript : MonoBehaviour
{
    [Serializable]
    struct RareSpawnsChances
    {
        public GameObject RareSpawn;
        [Tooltip("In %")]
        public float ChancePerMinute;
        public float ChancePerSecond;
    }

    [SerializeField] RareSpawnsChances[] RareSpawns;
    [SerializeField] float intervalToEnableAtBegining = 10f;
    [SerializeField] float intervalBetweenSpawns = 5f;
    [SerializeField] bool playSound = true;

    Dictionary<GameObject, GameObject> rareDict = new();

    void Start()
    {
        for(int i = 0; i < RareSpawns.Length; i++)
        {
            rareDict.Add(RareSpawns[i].RareSpawn, Instantiate(RareSpawns[i].RareSpawn, transform));
            rareDict[RareSpawns[i].RareSpawn].SetActive(false);

            RareSpawns[i].ChancePerSecond = RareSpawns[i].ChancePerMinute / 60;
        }

        StartCoroutine(rareSpawnerRoutine());
    }

    IEnumerator rareSpawnerRoutine()
    {
        yield return new WaitForSeconds(intervalToEnableAtBegining);

        while(true)
        {
            foreach (var rare in RareSpawns)
            {
                if (CheckSpawn(rare))
                {
                    SpawnRare(rareDict[rare.RareSpawn]);

                    yield return new WaitForSeconds(intervalBetweenSpawns - 1);
                }
            }

            yield return new WaitForSeconds(1);
        }
    }

    bool CheckSpawn(RareSpawnsChances rareSpawn)
    {
        float randonValue = UnityEngine.Random.Range(0f , 100f);

        if(randonValue < rareSpawn.ChancePerSecond)
        {
            Debug.Log($"Spawn >>> {randonValue}");
            return true;
        }
        else return false;
    }

    void SpawnRare(GameObject rare)
    {
        if (!rare.activeSelf)
        {
            rare.transform.position = EnemySpawner.Instance.GetSpawnPoint();
            rare.transform.rotation = Quaternion.identity;
            rare.SetActive(true);
        }
        else
        {
            Instantiate(rare, EnemySpawner.Instance.GetSpawnPoint(), Quaternion.identity, transform);
        }

        if(playSound)
            AudioManager.Instance.RareSpawnSound.PlayFeedbacks();
    }
}