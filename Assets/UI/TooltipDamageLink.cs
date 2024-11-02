using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipDamageLink : TooltipLink
{
    enum DamageSource
    {
        FrontLaser = 0,
        SpreadLaser = 1,
        LateralLaser = 2,
        BackLaser = 3,
        IonStream = 4,
        Drone1 = 5,
        Drone2 = 6,
        Drone3 = 7,
    }
    [SerializeField, PropertyOrder(-1)] DamageSource damageSource;

    public override string GetValue()
    {
        switch (damageSource)
        {
            case DamageSource.FrontLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.DamageLevel - 1].Damage.ToString();
            case DamageSource.SpreadLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.DamageLevel - 1].Damage.ToString();
            case DamageSource.LateralLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.DamageLevel - 1].Damage.ToString();
            case DamageSource.BackLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.DamageLevel - 1].Damage.ToString();
            case DamageSource.IonStream:
                return PlayerUpgradesManager.Instance.IonStreamUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.DamageLevel - 1].Damage.ToString();
            case DamageSource.Drone1:
                return PlayerUpgradesManager.Instance.DroneUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.DamageLevel - 1].DamagePerSecond.ToString();
            case DamageSource.Drone2:
                return PlayerUpgradesManager.Instance.DroneUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.DamageLevel - 1].DamagePerSecond.ToString();
            case DamageSource.Drone3:
                return PlayerUpgradesManager.Instance.DroneUpgradesInfo.PowerUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.DamageLevel - 1].DamagePerSecond.ToString();
            default:
                return "--";
        }
    }
}
