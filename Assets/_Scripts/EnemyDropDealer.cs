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

public class EnemyDropDealer : MonoBehaviour
{
    [SerializeField] DropsToSpawn[] dropsToSpawn;
    [SerializeField] int minDropsNum = 2;
    [SerializeField] int maxDropsNum = 5;

    [SerializeField] float radiusToSpawn = 1;
    
    float totalSpawnWeight;

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
            float randomSpawnValue = UnityEngine.Random.Range(0+float.Epsilon, totalSpawnWeight-float.Epsilon);
            MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[GetNextDrop(randomSpawnValue)];;            

            if (dropPooler != null)
            {
                GameObject drop = dropPooler.GetPooledGameObject();
                drop.transform.SetLocalPositionAndRotation(spawnPoint, Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward));
                drop.SetActive(true);
            }

        }
    }
    ResourceType GetNextDrop(float spawnValue)
    {
        ResourceType nextDrop = ResourceType.MetalCrumb;
        foreach (DropsToSpawn drop in dropsToSpawn)
        {
            if (spawnValue <= drop.spawnWeight)
            {
                nextDrop = drop.drop;
                break;
            }
            else
                spawnValue -= drop.spawnWeight;
        }
        return nextDrop;
    }
}