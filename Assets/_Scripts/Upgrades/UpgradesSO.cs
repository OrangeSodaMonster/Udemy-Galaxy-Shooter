using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LaserUpgrades
{
    public bool Enabled;
    public int DamageLevel;
    public int CadencyLevel;
}

[Serializable]
public struct ShieldUpgrades
{
    public bool Enabled;
    public int ResistenceLevel;
    public int RecoveryLevel;
}

[Serializable]
public struct IonStreamUpgrades
{
    public bool Enabled;
    public int DamageLevel;
    public int CadencyLevel;
    public int RangeLevel;
    public int NumberHitsLevel;
}

[Serializable]
public struct DronesUpgrades
{
    public bool Enabled;
    public int DamageLevel;
    public int RangeLevel;
    public int HealingLevel;
}

[Serializable]
public struct ShipUpgrades
{
    public int HPLevel;
    public int SpeedLevel;
    public int ManobrabilityLevel;
    public bool TractorBeamEnabled;
    public int TractorBeamLevel;
}

[CreateAssetMenu(fileName = "MyUpgradesSO", menuName = "MySOs/UpgradesSO")]
public class UpgradesSO : ScriptableObject
{
    public LaserUpgrades FrontLaserUpgrades;
    public LaserUpgrades SpreadLaserUpgrades;
    public LaserUpgrades SideLaserUpgrades;
    public LaserUpgrades BackLaserUpgrades;

    public ShieldUpgrades FrontShieldUpgrades;
    public ShieldUpgrades RightShieldUpgrades;
    public ShieldUpgrades BackShieldUpgrades;
    public ShieldUpgrades LeftShieldUpgrades;

    public IonStreamUpgrades IonStreamUpgrades;

    public DronesUpgrades Drone_1_Upgrades;
    public DronesUpgrades Drone_2_Upgrades;
    public DronesUpgrades Drone_3_Upgrades;

    public ShipUpgrades ShipUpgrades;
}