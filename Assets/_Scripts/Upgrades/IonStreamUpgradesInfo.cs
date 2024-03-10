using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct IonStreamPowerUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    public int Damage;
    public float Widht;
    public Material Material;
    public Gradient VFXGradient;
}

[Serializable]
public struct IonStreamCadencyUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    public float TimeBetween;
}

[Serializable]
public struct IonStreamRangeUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    [HorizontalGroup("G"), LabelWidth(115), Tooltip("RangeFromPlayer")]
    public float RangeFromPlayer;
    [HorizontalGroup("G"), LabelWidth(100), Tooltip("RangeFromHit")]
    public float RangeFromHit;
}

[Serializable]
public struct IonStreamHitNumUpgrade
{
    [BoxGroup("Cost", ShowLabel = false), HideLabel]
    public UpgradeCost CostLine;
    [HideInInspector] public ResourceNumber[] Cost;
    public int NumberOfHits;
}

[CreateAssetMenu(fileName = "IonStreamUpgradesInfo", menuName = "MySOs/IonStreamUpgradesInfo")]
public class IonStreamUpgradesInfo : ScriptableObject
{
    [BoxGroup("UnlockCost"), HideLabel]
    [SerializeField] UpgradeCost UnlockCostLine;
    [GUIColor("#c9f8ff")]
    [HideInInspector] public ResourceNumber[] UnlockCost;
    [GUIColor("#ffc9c9")]
    public IonStreamPowerUpgrade[] PowerUpgrades;
    [GUIColor("#ded9ff")]
    public IonStreamCadencyUpgrade[] CadencyUpgrades;
    [GUIColor("#fdffc9")]
    public IonStreamRangeUpgrade[] RangeUpgrades;
    [GUIColor("#ffd9f2")]
    public IonStreamHitNumUpgrade[] HitNumUpgrades;

    private void OnValidate()
    {
        ConvertUnlock();
        ConvertPower();
        ConvertCadency();
        ConvertRange();
        ConvertHitNum();
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

    void ConvertRange()
    {
        for (int i = 0; i < RangeUpgrades.Length; i++)
        {
            RangeUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(RangeUpgrades[i].CostLine);
        }
    }
    void ConvertHitNum()
    {
        for (int i = 0; i < HitNumUpgrades.Length; i++)
        {
            HitNumUpgrades[i].Cost = PlayerCollectiblesCount.ConvertUpgradeCost(HitNumUpgrades[i].CostLine);
        }
    }

    void ConvertUnlock()
    {
        UnlockCost = PlayerCollectiblesCount.ConvertUpgradeCost(UnlockCostLine);
    }
}