using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;



[CreateAssetMenu(fileName = "EnemyInfo", menuName = "MySOs/EnemyInfo")]
[InlineEditor]
public class EnemyInfoSO : ScriptableObject
{
    #region "values"

    [BoxGroup("Enemy Type")]
    [SerializeField, HorizontalGroup("Enemy Type/1")] EnemyType enemyType;
    public EnemyType EnemyType => enemyType;
    [SerializeField, HorizontalGroup("Enemy Type/1")] EnemyColor enemyColor;
    public EnemyColor EnemyColor => enemyColor;

    [SerializeField] EnemyInfoMasterSO masterSO;

    [BoxGroup("Basics"), HorizontalGroup("Basics/G", .2f), PreviewField(85, Alignment = ObjectFieldAlignment.Left), HideLabel()]
	[SerializeField] GameObject enemy;

	[ReadOnly, BoxGroup("Basics"), VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#ff5959")]
	public int MaxHP;	
	[ReadOnly, BoxGroup("Basics")][VerticalGroup("Basics/G/1"), LabelWidth(100), Space, GUIColor("#8559ff")]
	public float Speed;
    [ReadOnly, BoxGroup("Basics")][VerticalGroup("Basics/G/2"), LabelWidth(100), Space(30), GUIColor("#8559ff"), Tooltip("in %")]    
	public float SpeedVarPerc = 15;	
	[ReadOnly, BoxGroup("Basics")][VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#efff85")]
	public int CollisionDamage;
	[ReadOnly, BoxGroup("Basics")][VerticalGroup("Basics/G/2"), LabelWidth(100), GUIColor("#efff85")]
	public float ImpactVelocity;

    //Shooting

    [ReadOnly, BoxGroup("Shooting"), HorizontalGroup("Shooting/G"), LabelWidth(90), GUIColor("#ffb0b0")]
    public float ShootCD;
    [ReadOnly, Tooltip("in Seconds"), HorizontalGroup("Shooting/G"), LabelWidth(90), GUIColor("#ffb0b0")]
    public float ShootCdVar;

    // Drops

    [BoxGroup("Drops"), HorizontalGroup("Drops/Drops"), LabelWidth(60), Range(0,10), GUIColor("#b0fff6")]
	public int MinDrops;
	[HorizontalGroup("Drops/Drops"), LabelWidth(60), Range(0, 10), GUIColor("#b0fff6")]
	public int MaxDrops;
    [BoxGroup("Drops/Drop Weights"), HideLabel]
    [SerializeField] CollectibleLine DropWeightsLine;
    DropsToSpawn[] DropsChances;
    [FoldoutGroup("Drops/Drops Guaranteed"), HideLabel]
    [SerializeField] CollectibleLine GuaranteedLine;
    DropsGuaranteed[] DropsGuaranteed;    

    // PowerUps

    [BoxGroup("PowerUps"), LabelWidth(100), GUIColor("#d5b0ff")]
	[Range(0,100)] public float PuDropChance;
    [BoxGroup("PowerUps")]
    [ReadOnly] public List<PowerUpDrops> PuDrops;

    // EnemyShip

    [ReadOnly, FoldoutGroup("EnemyShip"), HorizontalGroup("EnemyShip/G"), LabelWidth(50)]
	public float XSpeed;
	[ReadOnly, HorizontalGroup("EnemyShip/G"), LabelWidth(100)]
	public float RotChangeTime;
	[ReadOnly, HorizontalGroup("EnemyShip/G"), LabelWidth(80)]
	public float RotTimeVar;
    [ReadOnly, HorizontalGroup("EnemyShip/D"), LabelWidth(80)]
    public float DroneSpawnTime;
    [ReadOnly, HorizontalGroup("EnemyShip/D"), LabelWidth(80)]
    public float DroneSpawnTimeVar;

    // Sentinel

    [FoldoutGroup("Sentinel"), HorizontalGroup("Sentinel/G", 0.25f), LabelWidth(75)]
	public float SentRange;
	[HorizontalGroup("Sentinel/G"), LabelWidth(80)]
	public int SentDamage;
	[HorizontalGroup("Sentinel/G", 0.45f), LabelWidth(125)]
	public float SentDamageInterval;
    [HorizontalGroup("Sentinel/G2")]
    public float ShootDistance;
    [HorizontalGroup("Sentinel/G2")]
    public float RotateInterval;
    [HorizontalGroup("Sentinel/G2"), Tooltip("Rotate After Shoot Interval")]
    public float RotateAfterShootInterval;

    ///////////////

    [HorizontalGroup("Top", .65f), PropertyOrder(-1), LabelWidth(40)]
    public string Name;

    [HorizontalGroup("Top"), PropertyOrder(-1), HideLabel(), ReadOnly()]
    [SerializeField] Color saveStateColor = Color.green;

    #endregion

    private void OnValidate()
    {
        saveStateColor = Color.red;
        masterSO.OnUpdateAndSaveAll.AddListener(UpdateValues);
    }

    void ConvertDropChance()
    {
        DropsChances = new DropsToSpawn[4];
        DropsChances[0].drop = ResourceType.Metal;
        DropsChances[0].spawnWeight = DropWeightsLine.MetalCrumb;
        DropsChances[1].drop = ResourceType.RareMetal;
        DropsChances[1].spawnWeight = DropWeightsLine.RareMetalCrumb;
        DropsChances[2].drop = ResourceType.EnergyCristal;
        DropsChances[2].spawnWeight = DropWeightsLine.EnergyCristal;
        DropsChances[3].drop = ResourceType.CondensedEnergyCristal;
        DropsChances[3].spawnWeight = DropWeightsLine.CondensedEnergyCristal;
    }

    void ConvertDropGuaranteed()
    {
        DropsGuaranteed = new DropsGuaranteed[4];
        DropsGuaranteed[0].drop = ResourceType.Metal;
        DropsGuaranteed[0].Amount = (int)GuaranteedLine.MetalCrumb;
        DropsGuaranteed[1].drop = ResourceType.RareMetal;
        DropsGuaranteed[1].Amount = (int)GuaranteedLine.RareMetalCrumb;
        DropsGuaranteed[2].drop = ResourceType.EnergyCristal;
        DropsGuaranteed[2].Amount = (int)GuaranteedLine.EnergyCristal;
        DropsGuaranteed[3].drop = ResourceType.CondensedEnergyCristal;
        DropsGuaranteed[3].Amount = (int)GuaranteedLine.CondensedEnergyCristal;
    }

    [Button("SavePrefab",ButtonSizes.Medium,  ButtonAlignment = 1, Stretch = false), PropertyOrder(-1), GUIColor("Cyan"), HorizontalGroup("Top")]
    public void UpdateValues()
    {
        PuDrops = masterSO.GetPowerUpList(enemyColor);
        ConvertDropChance();
        ConvertDropGuaranteed();

        MaxHP = masterSO.CalculateHP(enemyType, enemyColor);
        Speed = masterSO.CalculateSpeed(enemyType, enemyColor);
        XSpeed = masterSO.CalculateXSpeed(enemyType, enemyColor);
        SpeedVarPerc = masterSO.SpeedVariationPerc;
        CollisionDamage = masterSO.CalculateColDamage(enemyType, enemyColor);
        if (enemyType == EnemyType.Sentinel) CollisionDamage = masterSO.ColDamageSentinelGreen;
        ImpactVelocity = masterSO.CalculateImpactVelocity(enemyType, enemyColor);
        if (enemyType == EnemyType.Sentinel) ImpactVelocity = masterSO.ImpactSentinelGreen;
        ShootCD = masterSO.CalculateShootCD(enemyType, enemyColor);
        ShootCdVar = masterSO.ShootCdVariationPerc * 0.01f * ShootCD;
        RotChangeTime = masterSO.CalculateRotationChangeTime(enemyType, enemyColor);
        RotTimeVar = masterSO.ShipRotationTimeVariationPerc * 0.01f * ShootCD;
        DroneSpawnTime = masterSO.CalculateDroneSpawnTime(enemyType, enemyColor);
        DroneSpawnTimeVar = masterSO.ShipDroneSpawnVariationPerc * 0.01f * DroneSpawnTime;

        if (enemy.TryGetComponent(out EnemyHP enemyHP))
        {
            enemyHP.MaxHP = MaxHP;
        }

        if (enemy.TryGetComponent(out AsteroidMove asteroidMove))
        {
            asteroidMove.BaseSpeed = Speed;
            asteroidMove.SpeedVariationPerc = SpeedVarPerc;
        }

        if (enemy.TryGetComponent(out EnemyDroneMove droneMove))
        {
            droneMove.BaseSpeed = Speed;
            droneMove.SpeedVariationPerc = SpeedVarPerc;
        }

        if (enemy.TryGetComponent(out EnemyShipMove shipMove))
        {
            shipMove.MaxYSpeed = Speed;
            shipMove.MaxXSpeed = XSpeed;
            shipMove.RotationChangeTime = RotChangeTime;
            shipMove.RotationChangeTimeVar = RotTimeVar;
        }

        if (enemy.TryGetComponent(out CollisionWithPlayer collision))
        {
            collision.Damage = CollisionDamage;
            collision.ImpactVelocity = ImpactVelocity;
        }

        if (enemy.TryGetComponent(out EnemyDropDealer dropDealer))
        {
            dropDealer.DropsToSpawn = DropsChances;
            dropDealer.MinDropsNum = MinDrops;
            dropDealer.MaxDropsNum = MaxDrops;

            dropDealer.dropsGuaranteed = DropsGuaranteed;
        }

        if (enemy.TryGetComponent(out EnemyProjectileShoot shooter))
        {
            shooter.BaseShootCD = ShootCD;
            shooter.ShootCDVariation = ShootCdVar;
        }

        if (enemy.TryGetComponent(out PowerUpDrop powerUpDrop))
        {
            powerUpDrop.ChanceToDrop = PuDropChance;
            powerUpDrop.PuDrops = PuDrops;
        }

        foreach(Transform child in enemy.transform)
        {
            if (child.TryGetComponent(out SentinelAttack sentAttack))
            {
                sentAttack.Range = SentRange;
                sentAttack.Damage = SentDamage;
                sentAttack.DamageInterval = SentDamageInterval;
            }
        }
        if (enemy.TryGetComponent(out SentinelShoot sentinelShoot))
        {
            sentinelShoot.ShootInterval = ShootCD;
            sentinelShoot.ShootDistance = ShootDistance;
            sentinelShoot.RotateInterval = RotateInterval;
            sentinelShoot.RotateAfterShootInterval = RotateAfterShootInterval;
        }

        if (enemy.TryGetComponent(out SpawnDroneFromShip spawnDrone))
        {
            spawnDrone.BaseSpawnCD = DroneSpawnTime;
            spawnDrone.SpawnCDVariation = DroneSpawnTimeVar;
        }

        saveStateColor = Color.green;

        #if UNITY_EDITOR
            PrefabUtility.SavePrefabAsset(enemy, out bool success);
            Debug.Log(success ? $"Saved {name}" : "Could not save asset");
        #endif
    }
}