using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShieldStrenghtUpgrade
{
    public ResourceNumber[] Cost;
    [Range(0,50), HorizontalGroup("G", 0.6f), GUIColor("#b8ff91")]
    public int Strenght;
    [HorizontalGroup("G"), LabelWidth(110)]
    public float AlphaAtThisStr;
}
[Serializable]
public struct ShieldRecoveryUpgrade
{
    public ResourceNumber[] Cost;
    [GUIColor("#7863ff")]
    public float TimeBetween;
}

[CreateAssetMenu(fileName = "ShieldUpgradesInfo", menuName = "MySOs/ShieldUpgradesInfo")]
public class ShieldsUpgradesInfo : ScriptableObject
{
    [GUIColor("#c9f8ff")]
    public ResourceNumber[] UnlockCost;
    [GUIColor("#dcffc9")]
    public ShieldStrenghtUpgrade[] StrenghtUpgrades;
    [GUIColor("#ded9ff")]
    public ShieldRecoveryUpgrade[] RecoveryUpgrades;
}