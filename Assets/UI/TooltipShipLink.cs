using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TooltipShipLink : TooltipLink
{
    enum ShipPart
    {
        Speed = 0,
        Manobrability = 1,
        Resistence = 2,
        Tractor = 3,
    }
    [SerializeField, PropertyOrder(-1)] ShipPart part;
    float defaultManob;
    private void Start()
    {
        defaultManob = PlayerUpgradesManager.Instance.ShipUpgradesInfo.ManobrabilityUpgrade[0].TimeToStopRotating;
    }

    public override string GetValue()
    {
        switch (part)
        {
            case ShipPart.Speed:
                return PlayerUpgradesManager.Instance.ShipUpgradesInfo.SpeedUpgrade[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1].Speed.ToString();
            case ShipPart.Manobrability:
                float currentManob = PlayerUpgradesManager.Instance.ShipUpgradesInfo.ManobrabilityUpgrade[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStopRotating;
                return $"{PlayerStats.Instance.Ship.DefaultMaxTurningSpeed}";
            case ShipPart.Resistence:
                return PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP.ToString();
            case ShipPart.Tractor:
                if (!PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled)
                    return "0";
                else
                    return PlayerUpgradesManager.Instance.ShipUpgradesInfo.TractorBeamUpgrade[
                        PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].PullForce.ToString();

            default:
                return "--";
        }
    }
}