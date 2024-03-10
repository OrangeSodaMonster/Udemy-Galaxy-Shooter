using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct LaserPowerUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [Range(0,100), LabelWidth(50), GUIColor("#ff4242")]
    public int Damage;
    [HorizontalGroup("G"), LabelWidth(38)]
    public Sprite Sprite;
    [HorizontalGroup("G", .6f), GradientUsage(true)]
    public Gradient VFXGradient;
}

[Serializable]
public struct LaserCadencyUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [Range(0.05f, 1), LabelWidth(100), GUIColor("#7863ff")]
    public float TimeBetween;
}

[CreateAssetMenu(fileName = "LaserUpgradesInfo", menuName = "MySOs/LaserUpgradesInfo")]
public class LaserUpgradesInfo : ScriptableObject
{
    [BoxGroup("UnlockCost"), HideLabel]
    [SerializeField] UpgradeCost UnlockCostLine;
    [GUIColor("#c9f8ff")]
    [HideInInspector] public ResourceNumber[] UnlockCost;
    [GUIColor("#ffc9c9")]
    public LaserPowerUpgrade[] PowerUpgrades;
    [GUIColor("#ded9ff")]
    public LaserCadencyUpgrade[] CadencyUpgrades;

    private void OnValidate()
    {
        ConvertUnlock();
        ConvertPower();
        ConvertCadency();
    }

    void ConvertPower()
    {
        for (int i = 0; i < PowerUpgrades.Length; i++)
        {
            PowerUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(PowerUpgrades[i].CostLine);
        }
    }

    void ConvertCadency()
    {
        for (int i = 0; i < CadencyUpgrades.Length; i++)
        {
            CadencyUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(CadencyUpgrades[i].CostLine);
        }
    }

    void ConvertUnlock()
    {
        UnlockCost = PlayerCollectiblesCount.ConvertUpgradeCost(UnlockCostLine);
    }
}