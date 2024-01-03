using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShieldStrenghtUpgrade
{
    public ResourceNumber[] Cost;
    public int Strenght;
}
[Serializable]
public struct ShieldRecoveryUpgrade
{
    public ResourceNumber[] Cost;
    public float TimeBetween;
}

[CreateAssetMenu(fileName = "ShieldUpgradesInfo", menuName = "MySOs/ShieldUpgradesInfo")]
public class ShieldsUpgradesInfo : ScriptableObject
{
    public ShieldStrenghtUpgrade[] StrenghtUpgrades;
    public ShieldRecoveryUpgrade[] RecoveryUpgrades;
}