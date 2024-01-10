using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DronePowerUpgrade
{
    public ResourceNumber[] Cost;
    public int DamagePerSecond;
    public Gradient Color;
}

[Serializable]
public struct DroneRangeUpgrade
{
    public ResourceNumber[] Cost;
    public float Range;
}

[Serializable]
public struct DroneHealUpgrade
{
    public ResourceNumber[] Cost;
    public float ReduceFromHealInterval;
}

[CreateAssetMenu(fileName = "DroneUpgradesInfo", menuName = "MySOs/DroneUpgradesInfo")]
public class DroneUpgradeInfo : ScriptableObject
{
    public ResourceNumber UnlockCost; 
    public DronePowerUpgrade[] PowerUpgrades;
    public DroneRangeUpgrade[] RangeUpgrades;
    public DroneHealUpgrade[] HealUpgrade;
}