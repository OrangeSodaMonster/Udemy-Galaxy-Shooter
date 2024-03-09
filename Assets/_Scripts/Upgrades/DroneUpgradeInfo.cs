using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DronePowerUpgrade
{
    public ResourceNumber[] Cost;
    [HorizontalGroup("G",width: .6f), GUIColor("#ff7878"), LabelWidth(130)]
    public int DamagePerSecond;
    [HorizontalGroup("G"), LabelWidth(34)]
    public Gradient Color;
}

[Serializable]
public struct DroneRangeUpgrade
{
    public ResourceNumber[] Cost;
    [GUIColor("#fbff78")]
    public float Range;
}

[Serializable]
public struct DroneHealUpgrade
{
    public ResourceNumber[] Cost;
    [GUIColor("#b3ff78"), LabelWidth(160)]
    public float ReduceFromHealInterval;
}

[CreateAssetMenu(fileName = "DroneUpgradesInfo", menuName = "MySOs/DroneUpgradesInfo")]
public class DroneUpgradeInfo : ScriptableObject
{
    [GUIColor("#c9f8ff")]
    public ResourceNumber[] UnlockCost; 
    [GUIColor("#ffc9c9")]
    public DronePowerUpgrade[] PowerUpgrades;
    [GUIColor("#fdffc9")]
    public DroneRangeUpgrade[] RangeUpgrades;
    [GUIColor("#dcffc9")]
    public DroneHealUpgrade[] HealUpgrade;
}