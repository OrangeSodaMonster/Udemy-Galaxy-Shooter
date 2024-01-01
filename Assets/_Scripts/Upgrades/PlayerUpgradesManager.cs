using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{
    [SerializeField] UpgradesSO defaultUpgrades;
    public UpgradesSO currentUpgrades;
    public LaserUpgradesInfo laserUpgradesInfo;
    public ShieldsUpgradesInfo shieldUpgradesInfo;


    void Awake()
    {
        //currentUpgrades = Instantiate(defaultUpgrades);
        //currentUpgrades.name = "CurrentUpgradesSO";

        ResedCurrentUpgrades();
    }

    void Update()
    {
        
    }

    void ResedCurrentUpgrades()
    {
        currentUpgrades.FrontLaserUpgrades = defaultUpgrades.FrontLaserUpgrades;
        currentUpgrades.SpreadLaserUpgrades = defaultUpgrades.SpreadLaserUpgrades;
        currentUpgrades.SideLaserUpgrades = defaultUpgrades.SideLaserUpgrades;
        currentUpgrades.BackLaserUpgrades = defaultUpgrades.BackLaserUpgrades;

        currentUpgrades.FrontShieldUpgrades = defaultUpgrades.FrontShieldUpgrades;
        currentUpgrades.RightShieldUpgrades = defaultUpgrades.RightShieldUpgrades;
        currentUpgrades.BackShieldUpgrades = defaultUpgrades.BackShieldUpgrades;
        currentUpgrades.LeftShieldUpgrades = defaultUpgrades.LeftShieldUpgrades;

        currentUpgrades.Drone_1_Upgrades = defaultUpgrades.Drone_1_Upgrades;
        currentUpgrades.Drone_2_Upgrades = defaultUpgrades.Drone_2_Upgrades;
        currentUpgrades.Drone_3_Upgrades = defaultUpgrades.Drone_3_Upgrades;

        currentUpgrades.IonStreamUpgrades = defaultUpgrades.IonStreamUpgrades;

        currentUpgrades.ShipUpgrades = defaultUpgrades.ShipUpgrades;
    }
}