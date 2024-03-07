using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct DropsToSpawn
{
    public ResourceType drop;
    public float spawnWeight;
}
[Serializable]
public struct DropsGuaranteed
{
    public ResourceType drop;
    public int Amount;
}

public class EnemyDropDealer : MonoBehaviour
{
    [SerializeField] float radiusToSpawn = 1;
    [Space]
    [SerializeField] DropsToSpawn[] dropsToSpawn;
    [SerializeField] int minDropsNum = 2;
    [SerializeField] int maxDropsNum = 5;
    [Space]
    [SerializeField] DropsGuaranteed[] dropsGuaranteed;
    
    float totalSpawnWeight;

    public void SpawnDrops()
    {
        SpawnGuaranteedDrops();

        float dropsNumber = UnityEngine.Random.Range(minDropsNum, maxDropsNum);

        //foreach (var drop in dropsToSpawn)
        //{
        //    totalSpawnWeight += drop.spawnWeight;
        //}

        for (int i = 0; i < dropsToSpawn.Length; i++)
        {
            totalSpawnWeight += dropsToSpawn[i].spawnWeight;
        }

        for (int i = 0; i < dropsNumber; i++)
        {
            Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
            spawnPoint += transform.position;

            //Get Drop to Spawn
            float randomSpawnValue = UnityEngine.Random.Range(0+float.Epsilon, totalSpawnWeight-float.Epsilon);
            MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[GetNextDrop(randomSpawnValue)];;            

            if (dropPooler != null)
            {
                GameObject drop = dropPooler.GetPooledGameObject();
                drop.transform.position = spawnPoint;
                drop.SetActive(true);
            }
        }
    }

    void SpawnGuaranteedDrops()
    {
        //foreach (var drops in dropsGuaranteed) 
        for(int j = 0; j < dropsGuaranteed.Length; j++)
        {
            //for(int i = 0; i < drops.Amount; i++)
            for(int i = 0; i < dropsGuaranteed[j].Amount; i++)
            {
                MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[dropsGuaranteed[j].drop];

                if (dropPooler != null)
                {
                    Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
                    spawnPoint += transform.position;

                    GameObject drop = dropPooler.GetPooledGameObject();
                    drop.transform.position = spawnPoint;
                    drop.SetActive(true);
                }
            }
        }
    }

    ResourceType GetNextDrop(float spawnValue)
    {
        ResourceType nextDrop = ResourceType.Metal;

        for(int i=0; i < dropsToSpawn.Length; i++)
        //foreach (DropsToSpawn drop in dropsToSpawn)
        {
            if (spawnValue <= dropsToSpawn[i].spawnWeight)
            {
                nextDrop = dropsToSpawn[i].drop;
                break;
            }
            else
                spawnValue -= dropsToSpawn[i].spawnWeight;
        }
        return nextDrop;
    }
}