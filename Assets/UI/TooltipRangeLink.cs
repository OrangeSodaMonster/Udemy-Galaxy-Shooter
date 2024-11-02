using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipRangeLink : TooltipLink
{    enum RangeSource
    {
        IonStream = 0,
        Drone1 = 1,
        Drone2 = 2,
        Drone3 = 3,
    }
    [SerializeField, PropertyOrder(-1)] RangeSource rangeSource;

    public override string GetValue()
    {
        switch (rangeSource)
        {
            case RangeSource.IonStream:
                return PlayerUpgradesManager.Instance.IonStreamUpgradesInfo.RangeUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.RangeLevel - 1].RangeFromPlayer.ToString();
            case RangeSource.Drone1:
                return PlayerUpgradesManager.Instance.DroneUpgradesInfo.RangeUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.RangeLevel - 1].Range.ToString();
            case RangeSource.Drone2:
                return PlayerUpgradesManager.Instance.DroneUpgradesInfo.RangeUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.RangeLevel - 1].Range.ToString();
            case RangeSource.Drone3:
                return PlayerUpgradesManager.Instance.DroneUpgradesInfo.RangeUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.RangeLevel - 1].Range.ToString();
            default:
                return "--";
        }
    }
}
