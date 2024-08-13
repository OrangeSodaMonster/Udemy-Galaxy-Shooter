using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroidInfo", menuName = "MySOs/AsteroidInfo")]
[InlineEditor]
public class AsteroidInfoSO : ScriptableObject
{
    #region "values"

    [BoxGroup("Asteroid Type")]
    [SerializeField, HorizontalGroup("Asteroid Type/1")] AsteroidSize asteroidSize;
    public AsteroidSize AsteroidSize => asteroidSize;
    [SerializeField, HorizontalGroup("Asteroid Type/1")] AsteroidMaterial asteroidMaterial;
    public AsteroidMaterial AsteroidMaterial => asteroidMaterial;
    [SerializeField, HorizontalGroup("Asteroid Type/1")] AsteroidCrystal asteroidCrystal;
    public AsteroidCrystal AsteroidCrystal => asteroidCrystal;

    [SerializeField] AsteroidInfoMasterSO masterSO;

    [BoxGroup("Basics"), HorizontalGroup("Basics/G", .2f), PreviewField(85, Alignment = ObjectFieldAlignment.Left), HideLabel()]
    [SerializeField] GameObject enemy;

    [ReadOnly, BoxGroup("Basics"), VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#ff5959")]
    public int MaxHP;
    [ReadOnly, BoxGroup("Basics")]
    [VerticalGroup("Basics/G/1"), LabelWidth(100), Space, GUIColor("#8559ff")]
    public float Speed;
    [ReadOnly, BoxGroup("Basics")]
    [VerticalGroup("Basics/G/2"), LabelWidth(100), Space(30), GUIColor("#8559ff"), Tooltip("in %")]
    public float SpeedVarPerc = 15;
    [ReadOnly, BoxGroup("Basics")]
    [VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#efff85")]
    public int CollisionDamage;
    [ReadOnly, BoxGroup("Basics")]
    [VerticalGroup("Basics/G/2"), LabelWidth(100), GUIColor("#efff85")]
    public float ImpactVelocity;

    // Drops

    DropsToSpawn[] DropsChances;
    DropsGuaranteed[] DropsGuaranteed;

    [ReadOnly] public Drops Drops;

    ///////////////

    [HorizontalGroup("Top", .65f), PropertyOrder(-1), LabelWidth(40)]
    public string Name;

    [HorizontalGroup("Top"), PropertyOrder(-1), HideLabel(), ReadOnly()]
    [SerializeField] UnityEngine.Color saveStateColor = UnityEngine.Color.green;

    #endregion

    private void OnValidate()
    {
        saveStateColor = UnityEngine.Color.red;
        masterSO?.OnUpdateAndSaveAll?.AddListener(UpdateValues);

        Name = GetName();
    }

    string GetName()
    {
        string material = "";
        switch (asteroidMaterial)
        {
            case AsteroidMaterial.Base:
                material = "Base";
                break;
            case AsteroidMaterial.Metal:
                material = "Metal";
                break;
            case AsteroidMaterial.Rare:
                material = "Rare";
                break;
            case AsteroidMaterial.VeryRare:
                material = "VeryRare";
                break;
        }

        string crystal = "";
        switch (asteroidCrystal)
        {
            case AsteroidCrystal.None:
                crystal = "";
                break;
            case AsteroidCrystal.Blue:
                crystal = "Blue";
                break;
            case AsteroidCrystal.Pink:
                crystal = "Pink";
                break;
        }

        string size = "";
        switch (asteroidSize)
        {
            case AsteroidSize.Small:
                size = "Small";
                break;
            case AsteroidSize.Medium:
                size = "Medium";
                break;
            case AsteroidSize.Big:
                size = "Big";
                break;
        }       

        return material+crystal+size;
    }

    void ConvertDropChance()
    {
        DropsChances = new DropsToSpawn[4];
        DropsChances[0].drop = ResourceType.Metal;
        DropsChances[0].spawnWeight = Drops.DropWeightsLine.MetalCrumb;
        DropsChances[1].drop = ResourceType.RareMetal;
        DropsChances[1].spawnWeight = Drops.DropWeightsLine.RareMetalCrumb;
        DropsChances[2].drop = ResourceType.EnergyCristal;
        DropsChances[2].spawnWeight = Drops.DropWeightsLine.EnergyCristal;
        DropsChances[3].drop = ResourceType.CondensedEnergyCristal;
        DropsChances[3].spawnWeight = Drops.DropWeightsLine.CondensedEnergyCristal;
    }

    void ConvertDropGuaranteed()
    {
        DropsGuaranteed = new DropsGuaranteed[4];
        DropsGuaranteed[0].drop = ResourceType.Metal;
        DropsGuaranteed[0].Amount = (int)Drops.GuaranteedLine.MetalCrumb;
        DropsGuaranteed[1].drop = ResourceType.RareMetal;
        DropsGuaranteed[1].Amount = (int)Drops.GuaranteedLine.RareMetalCrumb;
        DropsGuaranteed[2].drop = ResourceType.EnergyCristal;
        DropsGuaranteed[2].Amount = (int)Drops.GuaranteedLine.EnergyCristal;
        DropsGuaranteed[3].drop = ResourceType.CondensedEnergyCristal;
        DropsGuaranteed[3].Amount = (int)Drops.GuaranteedLine.CondensedEnergyCristal;
    }

    [Button("SavePrefab", ButtonSizes.Medium, ButtonAlignment = 1, Stretch = false), PropertyOrder(-1), GUIColor("Cyan"), HorizontalGroup("Top")]
    public void UpdateValues()
    {        
        Drops = masterSO.GetDrops(asteroidSize == AsteroidSize.Small, asteroidMaterial, asteroidCrystal);
        ConvertDropChance();
        ConvertDropGuaranteed();

        MaxHP = masterSO.CalculateHP(asteroidSize, asteroidMaterial, asteroidCrystal);
        Speed = masterSO.CalculateSpeed(asteroidSize, asteroidMaterial, asteroidCrystal);
        SpeedVarPerc = masterSO.SpeedVariationPerc;
        CollisionDamage = masterSO.CalculateColDamage(asteroidSize, asteroidMaterial, asteroidCrystal);
        ImpactVelocity = masterSO.CalculateImpactVelocity(asteroidSize, asteroidMaterial, asteroidCrystal);

        if (enemy.TryGetComponent(out EnemyHP enemyHP))
        {
            enemyHP.MaxHP = MaxHP;
        }

        if (enemy.TryGetComponent(out AsteroidMove asteroidMove))
        {
            asteroidMove.BaseSpeed = Speed;
            asteroidMove.SpeedVariationPerc = SpeedVarPerc;
        }        

        if (enemy.TryGetComponent(out CollisionWithPlayer collision))
        {
            collision.Damage = CollisionDamage;
            collision.ImpactVelocity = ImpactVelocity;
        }

        if (enemy.TryGetComponent(out EnemyDropDealer dropDealer))
        {
            dropDealer.DropsToSpawn = DropsChances;
            dropDealer.MinDropsNum = Drops.MinDrops;
            dropDealer.MaxDropsNum = Drops.MaxDrops;

            dropDealer.dropsGuaranteed = DropsGuaranteed;
        }  

        saveStateColor = UnityEngine.Color.green;

#if UNITY_EDITOR
        PrefabUtility.SavePrefabAsset(enemy, out bool success);
        Debug.Log(success ? $"Saved {name}" : "Could not save asset");
#endif
    }
}
