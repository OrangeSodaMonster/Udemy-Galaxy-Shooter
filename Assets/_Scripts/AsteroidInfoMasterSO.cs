using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum AsteroidSize
{
    Small = 0,
    Medium = 1,
    Big = 2,
}

[Serializable]
public enum AsteroidMaterial
{
    Base = 0,
    Metal = 1,
    Rare = 2,
    VeryRare = 3,
}

[Serializable]
public enum AsteroidCrystal
{
    None = 0,
    Blue = 1,
    Pink = 2,
}

[Serializable]
public struct Drops
{
    [BoxGroup("Drops"), HorizontalGroup("Drops/Drops"), LabelWidth(60), Range(0, 10), GUIColor("#b0fff6")]
    public int MinDrops;
    [HorizontalGroup("Drops/Drops"), LabelWidth(60), Range(0, 10), GUIColor("#b0fff6")]
    public int MaxDrops;
    [BoxGroup("Drops/Drop Weights"), HideLabel]
    public CollectibleLine DropWeightsLine;
    [BoxGroup("Drops/Drops Guaranteed"), HideLabel, Space]
    public CollectibleLine GuaranteedLine;
}

[CreateAssetMenu(fileName = "MasterAsteroidInfo", menuName = "MySOs/MasterAsteroidInfo")]
public class AsteroidInfoMasterSO : BaseMasterEnemyInfo
{
    #region "Input"

    [SerializeField, GUIColor("#8559ff")] float speedVariationPerc;
    public float SpeedVariationPerc => speedVariationPerc;

    [BoxGroup("BaseSmall")]
    [SerializeField, HorizontalGroup("BaseSmall/1"), Tooltip("HP"), GUIColor("#ff5959")]
    int hPBaseSmall;
    [SerializeField, HorizontalGroup("BaseSmall/1"), Tooltip("Speed"), GUIColor("#8559ff")]
    float speedBaseSmall;
    [SerializeField, HorizontalGroup("BaseSmall/2"), Tooltip("Collision Damage"), GUIColor("#efff85")]
    int colDamageBaseSmall;
    [SerializeField, HorizontalGroup("BaseSmall/2"), Tooltip("Impact Velocity"), GUIColor("#efff85")]
    float impactVelBaseSmall;
        
    // HP

    [BoxGroup("HP Size Multiplier")]
    [SerializeField, HorizontalGroup("HP Size Multiplier/1"), GUIColor("Yellow")] float mediumHpMult;
    [SerializeField, HorizontalGroup("HP Size Multiplier/1"), GUIColor("Orange")] float bigHpMult;

    [BoxGroup("HP Material Multiplier")]
    [SerializeField, HorizontalGroup("HP Material Multiplier/1"), GUIColor("#c7dcff")] float metalHpMult;
    [SerializeField, HorizontalGroup("HP Material Multiplier/1"), GUIColor("#84e094")] float rareHpMult;
    public float RareHpMult => rareHpMult;
    [SerializeField, HorizontalGroup("HP Material Multiplier/1"), GUIColor("#388061")] float veryRareHpMult;
    public float VeryRareHpMult => veryRareHpMult;

    [BoxGroup("HP Crystal Multiplier")]
    [SerializeField, HorizontalGroup("HP Crystal Multiplier/1"), GUIColor("#59d3ff")] float blueCrystalHpMult;
    public float BlueCrystalHpMult => blueCrystalHpMult;
    [SerializeField, HorizontalGroup("HP Crystal Multiplier/1"), GUIColor("#d359ff")] float pinkCrystalHpMult;
    public float PinkCrystalHpMult => pinkCrystalHpMult;
 
    // Speed

    [BoxGroup("Speed Size Multiplier")]
    [SerializeField, HorizontalGroup("Speed Size Multiplier/1"), GUIColor("Yellow")] float mediumSpeedMult;
    [SerializeField, HorizontalGroup("Speed Size Multiplier/1"), GUIColor("Orange")] float bigSpeedMult;

    [BoxGroup("Speed Material Multiplier")]
    [SerializeField, HorizontalGroup("Speed Material Multiplier/1"), GUIColor("#c7dcff")] float metalSpeedMult;
    [SerializeField, HorizontalGroup("Speed Material Multiplier/1"), GUIColor("#84e094")] float rareSpeedMult;
    [SerializeField, HorizontalGroup("Speed Material Multiplier/1"), GUIColor("#388061")] float veryRareSpeedMult;
    
    // Collision Damage

    [BoxGroup("ColDamage Size Multiplier")]
    [SerializeField, HorizontalGroup("ColDamage Size Multiplier/1"), GUIColor("Yellow")] float mediumColDamageMult;
    [SerializeField, HorizontalGroup("ColDamage Size Multiplier/1"), GUIColor("Orange")] float bigColDamageMult;

    [BoxGroup("ColDamage Material Multiplier")]
    [SerializeField, HorizontalGroup("ColDamage Material Multiplier/1"), GUIColor("#c7dcff")] float metalColDamageMult;
    [SerializeField, HorizontalGroup("ColDamage Material Multiplier/1"), GUIColor("#84e094")] float rareColDamageMult;
    [SerializeField, HorizontalGroup("ColDamage Material Multiplier/1"), GUIColor("#388061")] float veryRareColDamageMult;

    [BoxGroup("ColDamage Crystal Multiplier")]
    [SerializeField, HorizontalGroup("ColDamage Crystal Multiplier/1"), GUIColor("#59d3ff")] float blueCrystalColDamageMult;
    [SerializeField, HorizontalGroup("ColDamage Crystal Multiplier/1"), GUIColor("#d359ff")] float pinkCrystalColDamageMult;

    // Impact

    [BoxGroup("Impact Size Multiplier")] // multiplicar pelo Material Speed também
    [SerializeField, HorizontalGroup("Impact Size Multiplier/1"), GUIColor("Yellow")] float mediumImpactMult;
    [SerializeField, HorizontalGroup("Impact Size Multiplier/1"), GUIColor("Orange")] float bigImpactMult;

    // Drops
    [Space]
    [SerializeField, GUIColor("#d9cc9e")] Drops baseDestroyDrops;
    [SerializeField, GUIColor("#d9cc9e")] Drops baseSplitDrops;
    [Space]
    [SerializeField, GUIColor("#c7dcff")] Drops metalDestroyDrops;
    [SerializeField, GUIColor("#c7dcff")] Drops metalSplitDrops;
    [Space]
    [SerializeField, GUIColor("#84e094")] Drops rareDestroyDrops;
    [SerializeField, GUIColor("#84e094")] Drops rareSplitDrops;
    [Space]
    [SerializeField, GUIColor("#59d3ff")] Drops rareBlueDestroyDrops;
    [SerializeField, GUIColor("#59d3ff")] Drops rareBlueSplitDrops;
    [Space]
    [SerializeField, GUIColor("#d359ff")] Drops rarePinkDestroyDrops;
    [SerializeField, GUIColor("#d359ff")] Drops rarePinkSplitDrops;
    [Space]
    [SerializeField, GUIColor("#388061")] Drops veryRareDestroyDrops;
    [SerializeField, GUIColor("#388061")] Drops veryRareSplitDrops;
    [Space]
    [SerializeField, GUIColor("#59d3ff")] Drops veryRareBlueDestroyDrops;
    [SerializeField, GUIColor("#59d3ff")] Drops veryRareBlueSplitDrops;
    [Space]
    [SerializeField, GUIColor("#d359ff")] Drops veryRarePinkDestroyDrops;
    [SerializeField, GUIColor("#d359ff")] Drops veryRarePinkSplitDrops;


    #endregion "Input"

    [HorizontalGroup("Top", 0.7f), PropertyOrder(-1), HideLabel(), ReadOnly()]
    [SerializeField] UnityEngine.Color saveStateColor = UnityEngine.Color.green;

    [HideInInspector] public UnityEvent OnUpdateAndSaveAll = new();

    [Button("UpdateAndSaveAll", ButtonSizes.Medium, ButtonAlignment = 1, Stretch = false), PropertyOrder(-1), GUIColor("Cyan"), HorizontalGroup("Top")]
    public void UpdateAndSaveAll()
    {
        OnUpdateAndSaveAll.Invoke();
        saveStateColor = UnityEngine.Color.green;
    }

    private void OnValidate()
    {
        saveStateColor = UnityEngine.Color.red;
    }


    public int CalculateHP(AsteroidSize size, AsteroidMaterial material, AsteroidCrystal crystal)
    {
        float mult1;
        switch (size)
        {
            case AsteroidSize.Small:
                mult1 = 1;
                break;
            case AsteroidSize.Medium:
                mult1 = mediumHpMult;
                break;
            case AsteroidSize.Big:
                mult1 = bigHpMult;
                break;
            default:
                mult1 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        float mult2;
        switch (material)
        {
            case AsteroidMaterial.Base:
                mult2 = 1;
                break;
            case AsteroidMaterial.Metal:
                mult2 = metalHpMult;
                break;
            case AsteroidMaterial.Rare:
                mult2 = rareHpMult;
                break;
            case AsteroidMaterial.VeryRare:
                mult2 = veryRareHpMult;
                break;
            default:
                mult2 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        float mult3;
        switch (crystal)
        {
            case AsteroidCrystal.None:
                mult3 = 1;
                break;
            case AsteroidCrystal.Blue:
                mult3 = blueCrystalHpMult;
                break;
            case AsteroidCrystal.Pink:
                mult3 = pinkCrystalHpMult;
                break;
            default:
                mult3 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }       

        int hp = (int)(hPBaseSmall * mult1 * mult2 * mult3);

        if (hp%5 != 0)
        {
            hp -= hp%5;
        }

        return hp;
    }

    public float CalculateSpeed(AsteroidSize size, AsteroidMaterial material, AsteroidCrystal crystal)
    {
        float mult1;
        switch (size)
        {
            case AsteroidSize.Small:
                mult1 = 1;
                break;
            case AsteroidSize.Medium:
                mult1 = mediumSpeedMult;
                break;
            case AsteroidSize.Big:
                mult1 = bigSpeedMult;
                break;
            default:
                mult1 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        float mult2;
        switch (material)
        {
            case AsteroidMaterial.Base:
                mult2 = 1;
                break;
            case AsteroidMaterial.Metal:
                mult2 = metalSpeedMult;
                break;
            case AsteroidMaterial.Rare:
                mult2 = rareSpeedMult;
                break;
            case AsteroidMaterial.VeryRare:
                mult2 = veryRareSpeedMult;
                break;
            default:
                mult2 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }       

        return speedBaseSmall * mult1 * mult2;
    }

    public int CalculateColDamage(AsteroidSize size, AsteroidMaterial material, AsteroidCrystal crystal)
    {
        float mult1;
        switch (size)
        {
            case AsteroidSize.Small:
                mult1 = 1;
                break;
            case AsteroidSize.Medium:
                mult1 = mediumColDamageMult;
                break;
            case AsteroidSize.Big:
                mult1 = bigColDamageMult;
                break;
            default:
                mult1 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        float mult2;
        switch (material)
        {
            case AsteroidMaterial.Base:
                mult2 = 1;
                break;
            case AsteroidMaterial.Metal:
                mult2 = metalColDamageMult;
                break;
            case AsteroidMaterial.Rare:
                mult2 = rareColDamageMult;
                break;
            case AsteroidMaterial.VeryRare:
                mult2 = veryRareColDamageMult;
                break;
            default:
                mult2 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        float mult3;
        switch (crystal)
        {
            case AsteroidCrystal.None:
                mult3 = 1;
                break;
            case AsteroidCrystal.Blue:
                mult3 = blueCrystalColDamageMult;
                break;
            case AsteroidCrystal.Pink:
                mult3 = pinkCrystalColDamageMult;
                break;
            default:
                mult3 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        int damage = (int)(colDamageBaseSmall * mult1 * mult2 * mult3);

        if (damage%5 != 0)
        {
            damage -= damage%5;
        }

        return damage;
    }

    public float CalculateImpactVelocity(AsteroidSize size, AsteroidMaterial material, AsteroidCrystal crystal)
    {
        float mult1;
        switch (size)
        {
            case AsteroidSize.Small:
                mult1 = 1;
                break;
            case AsteroidSize.Medium:
                mult1 = mediumImpactMult;
                break;
            case AsteroidSize.Big:
                mult1 = bigImpactMult;
                break;
            default:
                mult1 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        float mult2;
        switch (material)
        {
            case AsteroidMaterial.Base:
                mult2 = 1;
                break;
            case AsteroidMaterial.Metal:
                mult2 = metalSpeedMult;
                break;
            case AsteroidMaterial.Rare:
                mult2 = rareSpeedMult;
                break;
            case AsteroidMaterial.VeryRare:
                mult2 = veryRareSpeedMult;
                break;
            default:
                mult2 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        return impactVelBaseSmall * mult1 * mult2;
    }

    public Drops GetDrops(bool isSmallestPart, AsteroidMaterial material, AsteroidCrystal crystal)
    {
        if (isSmallestPart && material == AsteroidMaterial.Base)
            return baseDestroyDrops;
        else if (!isSmallestPart && material == AsteroidMaterial.Base)
            return baseSplitDrops;

        else if (isSmallestPart && material == AsteroidMaterial.Metal)
            return metalDestroyDrops;
        else if (!isSmallestPart && material == AsteroidMaterial.Metal)
            return metalSplitDrops;

        else if (isSmallestPart && material == AsteroidMaterial.Rare && crystal == AsteroidCrystal.None)
            return rareDestroyDrops;
        else if (isSmallestPart && material == AsteroidMaterial.Rare && crystal == AsteroidCrystal.Blue)
            return rareBlueDestroyDrops;
        else if (isSmallestPart && material == AsteroidMaterial.Rare && crystal == AsteroidCrystal.Pink)
            return rarePinkDestroyDrops;

        else if (!isSmallestPart && material == AsteroidMaterial.Rare && crystal == AsteroidCrystal.None)
            return rareSplitDrops;
        else if (!isSmallestPart && material == AsteroidMaterial.Rare && crystal == AsteroidCrystal.Blue)
            return rareBlueSplitDrops;
        else if (!isSmallestPart && material == AsteroidMaterial.Rare && crystal == AsteroidCrystal.Pink)
            return rarePinkSplitDrops;

        else if (isSmallestPart && material == AsteroidMaterial.VeryRare && crystal == AsteroidCrystal.None)
            return veryRareDestroyDrops;
        else if (isSmallestPart && material == AsteroidMaterial.VeryRare && crystal == AsteroidCrystal.Blue)
            return veryRareBlueDestroyDrops;
        else if (isSmallestPart && material == AsteroidMaterial.VeryRare && crystal == AsteroidCrystal.Pink)
            return veryRarePinkDestroyDrops;

        else if (!isSmallestPart && material == AsteroidMaterial.VeryRare && crystal == AsteroidCrystal.None)
            return veryRareSplitDrops;
        else if (!isSmallestPart && material == AsteroidMaterial.VeryRare && crystal == AsteroidCrystal.Blue)
            return veryRareBlueSplitDrops;
        else 
            return veryRarePinkSplitDrops;
    }
        
}
