using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PowerUpDrops
{
    [HorizontalGroup("G", width: .6f), LabelWidth(60)]
    public GameObject PowerUp;
    [HorizontalGroup("G"), GUIColor("#b894ff"), LabelWidth(50)]
    public float Weight;
}

public class PowerUpDrop : MonoBehaviour
{
    [Range(0, 100)] public float ChanceToDrop = 2f;
    public List<PowerUpDrops> PuDrops;
    public bool IsFromSpawner = false;

    EnemyHP enemyHP;
    bool canDropShield;
    bool canDropTractor;
    float defaultChanceToDrop;

    private void Awake()
    {
        enemyHP = GetComponent<EnemyHP>();
        defaultChanceToDrop = ChanceToDrop;
    }

    private void OnEnable()
    {
        enemyHP.Died += TryDropPowerUp;

        if (GameManager.IsSurvival)
        {
            float bonusMult = 1 + BonusPowersDealer.Instance.PowerUpDrop/100f;
            ChanceToDrop *= bonusMult;
            if (ChanceToDrop > 100) ChanceToDrop = 100;
        }        
    }

    public void SetFromSpawnerChance()
    {
        IsFromSpawner = true;
            
        ChanceToDrop *= 0.25f;
    }

    private void OnDisable()
    {
        enemyHP.Died -= TryDropPowerUp;

        IsFromSpawner = false;
        ChanceToDrop = defaultChanceToDrop;
    } 

    public void TryDropPowerUp()
    {
        var currentUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades;
        canDropTractor = currentUpgrades.ShipUpgrades.TractorBeamEnabled;
        canDropShield = currentUpgrades.FrontShieldUpgrades.Enabled || currentUpgrades.BackShieldUpgrades.Enabled ||
                        currentUpgrades.LeftShieldUpgrades.Enabled || currentUpgrades.RightShieldUpgrades.Enabled;

        if (!ShouldDrop()) return;

        int dropIndex = GetDropIndex();
        if(dropIndex >= 0)
            Instantiate(PuDrops[dropIndex].PowerUp, transform.position, Quaternion.identity);
    }

    bool ShouldDrop()
    {
        if(UnityEngine.Random.Range(0f,100f) < ChanceToDrop)
            return true;

        return false;
    }

    int GetDropIndex()
    {
        float totalSpawnWeight = 0;

        foreach (PowerUpDrops PU in PuDrops)
        {
            totalSpawnWeight += PU.Weight;
        }

        float randomSpawnValue = UnityEngine.Random.Range(0, totalSpawnWeight);
        
        for(int i = 0; i < PuDrops.Count; i++)
        {
            if (randomSpawnValue <= PuDrops[i].Weight)
            {
                if (!canDropShield && PuDrops[i].PowerUp.GetComponent<ShieldPowerUp>() != null)
                    return -1;
                else if (!canDropTractor && PuDrops[i].PowerUp.GetComponent<TractorBeamPowerUp>() != null)
                    return -1;
                else
                    return i;
            }
            else
                randomSpawnValue -= PuDrops[i].Weight;
        }
        Debug.Log("Chegou no fim da Iteração, o que nunca deveria acontecer");
        return -1;
    }
}

