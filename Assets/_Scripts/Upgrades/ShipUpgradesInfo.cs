using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShipHPUpgrade
{
    public ResourceNumber[] Cost;
    public int HP;
}

[Serializable]
public struct ShipSpeedUpgrade
{
    public ResourceNumber[] Cost;
    public float Speed;
}

[Serializable]
public struct ShipManobrabilityUpgrade
{
    public ResourceNumber[] Cost;
    public float TurningSpeed;
    public float TimeToStop;
    public float TimeToStopRotating;
}

[Serializable]
public struct ShipTractorBeamUpgrade
{
    public ResourceNumber[] Cost;
    public float RadiusMod;
    public float MaxPullSpeed;
    public float TimeToMaxPull;
    public float Alpha;
    public float TextureSpeed;
}

[CreateAssetMenu(fileName = "ShipUpgradesInfo", menuName = "MySOs/ShipUpgradesInfo")]
public class ShipUpgradesInfo : ScriptableObject
{
    public ShipHPUpgrade[] HP_Upgrade;
    public ShipSpeedUpgrade[] SpeedUpgrade;
    public ShipManobrabilityUpgrade[] ManobrabilityUpgrade;
    public ShipTractorBeamUpgrade[] TractorBeamUpgrade;
}