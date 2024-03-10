using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct IonStreamPowerUpgrade
{
    public ResourceNumber[] Cost;
    public int Damage;
    public float Widht;
    public Material Material;
    public Gradient VFXGradient;
}

[Serializable]
public struct IonStreamCadencyUpgrade
{
    public ResourceNumber[] Cost;
    public float TimeBetween;
}

[Serializable]
public struct IonStreamRangeUpgrade
{
    public ResourceNumber[] Cost;
    [HorizontalGroup("G"), LabelWidth(115), Tooltip("RangeFromPlayer")]
    public float RangeFromPlayer;
    [HorizontalGroup("G"), LabelWidth(100), Tooltip("RangeFromHit")]
    public float RangeFromHit;
}

[Serializable]
public struct IonStreamHitNumUpgrade
{
    public ResourceNumber[] Cost;
    public int NumberOfHits;
}

[CreateAssetMenu(fileName = "IonStreamUpgradesInfo", menuName = "MySOs/IonStreamUpgradesInfo")]
public class IonStreamUpgradesInfo : ScriptableObject
{
    [GUIColor("#c9f8ff")]
    public ResourceNumber[] UnlockCost;
    [GUIColor("#ffc9c9")]
    public IonStreamPowerUpgrade[] PowerUpgrades;
    [GUIColor("#ded9ff")]
    public IonStreamCadencyUpgrade[] CadencyUpgrades;
    [GUIColor("#fdffc9")]
    public IonStreamRangeUpgrade[] RangeUpgrades;
    [GUIColor("#ffd9f2")]
    public IonStreamHitNumUpgrade[] HitNumUpgrades;
}