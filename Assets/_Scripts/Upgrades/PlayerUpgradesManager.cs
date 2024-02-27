using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{
    //[SerializeField] UpgradesSO defaultUpgrades;
    [field: SerializeField] public UpgradesSO CurrentUpgrades { get; private set; }
    [field: SerializeField] public LaserUpgradesInfo LaserUpgradesInfo { get; private set; }
    [field: SerializeField] public ShieldsUpgradesInfo ShieldUpgradesInfo  { get; private set; }
    [field: SerializeField] public IonStreamUpgradesInfo IonStreamUpgradesInfo  { get; private set; }
    [field: SerializeField] public DroneUpgradeInfo DroneUpgradesInfo  { get; private set; }
    [field: SerializeField] public ShipUpgradesInfo ShipUpgradesInfo { get; private set; }

    public static PlayerUpgradesManager Instance;


    void Awake()
    {
        if(Instance == null)
            Instance = this;

        int minUpgradeHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[0].HP;
        int maxUpgradeHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade.Length - 1].HP;

        //ResedCurrentUpgrades();
        //SaveLoad.LoadUpgrades();
    }

    public void OverwriteCurrentUpgrades(UpgradesSO newUpgrades)
    {
        CurrentUpgrades.FrontLaserUpgrades = newUpgrades.FrontLaserUpgrades;
        CurrentUpgrades.SpreadLaserUpgrades = newUpgrades.SpreadLaserUpgrades;
        CurrentUpgrades.SideLaserUpgrades = newUpgrades.SideLaserUpgrades;
        CurrentUpgrades.BackLaserUpgrades = newUpgrades.BackLaserUpgrades;

        CurrentUpgrades.FrontShieldUpgrades = newUpgrades.FrontShieldUpgrades;
        CurrentUpgrades.RightShieldUpgrades = newUpgrades.RightShieldUpgrades;
        CurrentUpgrades.BackShieldUpgrades = newUpgrades.BackShieldUpgrades;
        CurrentUpgrades.LeftShieldUpgrades = newUpgrades.LeftShieldUpgrades;

        CurrentUpgrades.Drone_1_Upgrades = newUpgrades.Drone_1_Upgrades;
        CurrentUpgrades.Drone_2_Upgrades = newUpgrades.Drone_2_Upgrades;
        CurrentUpgrades.Drone_3_Upgrades = newUpgrades.Drone_3_Upgrades;

        CurrentUpgrades.IonStreamUpgrades = newUpgrades.IonStreamUpgrades;

        CurrentUpgrades.ShipUpgrades = newUpgrades.ShipUpgrades;
    }

    //void ResedCurrentUpgrades()
    //{
    //    OverwriteCurrentUpgrades(defaultUpgrades);
    //}
}