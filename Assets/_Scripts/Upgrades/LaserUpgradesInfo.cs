using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct LaserPowerUpgrade
{
    public ResourceNumber[] Cost;
    public float Damage;
    public Material Material;
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
    public LaserPowerUpgrade[] PowerUpgrades;
    public LaserCadencyUpgrade[] CadencyUpgrades;
}