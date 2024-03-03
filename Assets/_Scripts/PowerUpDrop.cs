using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PowerUpDrops
{
    public GameObject PowerUp;
    public float Weight;
}

public class PowerUpDrop : MonoBehaviour
{
    [SerializeField,Range(0,100)] float chanceToDrop = 2f;
    [SerializeField] List<PowerUpDrops> PuDrops;

    EnemyHP enemyHP;
    bool canDropShield;
    bool canDropTractor;

    private void Awake()
    {
        enemyHP = GetComponent<EnemyHP>();
    }

    private void OnEnable()
    {
        enemyHP.Died += TryDropPowerUp;
    }

    private void OnDisable()
    {
        enemyHP.Died -= TryDropPowerUp;        
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
        if(UnityEngine.Random.Range(0f,100f) < chanceToDrop)
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

