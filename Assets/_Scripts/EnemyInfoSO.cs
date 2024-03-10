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
	[SerializeField] public int MaxHP;	
	[BoxGroup("Basics")][VerticalGroup("Basics/G/1"), LabelWidth(100), Space, GUIColor("#8559ff")]
	[SerializeField] public float Speed;
    [BoxGroup("Basics")][VerticalGroup("Basics/G/2"), LabelWidth(100), Space(30), GUIColor("#8559ff"), Tooltip("in %")]    
	[SerializeField] public float SpeedVarPerc = 15;	
	[BoxGroup("Basics")][VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#efff85")]
	[SerializeField] public int CollisionDamage;
	[BoxGroup("Basics")][VerticalGroup("Basics/G/2"), LabelWidth(100), GUIColor("#efff85")]
	[SerializeField] public float ImpactVelocity;

	[BoxGroup("Drops"), HorizontalGroup("Drops/Drops"), LabelWidth(60), Range(0,10), GUIColor("#b0fff6")]
	[SerializeField] public int MinDrops;
	[HorizontalGroup("Drops/Drops"), LabelWidth(60), Range(0, 10), GUIColor("#b0fff6")]
	[SerializeField] public int MaxDrops;
    [BoxGroup("Drops"), GUIColor("#b0fff6")]
    [SerializeField] public DropsToSpawn[] DropsChances;
    [BoxGroup("Drops"), GUIColor("#78aeff")]
    [SerializeField] public DropsGuaranteed[] DropsGuaranteed;

    [BoxGroup("Shooting"), HorizontalGroup("Shooting/G"), LabelWidth(90), GUIColor("#ffb0b0")]
    [SerializeField] public float ShootCD;
    [Tooltip("in Seconds"), HorizontalGroup("Shooting/G"), LabelWidth(90), GUIColor("#ffb0b0")]
    [SerializeField] public float ShootCdVar;

    [BoxGroup("PowerUps"), LabelWidth(100), GUIColor("#d5b0ff")]
	[SerializeField] public float PuDropChance;
    [BoxGroup("PowerUps"), GUIColor("#d5b0ff")]
    [SerializeField] public List<PowerUpDrops> PuDrops;

    [FoldoutGroup("EnemyShipRotation"), HorizontalGroup("EnemyShipRotation/G"), LabelWidth(50)]
	[SerializeField] public float XSpeed;
	[HorizontalGroup("EnemyShipRotation/G"), LabelWidth(70)]
	[SerializeField] public float RotChangeTime;
	[HorizontalGroup("EnemyShipRotation/G"), LabelWidth(80)]
	[SerializeField] public float RotTimeVar;

    [FoldoutGroup("Sentinel"), HorizontalGroup("Sentinel/G", 0.25f), LabelWidth(50)]
	[SerializeField] public float SentRange;
	[HorizontalGroup("Sentinel/G"), LabelWidth(50)]
	[SerializeField] public int SentDamage;
	[HorizontalGroup("Sentinel/G", 0.45f), LabelWidth(100)]
	[SerializeField] public float SentDamageInterval;

    [Button("SavePrefab",ButtonSizes.Medium,  ButtonAlignment = 1, Stretch = false), PropertyOrder(-1), GUIColor("Green")]
    public void UpdateValues()
    {
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

        PrefabUtility.SavePrefabAsset(enemy, out bool success);
        Debug.Log(success ? $"Saved {name}" : "Could not save asset");
    }
}