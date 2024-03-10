using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DronePowerUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [HorizontalGroup("G",width: .6f), GUIColor("#ff7878"), LabelWidth(130)]
    public int DamagePerSecond;
    [HorizontalGroup("G"), LabelWidth(34)]
    public Gradient Color;
}

[Serializable]
public struct DroneRangeUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [GUIColor("#fbff78")]
    public float Range;
}

[Serializable]
public struct DroneHealUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [GUIColor("#b3ff78"), LabelWidth(160)]
    public float ReduceFromHealInterval;
}

[CreateAssetMenu(fileName = "DroneUpgradesInfo", menuName = "MySOs/DroneUpgradesInfo")]
public class DroneUpgradeInfo : ScriptableObject
{
    [BoxGroup("UnlockCost"), HideLabel]
    [SerializeField] UpgradeCost UnlockCostLine;
    [GUIColor("#c9f8ff")]
    [HideInInspector] public ResourceNumber[] UnlockCost;
    [GUIColor("#ffc9c9")]
    public DronePowerUpgrade[] PowerUpgrades;
    [GUIColor("#fdffc9")]
    public DroneRangeUpgrade[] RangeUpgrades;
    [GUIColor("#dcffc9")]
    public DroneHealUpgrade[] HealUpgrades;

    private void OnValidate()
    {
        ConvertUnlock();
        ConvertPower();
        ConvertRange();
        ConvertHeal();
    }

    void ConvertPower()
    {
        for(int i = 0; i < PowerUpgrades.Length; i++)
        {
            PowerUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(PowerUpgrades[i].CostLine);
        }
    }

    void ConvertRange()
    {
        for (int i = 0; i < RangeUpgrades.Length; i++)
        {
            RangeUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(RangeUpgrades[i].CostLine);
        }
    }
    void ConvertHeal()
    {
        for (int i = 0; i < HealUpgrades.Length; i++)
        {
            HealUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(HealUpgrades[i].CostLine);
        }
    }

    void ConvertUnlock()
    {
        UnlockCost = PlayerCollectiblesCount.ConvertUpgradeCost(UnlockCostLine);
    }
    
}