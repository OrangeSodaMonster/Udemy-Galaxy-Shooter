using Sirenix.OdinInspector;
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
    public ResourceNumber[] Cost;
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
}