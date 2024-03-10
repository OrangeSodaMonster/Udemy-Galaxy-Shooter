using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShieldStrenghtUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [Range(0,50), HorizontalGroup("G", 0.6f), GUIColor("#b8ff91")]
    public int Strenght;
    [HorizontalGroup("G"), LabelWidth(110)]
    public float AlphaAtThisStr;
}
[Serializable]
public struct ShieldRecoveryUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [GUIColor("#7863ff")]
    public float TimeBetween;
}

[CreateAssetMenu(fileName = "ShieldUpgradesInfo", menuName = "MySOs/ShieldUpgradesInfo")]
public class ShieldsUpgradesInfo : ScriptableObject
{
    [BoxGroup("UnlockCost"), HideLabel]
    [SerializeField] UpgradeCost UnlockCostLine;
    [GUIColor("#c9f8ff")]
    [HideInInspector] public ResourceNumber[] UnlockCost;
    [GUIColor("#dcffc9")]
    public ShieldStrenghtUpgrade[] StrenghtUpgrades;
    [GUIColor("#ded9ff")]
    public ShieldRecoveryUpgrade[] RecoveryUpgrades;

    private void OnValidate()
    {
        ConvertUnlock();
        ConvertStrenght();
        ConvertRecovery();
    }

    void ConvertStrenght()
    {
        for (int i = 0; i < StrenghtUpgrades.Length; i++)
        {
            StrenghtUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(StrenghtUpgrades[i].CostLine);
        }
    }

    void ConvertRecovery()
    {
        for (int i = 0; i < RecoveryUpgrades.Length; i++)
        {
            RecoveryUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(RecoveryUpgrades[i].CostLine);
        }
    }

    void ConvertUnlock()
    {
        UnlockCost = PlayerCollectiblesCount.ConvertUpgradeCost(UnlockCostLine);
    }
}