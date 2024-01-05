using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{
    [SerializeField] UpgradesSO defaultUpgrades;
    [field: SerializeField] public UpgradesSO CurrentUpgrades { get; private set; }
    [field: SerializeField] public LaserUpgradesInfo LaserUpgradesInfo { get; private set; }
    [field: SerializeField] public ShieldsUpgradesInfo ShieldUpgradesInfo  { get; private set; }
    [field: SerializeField] public IonStreamUpgradesInfo IonStreamUpgradesInfo  { get; private set; }
    [field: SerializeField] public DroneUpgradeInfo DroneUpgradesInfo  { get; private set; }
    [field: SerializeField] public ShipUpgradesInfo ShipUpgradesInfo { get; private set; }

    public static PlayerUpgradesManager Instance;


    void Awake()
    {
        Instance = this;
        ResedCurrentUpgrades();
    }

    void ResedCurrentUpgrades()
    {
        CurrentUpgrades.FrontLaserUpgrades = defaultUpgrades.FrontLaserUpgrades;
        CurrentUpgrades.SpreadLaserUpgrades = defaultUpgrades.SpreadLaserUpgrades;
        CurrentUpgrades.SideLaserUpgrades = defaultUpgrades.SideLaserUpgrades;
        CurrentUpgrades.BackLaserUpgrades = defaultUpgrades.BackLaserUpgrades;

        CurrentUpgrades.FrontShieldUpgrades = defaultUpgrades.FrontShieldUpgrades;
        CurrentUpgrades.RightShieldUpgrades = defaultUpgrades.RightShieldUpgrades;
        CurrentUpgrades.BackShieldUpgrades = defaultUpgrades.BackShieldUpgrades;
        CurrentUpgrades.LeftShieldUpgrades = defaultUpgrades.LeftShieldUpgrades;

        CurrentUpgrades.Drone_1_Upgrades = defaultUpgrades.Drone_1_Upgrades;
        CurrentUpgrades.Drone_2_Upgrades = defaultUpgrades.Drone_2_Upgrades;
        CurrentUpgrades.Drone_3_Upgrades = defaultUpgrades.Drone_3_Upgrades;

        CurrentUpgrades.IonStreamUpgrades = defaultUpgrades.IonStreamUpgrades;

        CurrentUpgrades.ShipUpgrades = defaultUpgrades.ShipUpgrades;
    }
}