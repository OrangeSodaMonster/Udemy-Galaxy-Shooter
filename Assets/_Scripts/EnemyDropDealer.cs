using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;


[Serializable]
public struct DropsToSpawn
{
    [HorizontalGroup("G", width: .6f), LabelWidth(40)]
    public ResourceType drop;
    [HorizontalGroup("G"), GUIColor("cyan"), LabelWidth(90)]
    public float spawnWeight;
}
[Serializable]
public struct DropsGuaranteed
{
    [HorizontalGroup("G", width: .6f), LabelWidth(40)]
    public ResourceType drop;
    [HorizontalGroup("G"), GUIColor("#ff61e5"), LabelWidth(90)]
    public int Amount;
}

public class EnemyDropDealer : MonoBehaviour
{
    [SerializeField] float radiusToSpawn = 1;
    public bool IsFromSpawner = false;
    [Space]
    public DropsToSpawn[] DropsToSpawn;
    public int MinDropsNum = 2;
    public int MaxDropsNum = 5;
    [Space]
    public DropsGuaranteed[] dropsGuaranteed;
    
    float totalSpawnWeight;

    DropsToSpawn[] defaultDropsToSpawn;
    int defaultMinDropsNum = 2;
    int defaultMaxDropsNum = 5;
    DropsGuaranteed[] defaultDropsGuaranteed;

    private void Awake()
    {
        SetDefaults();

        void SetDefaults()
        {
            defaultDropsToSpawn = new DropsToSpawn[DropsToSpawn.Length];
            defaultDropsGuaranteed = new DropsGuaranteed[dropsGuaranteed.Length];

            Array.Copy(DropsToSpawn, defaultDropsToSpawn, DropsToSpawn.Length);
            Array.Copy(dropsGuaranteed, defaultDropsGuaranteed, dropsGuaranteed.Length);

            defaultMinDropsNum = MinDropsNum;
            defaultMaxDropsNum = MaxDropsNum;
        }
    }

    private void OnEnable()
    {
        if (GameManager.IsSurvival)
        {
            SurvivalExtraDropMetals();
        }        
    }

    public void SetFromSpawnerValues()
    {
        IsFromSpawner = true;

        for (int i = 0; i < dropsGuaranteed.Length; i++)
        {
            dropsGuaranteed[i].Amount = 0;
        }

        MinDropsNum = MinDropsNum == 0 ? 0 : 1;
        MaxDropsNum = MaxDropsNum < 2 ? MaxDropsNum : 2;

        for (int i = 0; i < DropsToSpawn.Length; i++)
        {
            if (DropsToSpawn[i].drop == ResourceType.RareMetal)
                DropsToSpawn[i].spawnWeight *= 0.8f;
            if (DropsToSpawn[i].drop == ResourceType.EnergyCristal)
                DropsToSpawn[i].spawnWeight *= 0.5f;
            if (DropsToSpawn[i].drop == ResourceType.CondensedEnergyCristal)
                DropsToSpawn[i].spawnWeight *= 0.05f;
        }
    }

    public void SpawnDrops()
    {
        SpawnGuaranteedDrops();

        if (GameManager.IsSurvival && !IsFromSpawner)
            SpawnBonusDrops();

        int dropsNumber = UnityEngine.Random.Range(MinDropsNum, MaxDropsNum+1);

        for (int i = 0; i < DropsToSpawn.Length; i++)
        {
            totalSpawnWeight += DropsToSpawn[i].spawnWeight;
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

    void SpawnBonusDrops()
    {
        if(BonusPowersDealer.Instance.EnergyCristalsDrop == 0) return;

        float energyCristalDropWeight = 0;
        float condEnergyCristalDropWeight = 0;
        float totalDropWeight = 0;
        float cristalDropWeight = 0;

        for (int i = 0; i < DropsToSpawn.Length; i++)
        {
            if (DropsToSpawn[i].drop == ResourceType.EnergyCristal)
            {
                energyCristalDropWeight = DropsToSpawn[i].spawnWeight;
                cristalDropWeight += DropsToSpawn[i].spawnWeight;
            }
            else if (DropsToSpawn[i].drop == ResourceType.CondensedEnergyCristal)
            {
                condEnergyCristalDropWeight = DropsToSpawn[i].spawnWeight;
                cristalDropWeight += DropsToSpawn[i].spawnWeight;
            }
            totalDropWeight += DropsToSpawn[i].spawnWeight;
        }

        float averageDrops = (MinDropsNum + MaxDropsNum)*0.5f;
        float cristalDropChance = (cristalDropWeight / totalDropWeight)*100f;
        cristalDropChance *= (BonusPowersDealer.Instance.EnergyCristalsDrop/100f); //Obter 10% da chance de dropar cristal, por exemplo
        cristalDropChance *= averageDrops;
        float condensedDropChance = (condEnergyCristalDropWeight / cristalDropWeight)*100f;

        float randomDropValue = UnityEngine.Random.Range(0f, 100f);
        Debug.Log($"NormalCristalChance: {cristalDropChance}...RandomN: {randomDropValue}");
        if(randomDropValue < cristalDropChance)
        {
            randomDropValue = UnityEngine.Random.Range(0f, 100f);
            Debug.Log($"NormalCondCristalChance: {condensedDropChance}...RandomN: {randomDropValue}");
            if (randomDropValue < condensedDropChance)
            {
                MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[ResourceType.CondensedEnergyCristal];
                Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
                spawnPoint += transform.position;

                GameObject drop = dropPooler.GetPooledGameObject();
                drop.transform.position = spawnPoint;
                drop.SetActive(true);
                Debug.Log($"<color=#d900ff>Bonus: Condensed Energy Cristal</color>");
            }
            else
            {
                MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[ResourceType.EnergyCristal];
                Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
                spawnPoint += transform.position;

                GameObject drop = dropPooler.GetPooledGameObject();
                drop.transform.position = spawnPoint;
                drop.SetActive(true);
                Debug.Log($"<color=#00bbff>Bonus: Energy Cristal</color>");
            }
        }
        SpawnBonusGaranteedDrops();
    }
    void SpawnBonusGaranteedDrops()
    {
        if (BonusPowersDealer.Instance.EnergyCristalsDrop == 0) return;

        float totalCristals = 0;
        float totalCondensedCristals = 0;

        for (int i = 0; i < dropsGuaranteed.Length; i++)
        {
            if (dropsGuaranteed[i].drop == ResourceType.EnergyCristal)
            {
                totalCristals += dropsGuaranteed[i].Amount;
            }
            else if (dropsGuaranteed[i].drop == ResourceType.CondensedEnergyCristal)
            {
                totalCristals += dropsGuaranteed[i].Amount;
                totalCondensedCristals = dropsGuaranteed[i].Amount;
            }
        }

        float cristalDropChance = totalCristals * (BonusPowersDealer.Instance.EnergyCristalsDrop);
        float condensedDropChance = (totalCondensedCristals / totalCristals)*100f;

        float randomDropValue = UnityEngine.Random.Range(0f, 100f);
        Debug.Log($"GaranteedCristalChance: {cristalDropChance}...RandomN: {randomDropValue}");
        if (randomDropValue < cristalDropChance)
        {
            randomDropValue = UnityEngine.Random.Range(0f, 100f);
            Debug.Log($"GaranteedCondCristalChance: {condensedDropChance}...RandomN: {randomDropValue}");
            if (randomDropValue < condensedDropChance)
            {
                MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[ResourceType.CondensedEnergyCristal];
                Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
                spawnPoint += transform.position;

                GameObject drop = dropPooler.GetPooledGameObject();
                drop.transform.position = spawnPoint;
                drop.SetActive(true);
                Debug.Log($"<color=#d900ff>Bonus: Condensed Energy Cristal</color>");
            }
            else
            {
                MMSimpleObjectPooler dropPooler = DropsPoolRef.Instance.ResourcePoolers[ResourceType.EnergyCristal];
                Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle * radiusToSpawn;
                spawnPoint += transform.position;

                GameObject drop = dropPooler.GetPooledGameObject();
                drop.transform.position = spawnPoint;
                drop.SetActive(true);
                Debug.Log($"<color=#00bbff>Bonus: Energy Cristal</color>");
            }
        }
    }

    ResourceType GetNextDrop(float spawnValue)
    {
        ResourceType nextDrop = ResourceType.Metal;

        for(int i=0; i < DropsToSpawn.Length; i++)
        {
            if (spawnValue <= DropsToSpawn[i].spawnWeight)
            {
                nextDrop = DropsToSpawn[i].drop;
                break;
            }
            else
                spawnValue -= DropsToSpawn[i].spawnWeight;
        }
        return nextDrop;
    }    

    bool wasExtraDropsMetalsDone = false;
    void SurvivalExtraDropMetals()
    {
        if(wasExtraDropsMetalsDone || !GameManager.IsSurvival) return;
        wasExtraDropsMetalsDone = true;

        float expectedDrops = (MinDropsNum + MaxDropsNum) / 2f;

        float totalWeight = 0;
        float metalWeight = 0;
        float rareMetalWeight = 0;
        for(int i = 0; i<DropsToSpawn.Length; i++)
        {
            totalWeight += DropsToSpawn[i].spawnWeight;

            if(DropsToSpawn[i].drop == ResourceType.Metal)
                metalWeight = DropsToSpawn[i].spawnWeight;
            else if (DropsToSpawn[i].drop == ResourceType.RareMetal)
                rareMetalWeight = DropsToSpawn[i].spawnWeight;
        }

        float metalProportion = metalWeight/totalWeight;
        float rareMetalProportion = rareMetalWeight/totalWeight;

        float metalGuaranteed = 0;
        float rareMetalGuaranteed = 0;

        for (int i = 0; i<dropsGuaranteed.Length; i++)
        {
            if (dropsGuaranteed[i].drop == ResourceType.Metal)
                metalGuaranteed = dropsGuaranteed[i].Amount;
            else if (dropsGuaranteed[i].drop == ResourceType.RareMetal)
                rareMetalGuaranteed = dropsGuaranteed[i].Amount;
        }

        float extraMetalChance = (expectedDrops * metalProportion + metalGuaranteed) * SurvivalManager.ExtraMetalDropPerc * 0.01f;
        float extraRareMetalChance = (expectedDrops * rareMetalProportion + rareMetalGuaranteed) * SurvivalManager.ExtraRareMetalDropPerc * 0.01f;

        int extraMetal = Mathf.FloorToInt(extraMetalChance) + CheckExtraChanceDecimal(extraMetalChance);
        int extraRareMetal = Mathf.FloorToInt(extraRareMetalChance) + CheckExtraChanceDecimal(extraRareMetalChance);

        Debug.Log($"Extra Drops...Metal: {extraMetal}, RareMetal: {extraRareMetal}");

        for (int i = 0; i<dropsGuaranteed.Length; i++)
        {
            if (dropsGuaranteed[i].drop == ResourceType.Metal)
                dropsGuaranteed[i].Amount += extraMetal;
            else if (dropsGuaranteed[i].drop == ResourceType.RareMetal)
                dropsGuaranteed[i].Amount += extraRareMetal;
        }

        int CheckExtraChanceDecimal(float chance)
        {
            float num = chance%1;

            if (UnityEngine.Random.Range(0, 1f) <= num) return 1;
            else return 0;
        }
    }

    void ResetDefaults()
    {
        IsFromSpawner = false;

        Array.Copy(defaultDropsToSpawn, DropsToSpawn, defaultDropsToSpawn.Length);
        Array.Copy(defaultDropsGuaranteed, dropsGuaranteed, defaultDropsGuaranteed.Length);

        MinDropsNum = defaultMinDropsNum;
        MaxDropsNum = defaultMaxDropsNum;
    }

    private void OnDisable()
    {        
        ResetDefaults();
    }
}