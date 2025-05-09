using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipRateLink : TooltipLink
{
    enum RateSource
    {
        FrontLaser = 0,
        SpreadLaser = 1,
        LateralLaser = 2,
        BackLaser = 3,
        IonStream = 4,
        Drone1 = 5,
        Drone2 = 6,
        Drone3 = 7,
        FrontShield = 8,
        RightShield = 9,
        BackShield = 10,
        LeftShield = 11,
    }
    [SerializeField, PropertyOrder(-1)] RateSource rateSource;
    PlayerHeal playerHeal;

    private void Start()
    {
        playerHeal = FindObjectOfType<PlayerHeal>();

        if(rateSource == RateSource.Drone1 || rateSource == RateSource.Drone2 || rateSource == RateSource.Drone3)
        {
            playerHeal?.OnHealTimeChange.AddListener(UpdateTooltip);
        }
    }

    public override string GetValue()
    {
        switch (rateSource)
        {
            case RateSource.FrontLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.CadencyUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel - 1].TimeBetween.ToString();
            case RateSource.SpreadLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.CadencyUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel - 1].TimeBetween.ToString();
            case RateSource.LateralLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.CadencyUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.CadencyLevel - 1].TimeBetween.ToString();
            case RateSource.BackLaser:
                return PlayerUpgradesManager.Instance.LaserUpgradesInfo.CadencyUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.CadencyLevel - 1].TimeBetween.ToString();

            case RateSource.IonStream:
                return PlayerUpgradesManager.Instance.IonStreamUpgradesInfo.CadencyUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.CadencyLevel - 1].TimeBetween.ToString();

            case RateSource.Drone1:
                return $"{PlayerStats.Instance.Drones.Drone1.HealIntervalSubtraction}";
            case RateSource.Drone2:
                return $"{PlayerStats.Instance.Drones.Drone2.HealIntervalSubtraction}";
            case RateSource.Drone3:
                return $"{PlayerStats.Instance.Drones.Drone3.HealIntervalSubtraction}";

            case RateSource.FrontShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.RecoveryUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.RecoveryLevel - 1].TimeBetween.ToString();
            case RateSource.RightShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.RecoveryUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.RecoveryLevel - 1].TimeBetween.ToString();
            case RateSource.BackShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.RecoveryUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.RecoveryLevel - 1].TimeBetween.ToString();
            case RateSource.LeftShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.RecoveryUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.RecoveryLevel - 1].TimeBetween.ToString();

            default:
                return "--";
        }
    }
}
