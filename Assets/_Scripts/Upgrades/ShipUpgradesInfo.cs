using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShipHPUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector]public ResourceNumber[] Cost;
    public int HP;
}

[Serializable]
public struct ShipSpeedUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    public float Speed;
}

[Serializable]
public struct ShipManobrabilityUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [HorizontalGroup("G"), Tooltip("TurningSpeed")]
    public float TurningSpeed;
    [HorizontalGroup("G"), Tooltip("TimeToStop")]
    public float TimeToStop;
    [HorizontalGroup("G"), Tooltip("TimeToStopRotating")]
    public float TimeToStopRotating;
}

[Serializable]
public struct ShipTractorBeamUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [HorizontalGroup("G")]
    public float RadiusMod;
    [HorizontalGroup("G")]
    public float PullForce;
    [HorizontalGroup("GG")]
    public float Alpha;
    [HorizontalGroup("GG"), Tooltip("TextureSpeed")]
    public float TextureSpeed;
}

[CreateAssetMenu(fileName = "ShipUpgradesInfo", menuName = "MySOs/ShipUpgradesInfo")]
public class ShipUpgradesInfo : ScriptableObject
{
    [GUIColor("#dcffc9")]
    public ShipHPUpgrade[] HP_Upgrade;
    [GUIColor("#fdffc9")]
    public ShipSpeedUpgrade[] SpeedUpgrade;
    [GUIColor("#ded9ff")]
    public ShipManobrabilityUpgrade[] ManobrabilityUpgrade;
    [GUIColor("#ffd9f2")]
    public ShipTractorBeamUpgrade[] TractorBeamUpgrade;

    private void OnValidate()
    {
        ConvertHP();
        ConvertSpeed();
        ConvertManobrability();
        ConvertTractor();
    }

    void ConvertHP()
    {
        for (int i = 0; i < HP_Upgrade.Length; i++)
        {
            HP_Upgrade[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(HP_Upgrade[i].CostLine);
        }
    }

    void ConvertSpeed()
    {
        for (int i = 0; i < SpeedUpgrade.Length; i++)
        {
            SpeedUpgrade[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(SpeedUpgrade[i].CostLine);
        }
    }

    void ConvertManobrability()
    {
        for (int i = 0; i < ManobrabilityUpgrade.Length; i++)
        {
            ManobrabilityUpgrade[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(ManobrabilityUpgrade[i].CostLine);
        }
    }

    void ConvertTractor()
    {
        for (int i = 0; i < TractorBeamUpgrade.Length; i++)
        {
            TractorBeamUpgrade[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(TractorBeamUpgrade[i].CostLine);
        }
    }
}