using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum ObjectiveSize
{
    Smallest = 0,
    Small = 1,
    Medium = 2,
    Big = 3,
}

[Serializable]
public enum ObjectiveMaterial
{    
    Rare = 0,
    VeryRare = 1,
}

[Serializable]
public enum ObjectiveCrystal
{
    None = 0,
    Blue = 1,
    Pink = 2,
}

[CreateAssetMenu(fileName = "MasterObjectiveInfo", menuName = "MySOs/MasterObjectiveInfo")]
public class ObjectiveInfoMasterSO : ScriptableObject
{
    #region "Input"

    [SerializeField] AsteroidInfoMasterSO asteroidMaster;

    [SerializeField, Tooltip("Smallest HP"), GUIColor("#ff5959")]
    int hPRareSmallest;

    [BoxGroup("Collision")]
    [SerializeField, HorizontalGroup("Collision/1"), Tooltip("Collision Damage"), GUIColor("#efff85")]
    int colDamage;
    public int ColDamage => colDamage;
    [SerializeField, HorizontalGroup("Collision/1"), Tooltip("Impact Velocity"), GUIColor("#efff85")]
    float impactVel;
    public float ImpactVel => impactVel;

    // HP

    [BoxGroup("HP Size Multiplier")]
    [SerializeField, HorizontalGroup("HP Size Multiplier/1"), GUIColor("Yellow")] float smallHpMult;
    [SerializeField, HorizontalGroup("HP Size Multiplier/1"), GUIColor("Orange")] float mediumHpMult;
    [SerializeField, HorizontalGroup("HP Size Multiplier/1"), GUIColor("red")] float bigHpMult;

    [BoxGroup("HP Material Multiplier")]
    [SerializeField, HorizontalGroup("HP Material Multiplier/1"), GUIColor("#388061")] float veryRareHpMult;

    [BoxGroup("HP Crystal Multiplier")]
    [SerializeField, HorizontalGroup("HP Crystal Multiplier/1"), GUIColor("#59d3ff")] float blueCrystalHpMult;
    [SerializeField, HorizontalGroup("HP Crystal Multiplier/1"), GUIColor("#d359ff")] float pinkCrystalHpMult;   

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
        GetMultipliersFromAsteroid();
    }

    void GetMultipliersFromAsteroid()
    {
        veryRareHpMult = asteroidMaster.VeryRareHpMult / asteroidMaster.RareHpMult;
        blueCrystalHpMult = asteroidMaster.BlueCrystalHpMult;
        pinkCrystalHpMult = asteroidMaster.PinkCrystalHpMult;
    }


    public int CalculateHP(ObjectiveSize size, ObjectiveMaterial material, ObjectiveCrystal crystal)
    {
        float mult1;
        switch (size)
        {
            case ObjectiveSize.Smallest:
                mult1 = 1;
                break;
            case ObjectiveSize.Small:
                mult1 = smallHpMult;
                break;
            case ObjectiveSize.Medium:
                mult1 = mediumHpMult;
                break;
            case ObjectiveSize.Big:
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
            case ObjectiveMaterial.Rare:
                mult2 = 1;
                break;
            case ObjectiveMaterial.VeryRare:
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
            case ObjectiveCrystal.None:
                mult3 = 1;
                break;
            case ObjectiveCrystal.Blue:
                mult3 = blueCrystalHpMult;
                break;
            case ObjectiveCrystal.Pink:
                mult3 = pinkCrystalHpMult;
                break;
            default:
                mult3 = 1;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        int hp = (int)(hPRareSmallest * mult1 * mult2 * mult3);

        if (hp%5 != 0)
        {
            hp -= hp%5;
        }

        return hp;
    }


    public Drops GetDrops(ObjectiveSize size, ObjectiveMaterial material, ObjectiveCrystal crystal)
    {
        AsteroidMaterial astMaterial = AsteroidMaterial.Rare;
        if (material == ObjectiveMaterial.VeryRare) astMaterial = AsteroidMaterial.VeryRare;

        AsteroidCrystal astCrystal = AsteroidCrystal.None;
        if (crystal == ObjectiveCrystal.Blue) astCrystal = AsteroidCrystal.Blue;
        else if (crystal == ObjectiveCrystal.Pink) astCrystal = AsteroidCrystal.Pink;

        return asteroidMaster.GetDrops(size == ObjectiveSize.Smallest, astMaterial, astCrystal);
    }

}
