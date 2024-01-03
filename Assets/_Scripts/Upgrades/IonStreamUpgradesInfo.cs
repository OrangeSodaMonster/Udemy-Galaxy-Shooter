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
    public float RangeFromPlayer;
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
    public IonStreamPowerUpgrade[] PowerUpgrades;
    public IonStreamCadencyUpgrade[] CadencyUpgrades;
    public IonStreamRangeUpgrade[] RangeUpgrades;
    public IonStreamHitNumUpgrade[] HitNumUpgrades;
}