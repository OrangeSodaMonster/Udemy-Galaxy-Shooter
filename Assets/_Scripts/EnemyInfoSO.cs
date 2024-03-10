using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "MySOs/EnemyInfo")]
[InlineEditor]
public class EnemyInfoSO : ScriptableObject
{    
    [BoxGroup("Basics"), HorizontalGroup("Basics/G", .2f), PreviewField(85, Alignment = ObjectFieldAlignment.Left), HideLabel()]
	[SerializeField] GameObject enemy;

	[BoxGroup("Basics"), VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#ff5959")]
	public int MaxHP;	
	[BoxGroup("Basics")][VerticalGroup("Basics/G/1"), LabelWidth(100), Space, GUIColor("#8559ff")]
	public float Speed;
    [BoxGroup("Basics")][VerticalGroup("Basics/G/2"), LabelWidth(100), Space(30), GUIColor("#8559ff"), Tooltip("in %")]    
	public float SpeedVarPerc = 15;	
	[BoxGroup("Basics")][VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#efff85")]
	public int CollisionDamage;
	[BoxGroup("Basics")][VerticalGroup("Basics/G/2"), LabelWidth(100), GUIColor("#efff85")]
	public float ImpactVelocity;

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

    //Shooting

    [BoxGroup("Shooting"), HorizontalGroup("Shooting/G"), LabelWidth(90), GUIColor("#ffb0b0")]
    public float ShootCD;
    [Tooltip("in Seconds"), HorizontalGroup("Shooting/G"), LabelWidth(90), GUIColor("#ffb0b0")]
    public float ShootCdVar;

    // PowerUps

    [BoxGroup("PowerUps"), LabelWidth(100), GUIColor("#d5b0ff")]
	[Range(0,100)] public float PuDropChance;
    [BoxGroup("PowerUps"), GUIColor("#d5b0ff")]
    public List<PowerUpDrops> PuDrops;

    // EnemyShip

    [FoldoutGroup("EnemyShip"), HorizontalGroup("EnemyShip/G"), LabelWidth(50)]
	public float XSpeed;
	[HorizontalGroup("EnemyShip/G"), LabelWidth(100)]
	public float RotChangeTime;
	[HorizontalGroup("EnemyShip/G"), LabelWidth(80)]
	public float RotTimeVar;
    [HorizontalGroup("EnemyShip/D"), LabelWidth(80)]
    public float DroneSpawnTime;
    [HorizontalGroup("EnemyShip/D"), LabelWidth(80)]
    public float DroneSpawnTimeVar;

    // Sentinel

    [FoldoutGroup("Sentinel"), HorizontalGroup("Sentinel/G", 0.25f), LabelWidth(75)]
	public float SentRange;
	[HorizontalGroup("Sentinel/G"), LabelWidth(80)]
	public int SentDamage;
	[HorizontalGroup("Sentinel/G", 0.45f), LabelWidth(125)]
	public float SentDamageInterval;

    [HorizontalGroup("Top", .7f), PropertyOrder(-1), LabelWidth(40)]
    public string Name;

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

    [Button("SavePrefab",ButtonSizes.Medium,  ButtonAlignment = 1, Stretch = false), PropertyOrder(-1), GUIColor("Green"), HorizontalGroup("Top")]
    public void UpdateValues()
    {
        ConvertDropChance();
        ConvertDropGuaranteed();

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

        if (enemy.TryGetComponent(out SentinelAttack sentAttack))
        {
            sentAttack.Range = SentRange;
            sentAttack.Damage = SentDamage;
            sentAttack.DamageInterval = SentDamageInterval;
        }

        if(enemy.TryGetComponent(out SpawnDroneFromShip spawnDrone))
        {
            spawnDrone.BaseSpawnCD = DroneSpawnTime;
            spawnDrone.SpawnCDVariation = DroneSpawnTimeVar;
        }

        PrefabUtility.SavePrefabAsset(enemy, out bool success);
        Debug.Log(success ? $"Saved {name}" : "Could not save asset");
    }
}