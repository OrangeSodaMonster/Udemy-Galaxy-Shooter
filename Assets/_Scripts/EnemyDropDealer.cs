using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DropsToSpawn
{
    public GameObject drop;
    public float spawnWeight;
}

public class EnemyDropDealer : MonoBehaviour
{
    [SerializeField] DropsToSpawn[] dropsToSpawn;
    [SerializeField] int minDropsNum = 2;
    [SerializeField] int maxDropsNum = 5;

    [SerializeField] float radiusToSpawn = 1;

    Transform transformParent;
    float totalSpawnWeight;

    private void Awake()
    {
        transformParent = FindObjectOfType<MetalCrumbsParent>().transform;
    }
    
    public void SpawnDrops()
    {
        float dropsNumber = UnityEngine.Random.Range(minDropsNum, maxDropsNum);

        foreach (var drop in dropsToSpawn)
        {
            totalSpawnWeight += drop.spawnWeight;
        }

        for (int i = 0; i < dropsNumber; i++)
        {
            Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
            spawnPoint += transform.position;

            //Get Drop to Spawn
            float randomSpawnValue = UnityEngine.Random.Range(0, totalSpawnWeight);
            GameObject dropToSpawn = null;

            int j = 0;
            while (dropToSpawn == null & j < dropsToSpawn.Length)
            {
                if (randomSpawnValue <= dropsToSpawn[j].spawnWeight)
                { dropToSpawn = dropsToSpawn[j].drop; }
                else
                {
                    randomSpawnValue -= dropsToSpawn[j].spawnWeight;
                    j++;
                }
            }

            if (dropToSpawn != null)
                Instantiate(dropToSpawn, spawnPoint, Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward), transformParent);
        }
    }
}