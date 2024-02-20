using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct LaserPowerUpgrade
{
    public ResourceNumber[] Cost;
    public int Damage;
    public Material Material;
    [GradientUsage(true)]
    public Gradient VFXGradient;
}

[Serializable]
public struct LaserCadencyUpgrade
{
    public ResourceNumber[] Cost;
    public float TimeBetween;
}

[CreateAssetMenu(fileName = "LaserUpgradesInfo", menuName = "MySOs/LaserUpgradesInfo")]
public class LaserUpgradesInfo : ScriptableObject
{
    public ResourceNumber[] UnlockCost;
    public LaserPowerUpgrade[] PowerUpgrades;
    public LaserCadencyUpgrade[] CadencyUpgrades;
}