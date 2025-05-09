using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



public class PlayerStats : MonoBehaviour
{
    [FoldoutGroup("scripts")]
    [SerializeField] PlayerUpgradesManager upgradesManager;
    [FoldoutGroup("scripts")]
    [SerializeField] BonusPowersDealer bonusPowers;
    [FoldoutGroup("scripts")]
    [SerializeField] PlayerHeal playerHeal;

    public ShipStats Ship;
    public LasersStats Lasers;
    public IonStreamStats IonStream;
    public DronesStats Drones;
    public ShieldsStats Shields;
    public BombStats Bomb;

    #region classes
    [Serializable]
    public class LasersStats
    {
        public LaserStats FrontLaser;
        public LaserStats SpreadLaser;
        public LaserStats SideLaser;
        public LaserStats BackLaser;
        [Space]
        public TotalUpgrades PowerUpgrades;
        public TotalUpgrades CadencyUpgrades;
        public TotalUpgrades Upgrades;
        
    }
    [Serializable]
    public class LaserStats
    {
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Unlocked;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool ForceDisable;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Enabled;
        [BoxGroup("power")]
        [HorizontalGroup("power/power", LabelWidth = 130)]
        [ReadOnly] public int DefaultPower;
        [HorizontalGroup("power/power", LabelWidth = 130)]
        [ReadOnly] public int CurrentPower;
        //[HorizontalGroup("power/power2", LabelWidth = 130)]
        //[ReadOnly] public int PowerUpgradeLevel;
        [HorizontalGroup("power/power2", LabelWidth = 130)]
        [ReadOnly] public bool IsPowerBonus;
        [BoxGroup("power")]
        public TotalUpgrades PowerUpgrades;
        [BoxGroup("Cadency")]
        [HorizontalGroup("Cadency/Interval", LabelWidth = 130)]
        [ReadOnly] public float DefaultInterval;
        [HorizontalGroup("Cadency/Interval", LabelWidth = 130)]
        [ReadOnly] public float CurrentInterval;
        [HorizontalGroup("Cadency/Interval2", LabelWidth = 130)]
        [ReadOnly] public int CadencyUpgradeLevel;
        [HorizontalGroup("Cadency/Interval2", LabelWidth = 130)]
        [ReadOnly] public bool IsCadencyBonus;
        [BoxGroup("Cadency")]
        public TotalUpgrades CadencyUpgrades;
        [Space]
        public TotalUpgrades TotalUpgrades;
    }

    [Serializable]
    public class IonStreamStats
    {
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Unlocked;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool ForceDisable;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Enabled;
        [BoxGroup("power")]
        [HorizontalGroup("power/power", LabelWidth = 130)]
        [ReadOnly] public int DefaultPower;
        [HorizontalGroup("power/power", LabelWidth = 130)]
        [ReadOnly] public int CurrentPower;
        //[HorizontalGroup("power/power2", LabelWidth = 130)]
        //[ReadOnly] public int PowerUpgradeLevel;
        [HorizontalGroup("power/power2", LabelWidth = 130)]
        [ReadOnly] public bool IsPowerBonus;
        [BoxGroup("power")]
        public TotalUpgrades PowerUpgrades;
        [BoxGroup("Cadency")]
        [HorizontalGroup("Cadency/Interval", LabelWidth = 130)]
        [ReadOnly] public float DefaultInterval;
        [HorizontalGroup("Cadency/Interval", LabelWidth = 130)]
        [ReadOnly] public float CurrentInterval;
        //[HorizontalGroup("Cadency/Interval2", LabelWidth = 130)]
        //[ReadOnly] public int CadencyUpgradeLevel;
        [HorizontalGroup("Cadency/Interval2", LabelWidth = 130)]
        [ReadOnly] public bool IsCadencyBonus;
        [BoxGroup("Cadency")]
        public TotalUpgrades CadencyUpgrades;
        [BoxGroup("Range")]
        [HorizontalGroup("Range/Range", LabelWidth = 130)]
        [ReadOnly] public float DefaultPlayerRange;
        [HorizontalGroup("Range/Range", LabelWidth = 130)]
        [ReadOnly] public float CurrentPlayerRange;
        [HorizontalGroup("Range/RangeHit", LabelWidth = 130)]
        [ReadOnly] public float DefaultHitRange;
        [HorizontalGroup("Range/RangeHit", LabelWidth = 130)]
        [ReadOnly] public float CurrentHitRange;
        //[HorizontalGroup("Range/Range3", LabelWidth = 130)]
        //[ReadOnly] public int RangeUpgradeLevel;
        [HorizontalGroup("Range/Range3", LabelWidth = 130)]
        [ReadOnly] public bool IsRangeBonus;
        [BoxGroup("Range")]
        public TotalUpgrades RangeUpgrades;
        [BoxGroup("Hits")]
        [HorizontalGroup("Hits/Hit", LabelWidth = 130)]
        [ReadOnly] public int DefaultHitNumber;
        [HorizontalGroup("Hits/Hit", LabelWidth = 130)]
        [ReadOnly] public int CurrentHitNumber;
        //[HorizontalGroup("Hits/Hit2", LabelWidth = 130)]
        //[ReadOnly] public int HitNumberUpgradeLevel;
        [HorizontalGroup("Hits/Hit2", LabelWidth = 130)]
        [ReadOnly] public bool IsHitNumberBonus;
        [BoxGroup("Hits")]
        public TotalUpgrades HitNumberUpgrades;
        [Space]
        public TotalUpgrades Upgrades;
    }
    [Serializable]
    public class DronesStats
    {
        [BoxGroup("Heal")]
        [HorizontalGroup("Heal/Heal", LabelWidth = 130)]
        [ReadOnly] public float DefaultHealInterval;
        [HorizontalGroup("Heal/Heal", LabelWidth = 130)]
        [ReadOnly] public float CurrentHealInterval;
        [HorizontalGroup("Heal/Heal2", LabelWidth = 130)]
        [ReadOnly] public float DronesHealIntervalSubtraction;
        [HorizontalGroup("Heal/Heal2", LabelWidth = 130)]
        [ReadOnly] public bool IsHealBonus;
        public DroneStats Drone1;
        public DroneStats Drone2;
        public DroneStats Drone3;        
        [Space]
        public TotalUpgrades PowerUpgrades;
        public TotalUpgrades RangeUpgrades;
        public TotalUpgrades HealUpgrades;
        public TotalUpgrades Upgrades;
    }
    [Serializable]
    public class DroneStats
    {
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Unlocked;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool ForceDisable;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Enabled;
        [BoxGroup("power")]
        [HorizontalGroup("power/power", LabelWidth = 130)]
        [ReadOnly] public int DefaultPower;
        [HorizontalGroup("power/power", LabelWidth = 130)]
        [ReadOnly] public int CurrentPower;
        [HorizontalGroup("power/power2", LabelWidth = 130)]
        [ReadOnly] public bool IsPowerBonus;
        [BoxGroup("power")]
        public TotalUpgrades PowerUpgrades;
        [BoxGroup("Range")]
        [HorizontalGroup("Range/Range", LabelWidth = 130)]
        [ReadOnly] public float DefaultRange;
        [HorizontalGroup("Range/Range", LabelWidth = 130)]
        [ReadOnly] public float CurrentRange;
        [HorizontalGroup("Range/Range2", LabelWidth = 130)]
        [ReadOnly] public bool IsRangeBonus;
        [BoxGroup("Range")]
        public TotalUpgrades RangeUpgrades;
        [BoxGroup("HealSubtraction")]
        [HorizontalGroup("HealSubtraction/Heal", LabelWidth = 150)]
        [ReadOnly] public float HealIntervalSubtraction;
        [BoxGroup("HealSubtraction")]
        public TotalUpgrades HealUpgrades;
        
        [Space]        
        public TotalUpgrades TotalUpgrades;
    }
    [Serializable]
    public class ShieldsStats
    {
        public ShieldStats ShieldFront;
        public ShieldStats ShieldRight;
        public ShieldStats ShieldLeft;
        public ShieldStats ShieldBack;
        public TotalUpgrades StrenghtUpgrades;
        public TotalUpgrades RecoveryUpgrades;
        public TotalUpgrades Upgrades;
    }
    [Serializable]
    public class ShieldStats
    {
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Unlocked;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool ForceDisable;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Enabled;
        [BoxGroup("Strenght")]
        [HorizontalGroup("Strenght/Strenght", LabelWidth = 130)]
        [ReadOnly] public int DefaultMaxStrenght;
        [HorizontalGroup("Strenght/Strenght", LabelWidth = 130)]
        [ReadOnly] public int CurrentMaxStrenght;
        [HorizontalGroup("Strenght/Strenght2", LabelWidth = 130)]
        [ReadOnly] public bool IsStrenghtBonus;
        [HorizontalGroup("Strenght/Strenght3", LabelWidth = 130)]
        [ReadOnly] public int CurrentStrenght;
        [BoxGroup("Strenght")]
        public TotalUpgrades StrenghtUpgrades;
        [BoxGroup("Recovery")]
        [HorizontalGroup("Recovery/Interval", LabelWidth = 130)]
        [ReadOnly] public float DefaultInterval;
        [HorizontalGroup("Recovery/Interval", LabelWidth = 130)]
        [ReadOnly] public float CurrentInterval;
        [HorizontalGroup("Recovery/Interval2", LabelWidth = 130)]
        [ReadOnly] public bool IsRecoveryBonus;
        [BoxGroup("Recovery")]
        public TotalUpgrades RecoveryUpgrades;
        [Space]
        public TotalUpgrades TotalUpgrades;
    }
    [Serializable]
    public class ShipStats
    {
        [BoxGroup("HP")]
        [HorizontalGroup("HP/HP", LabelWidth = 130)]
        [ReadOnly] public int DefaultMaxHP;
        [HorizontalGroup("HP/HP", LabelWidth = 130)]
        [ReadOnly] public int CurrentMaxHP;     
        [HorizontalGroup("HP/HP2", LabelWidth = 130)]
        [ReadOnly] public bool IsHpBonus;
        [HorizontalGroup("HP/HP3", LabelWidth = 130)]
        [ReadOnly] public int CurrentHP;
        [BoxGroup("HP")]
        public TotalUpgrades HpUpgrades;
        [BoxGroup("Speed")]
        [HorizontalGroup("Speed/Speed", LabelWidth = 130)]
        [ReadOnly] public float DefaultMaxSpeed;
        [HorizontalGroup("Speed/Speed", LabelWidth = 130)]
        [ReadOnly] public float CurrentMaxSpeed;
        [HorizontalGroup("Speed/Inertia", LabelWidth = 130)]
        [ReadOnly] public float DefaultLinearInertia;
        [HorizontalGroup("Speed/Inertia", LabelWidth = 130)]
        [ReadOnly] public float CurrentLinearInertia;
        [HorizontalGroup("Speed/Speed2", LabelWidth = 130)]
        [ReadOnly] public bool IsSpeedBonus;
        [BoxGroup("Speed")]
        public TotalUpgrades SpeedUpgrades;
        [BoxGroup("Turning")]
        [HorizontalGroup("Turning/Turning", LabelWidth = 130)]
        [ReadOnly] public float DefaultMaxTurningSpeed;
        [HorizontalGroup("Turning/Turning", LabelWidth = 130)]
        [ReadOnly] public float CurrentMaxTurningSpeed;
        [HorizontalGroup("Turning/Inertia", LabelWidth = 130)]
        [ReadOnly] public float DefaultAngularInertia;
        [HorizontalGroup("Turning/Inertia", LabelWidth = 130)]
        [ReadOnly] public float CurrentAngularInertia;
        [HorizontalGroup("Turning/Turning2", LabelWidth = 130)]
        [ReadOnly] public bool IsTurningSpeedBonus;
        [BoxGroup("Turning")]
        public TotalUpgrades ManobrabilityUpgrades;
        [BoxGroup("Tractor")]
        [ReadOnly] public TractorStats Tractor;
        [BoxGroup("Tractor")]
        public TotalUpgrades TractorUpgrades;
        [Space]
        public TotalUpgrades Upgrades;
    }
    [Serializable]
    public class TractorStats
    {
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Unlocked;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool ForceDisable;
        [HorizontalGroup("enabled", LabelWidth = 90)]
        [ReadOnly] public bool Enabled;
        [BoxGroup("Range")]
        [HorizontalGroup("Range/Range", LabelWidth = 130)]
        [ReadOnly] public float DefaultRange;
        [HorizontalGroup("Range/Range", LabelWidth = 130)]
        [ReadOnly] public float CurrentRange;
        [HorizontalGroup("Range/Range2", LabelWidth = 130)]
        [ReadOnly] public bool IsRangeBonus;
        [BoxGroup("Force")]
        [HorizontalGroup("Force/Force", LabelWidth = 130)]
        [ReadOnly] public float DefaultForce;
        [HorizontalGroup("Force/Force", LabelWidth = 130)]
        [ReadOnly] public float CurrentForce;
        [HorizontalGroup("Force/Force2", LabelWidth = 130)]
        [ReadOnly] public bool IsForceBonus;
    }
    [Serializable]
    public class BombStats
    {
        [HorizontalGroup("power", LabelWidth = 130)]
        [ReadOnly] public int DefaultPower;
        [HorizontalGroup("power", LabelWidth = 130)]
        [ReadOnly] public int CurrentPower;
        [HorizontalGroup("PowerBonus", LabelWidth = 130)]
        [ReadOnly] public bool IsPowerBonus;
        [HorizontalGroup("Range", LabelWidth = 130)]
        [ReadOnly] public float DefaultRange;
        [HorizontalGroup("Range", LabelWidth = 130)]
        [ReadOnly] public float CurrentRange;
        [HideInInspector] public float RangeModifier;
        [HorizontalGroup("RangeBonus", LabelWidth = 130)]
        [ReadOnly] public bool IsRangeBonus;
        [HorizontalGroup("Charges", LabelWidth = 130)]
        [ReadOnly] public int Charges;
        [HorizontalGroup("Generation", LabelWidth = 130)]
        [ReadOnly] public bool IsGenerationBonus;
        [HorizontalGroup("Generation", LabelWidth = 130)]
        [ReadOnly] public float GenerationInterval;
    }
    [Serializable]
    public class TotalUpgrades
    {
        [HorizontalGroup("Total", LabelWidth = 130)]
        [ReadOnly] public int Upgrades;
        [HorizontalGroup("Total", LabelWidth = 130)]
        [ReadOnly] public int UpgradesPerc;
        [HideInInspector] public int NumberOfUpgrades;
    }
    #endregion

    public static PlayerStats Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        UpdateLasersStats();
        UpdateIonStreamStats();
        UpdateDronesStats();
        UpdateShieldsStats();
        UpdateBombStats();
        UpdateShipStats();
    }

    void UpdateLasersStats()
    {
        UpdateLaserStats(Lasers.FrontLaser, upgradesManager.CurrentUpgrades.FrontLaserUpgrades);
        UpdateLaserStats(Lasers.SpreadLaser, upgradesManager.CurrentUpgrades.SpreadLaserUpgrades);
        UpdateLaserStats(Lasers.SideLaser, upgradesManager.CurrentUpgrades.SideLaserUpgrades);
        UpdateLaserStats(Lasers.BackLaser, upgradesManager.CurrentUpgrades.BackLaserUpgrades);

        Lasers.Upgrades = SumTotalUpgrades(Lasers.FrontLaser.TotalUpgrades,
            Lasers.SpreadLaser.TotalUpgrades, Lasers.SideLaser.TotalUpgrades, Lasers.BackLaser.TotalUpgrades);
        Lasers.PowerUpgrades = SumTotalUpgrades(Lasers.FrontLaser.PowerUpgrades,
            Lasers.SpreadLaser.PowerUpgrades, Lasers.SideLaser.PowerUpgrades, Lasers.BackLaser.PowerUpgrades);
        Lasers.CadencyUpgrades = SumTotalUpgrades(Lasers.FrontLaser.CadencyUpgrades,
            Lasers.SpreadLaser.CadencyUpgrades, Lasers.SideLaser.CadencyUpgrades, Lasers.BackLaser.CadencyUpgrades);

        void UpdateLaserStats(LaserStats stats, LaserUpgrades upgrades)
        {
            // Enabled
            stats.Unlocked = upgrades.Enabled;
            stats.ForceDisable = upgrades.DisableOverwrite;
            stats.Enabled = stats.Unlocked && !stats.ForceDisable;
            // Upgrades
            UpdadeUpgrades(stats.PowerUpgrades, upgrades.DamageLevel, upgradesManager.LaserUpgradesInfo.PowerUpgrades.Length, stats.Unlocked);
            UpdadeUpgrades(stats.CadencyUpgrades, upgrades.CadencyLevel, upgradesManager.LaserUpgradesInfo.CadencyUpgrades.Length, stats.Unlocked);
            stats.TotalUpgrades = SumTotalUpgrades(stats.PowerUpgrades, stats.CadencyUpgrades);
            // Power
            stats.DefaultPower = upgradesManager.LaserUpgradesInfo.PowerUpgrades[upgrades.DamageLevel - 1].Damage;
            stats.CurrentPower = stats.DefaultPower;
            if (GameManager.IsSurvival)
                stats.CurrentPower += bonusPowers.LaserPower;

            if(GameManager.IsSurvival && bonusPowers.LaserPower > 0)
                stats.IsPowerBonus = true;
            else
                stats.IsPowerBonus = false;
            // Cadency
            stats.DefaultInterval = upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgrades.CadencyLevel-1].TimeBetween;
            stats.CurrentInterval = stats.DefaultInterval;
            if (GameManager.IsSurvival)
            {
                float bonusMultiplier = 1 - bonusPowers.LaserIonStreamCadency/100f;
                stats.CurrentInterval *= bonusMultiplier;

                if (bonusPowers.IsMoreLaserCadency)
                {
                    bonusMultiplier = 1 - BonusPowersDealer.Instance.MoreLaserCadencyPerc/100f;
                    stats.CurrentInterval *= bonusMultiplier;
                }
            }

            if (GameManager.IsSurvival && (bonusPowers.LaserIonStreamCadency > 0 || bonusPowers.IsMoreLaserCadency))
                stats.IsCadencyBonus = true;
            else
                stats.IsCadencyBonus = false;
        }
    }

    void UpdateShieldsStats()
    {
        UpdateShieldsStats(Shields.ShieldFront, upgradesManager.CurrentUpgrades.FrontShieldUpgrades);
        UpdateShieldsStats(Shields.ShieldRight, upgradesManager.CurrentUpgrades.RightShieldUpgrades);
        UpdateShieldsStats(Shields.ShieldLeft, upgradesManager.CurrentUpgrades.LeftShieldUpgrades);
        UpdateShieldsStats(Shields.ShieldBack, upgradesManager.CurrentUpgrades.BackShieldUpgrades);

        Shields.Upgrades = SumTotalUpgrades(Shields.ShieldFront.TotalUpgrades, Shields.ShieldRight.TotalUpgrades,
            Shields.ShieldLeft.TotalUpgrades, Shields.ShieldBack.TotalUpgrades);
        Shields.StrenghtUpgrades = SumTotalUpgrades(Shields.ShieldFront.StrenghtUpgrades, Shields.ShieldRight.StrenghtUpgrades,
            Shields.ShieldLeft.StrenghtUpgrades, Shields.ShieldBack.StrenghtUpgrades);
        Shields.RecoveryUpgrades = SumTotalUpgrades(Shields.ShieldFront.RecoveryUpgrades, Shields.ShieldRight.RecoveryUpgrades,
            Shields.ShieldLeft.RecoveryUpgrades, Shields.ShieldBack.RecoveryUpgrades);

        void UpdateShieldsStats(ShieldStats stats, ShieldUpgrades upgrades)
        {
            // Enabled
            stats.Unlocked = upgrades.Enabled;
            stats.ForceDisable = upgrades.DisableOverwrite;
            stats.Enabled = stats.Unlocked && !stats.ForceDisable;
            // Upgrades
            UpdadeUpgrades(stats.StrenghtUpgrades, upgrades.ResistenceLevel, upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades.Length, stats.Unlocked);
            UpdadeUpgrades(stats.RecoveryUpgrades, upgrades.RecoveryLevel, upgradesManager.ShieldUpgradesInfo.RecoveryUpgrades.Length, stats.Unlocked);
            stats.TotalUpgrades = SumTotalUpgrades(stats.StrenghtUpgrades, stats.RecoveryUpgrades);
            // Strenght
            stats.DefaultMaxStrenght = upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[upgrades.ResistenceLevel - 1].Strenght;
            stats.CurrentMaxStrenght = stats.DefaultMaxStrenght;
            if (GameManager.IsSurvival)
                stats.CurrentMaxStrenght += bonusPowers.ShieldStrenght;

            if (GameManager.IsSurvival && bonusPowers.ShieldStrenght > 0)
                stats.IsStrenghtBonus = true;
            else stats.IsStrenghtBonus = false;
            // Recovery
            stats.DefaultInterval = upgradesManager.ShieldUpgradesInfo.RecoveryUpgrades[upgrades.RecoveryLevel-1].TimeBetween;
            stats.CurrentInterval = stats.DefaultInterval;
            if (GameManager.IsSurvival)
            {
                float bonusMultiplier = 1 - bonusPowers.ShieldRecovery/100f;
                stats.CurrentInterval *= bonusMultiplier;

                if (bonusPowers.ShieldRecovery > 0)
                    stats.IsRecoveryBonus = true;
                else stats.IsRecoveryBonus = false;
            }           
        }
    }

    void UpdateBombStats()
    {
        Bomb.DefaultPower = BombScript.BaseDamage;
        Bomb.CurrentPower = Bomb.DefaultPower;
        Bomb.DefaultRange = BombScript.BaseRange;
        Bomb.CurrentRange = Bomb.DefaultRange;
        Bomb.Charges = BombScript.BombAmount;

        if (GameManager.IsSurvival)
        {
            Bomb.GenerationInterval = BonusPowersDealer.Instance.BombGeneration;
            Bomb.IsGenerationBonus = Bomb.GenerationInterval > 0;
            //Power
            Bomb.CurrentPower += BonusPowersDealer.Instance.BombPower;
            if(BonusPowersDealer.Instance.IsSuperBomb)
                Bomb.CurrentPower += BonusPowersDealer.Instance.SuperBombExtraDamage;
            Bomb.IsPowerBonus = BonusPowersDealer.Instance.BombPower > 0 || BonusPowersDealer.Instance.IsSuperBomb;
            //Range
            float bonusMult = 1 + BonusPowersDealer.Instance.DroneIonStreamBombRange/100f;
            Bomb.CurrentRange *= bonusMult;
            if (BonusPowersDealer.Instance.IsSuperBomb)
            {
                bonusMult = 1 + BonusPowersDealer.Instance.SuperBombExtraRange/100f;
                Bomb.CurrentRange *= bonusMult;
            }
            Bomb.IsRangeBonus = BonusPowersDealer.Instance.DroneIonStreamBombRange > 0 || BonusPowersDealer.Instance.IsSuperBomb;
            Bomb.RangeModifier = Bomb.CurrentRange/Bomb.DefaultRange;
        }
    }

    void UpdateDronesStats()
    {
        UpdateDroneStats(Drones.Drone1, upgradesManager.CurrentUpgrades.Drone_1_Upgrades);
        UpdateDroneStats(Drones.Drone2, upgradesManager.CurrentUpgrades.Drone_2_Upgrades);
        UpdateDroneStats(Drones.Drone3, upgradesManager.CurrentUpgrades.Drone_3_Upgrades);

        Drones.Upgrades = SumTotalUpgrades(Drones.Drone1.TotalUpgrades, Drones.Drone2.TotalUpgrades, Drones.Drone3.TotalUpgrades);
        Drones.PowerUpgrades = SumTotalUpgrades(Drones.Drone1.PowerUpgrades, Drones.Drone2.PowerUpgrades, Drones.Drone3.PowerUpgrades);
        Drones.RangeUpgrades = SumTotalUpgrades(Drones.Drone1.RangeUpgrades, Drones.Drone2.RangeUpgrades, Drones.Drone3.RangeUpgrades);
        Drones.HealUpgrades = SumTotalUpgrades(Drones.Drone1.HealUpgrades, Drones.Drone2.HealUpgrades, Drones.Drone3.HealUpgrades);

        Drones.DronesHealIntervalSubtraction = 0;
        if (Drones.Drone1.Enabled) Drones.DronesHealIntervalSubtraction += Drones.Drone1.HealIntervalSubtraction;
        if (Drones.Drone2.Enabled) Drones.DronesHealIntervalSubtraction += Drones.Drone2.HealIntervalSubtraction;
        if (Drones.Drone3.Enabled) Drones.DronesHealIntervalSubtraction += Drones.Drone3.HealIntervalSubtraction;


        Drones.DefaultHealInterval = playerHeal.BaseSecondsBetweenHeal - Drones.DronesHealIntervalSubtraction;
        Drones.CurrentHealInterval = Drones.DefaultHealInterval;
        if (GameManager.IsSurvival)
        {
            bool allDronesEnabled = Drones.Drone1.Enabled && Drones.Drone2.Enabled && Drones.Drone3.Enabled;
            if (BonusPowersDealer.Instance.IsFourthDrone && allDronesEnabled)
            {
                float minHealIntervalReduction = Mathf.Min(Drones.Drone1.HealIntervalSubtraction, Drones.Drone2.HealIntervalSubtraction, Drones.Drone3.HealIntervalSubtraction);
                Drones.CurrentHealInterval -= minHealIntervalReduction;
            }

            float bonusMultiplier = 1 - BonusPowersDealer.Instance.HP_Recovery/100f;
            Drones.CurrentHealInterval *= bonusMultiplier;

            if(BonusPowersDealer.Instance.HP_Recovery > 0 || (BonusPowersDealer.Instance.IsFourthDrone && allDronesEnabled))
                Drones.IsHealBonus = true;
            else Drones.IsHealBonus = false;
        }

        void UpdateDroneStats(DroneStats stats, DronesUpgrades upgrades)
        {
            // Enabled
            stats.Unlocked = upgrades.Enabled;
            stats.ForceDisable = upgrades.DisableOverwrite;
            stats.Enabled = stats.Unlocked && !stats.ForceDisable;
            // Upgrades
            UpdadeUpgrades(stats.PowerUpgrades, upgrades.DamageLevel, upgradesManager.DroneUpgradesInfo.PowerUpgrades.Length, stats.Unlocked);
            UpdadeUpgrades(stats.RangeUpgrades, upgrades.RangeLevel, upgradesManager.DroneUpgradesInfo.RangeUpgrades.Length, stats.Unlocked);
            UpdadeUpgrades(stats.HealUpgrades, upgrades.HealingLevel, upgradesManager.DroneUpgradesInfo.HealUpgrades.Length, stats.Unlocked);
            stats.TotalUpgrades = SumTotalUpgrades(stats.PowerUpgrades, stats.RangeUpgrades, stats.HealUpgrades);
            // Power
            stats.DefaultPower = upgradesManager.DroneUpgradesInfo.PowerUpgrades[upgrades.DamageLevel - 1].DamagePerSecond;
            stats.CurrentPower = stats.DefaultPower;
            if (GameManager.IsSurvival)
                stats.CurrentPower += bonusPowers.DronePower;

            if (GameManager.IsSurvival && bonusPowers.DronePower > 0)
                stats.IsPowerBonus = true;
            else stats.IsPowerBonus = false;
            // Range
            stats.DefaultRange = upgradesManager.DroneUpgradesInfo.RangeUpgrades[upgrades.RangeLevel-1].Range;
            stats.CurrentRange = stats.DefaultRange;
            if (GameManager.IsSurvival)
            {
                float bonusMultiplier = 1 + BonusPowersDealer.Instance.DroneIonStreamBombRange/100f;
                stats.CurrentRange *= bonusMultiplier;
            }

            if (GameManager.IsSurvival && bonusPowers.DroneIonStreamBombRange > 0)
                stats.IsRangeBonus = true;
            else stats.IsRangeBonus = false;
            //Heal
            stats.HealIntervalSubtraction = upgradesManager.DroneUpgradesInfo.HealUpgrades[upgrades.HealingLevel-1].ReduceFromHealInterval;
        }
    }

    void UpdateIonStreamStats()
    {
        IonStreamUpgrades upgrades = upgradesManager.CurrentUpgrades.IonStreamUpgrades;
        // Enabled
        IonStream.Unlocked = upgrades.Enabled;
        IonStream.ForceDisable = upgrades.DisableOverwrite;
        IonStream.Enabled = IonStream.Unlocked && !IonStream.ForceDisable;
        // Upgrades
        UpdadeUpgrades(IonStream.PowerUpgrades, upgrades.DamageLevel, upgradesManager.IonStreamUpgradesInfo.PowerUpgrades.Length, IonStream.Unlocked);
        UpdadeUpgrades(IonStream.CadencyUpgrades, upgrades.CadencyLevel, upgradesManager.IonStreamUpgradesInfo.CadencyUpgrades.Length, IonStream.Unlocked);
        UpdadeUpgrades(IonStream.RangeUpgrades, upgrades.RangeLevel, upgradesManager.IonStreamUpgradesInfo.RangeUpgrades.Length, IonStream.Unlocked);
        UpdadeUpgrades(IonStream.HitNumberUpgrades, upgrades.NumberHitsLevel, upgradesManager.IonStreamUpgradesInfo.HitNumUpgrades.Length, IonStream.Unlocked);
        IonStream.Upgrades = SumTotalUpgrades(IonStream.PowerUpgrades, IonStream.CadencyUpgrades, IonStream.RangeUpgrades, IonStream.HitNumberUpgrades);
        // Power
        IonStream.DefaultPower = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[upgradesManager.CurrentUpgrades.IonStreamUpgrades.DamageLevel - 1].Damage;
        IonStream.CurrentPower = IonStream.DefaultPower;
        if (GameManager.IsSurvival)
            IonStream.CurrentPower += BonusPowersDealer.Instance.IonStreamPower;

        if (GameManager.IsSurvival && bonusPowers.IonStreamPower > 0)
            IonStream.IsPowerBonus = true;
        else IonStream.IsPowerBonus = false;
        // Cadency
        IonStream.DefaultInterval = upgradesManager.IonStreamUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.IonStreamUpgrades.CadencyLevel - 1].TimeBetween;
        IonStream.CurrentInterval = IonStream.DefaultInterval;
        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 - BonusPowersDealer.Instance.LaserIonStreamCadency/100f;
            IonStream.CurrentInterval *= bonusMultiplier;
        }

        if (GameManager.IsSurvival && bonusPowers.LaserIonStreamCadency > 0)
            IonStream.IsCadencyBonus = true;
        else IonStream.IsCadencyBonus = false;
        // Range
        IonStream.DefaultPlayerRange = upgradesManager.IonStreamUpgradesInfo.RangeUpgrades[upgradesManager.CurrentUpgrades.IonStreamUpgrades.RangeLevel - 1].RangeFromPlayer;
        IonStream.CurrentPlayerRange = IonStream.DefaultPlayerRange;
        IonStream.DefaultHitRange = upgradesManager.IonStreamUpgradesInfo.RangeUpgrades[upgradesManager.CurrentUpgrades.IonStreamUpgrades.RangeLevel - 1].RangeFromHit;
        IonStream.CurrentHitRange = IonStream.DefaultHitRange;
        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 + BonusPowersDealer.Instance.DroneIonStreamBombRange/100f;
            IonStream.CurrentPlayerRange *= bonusMultiplier;
            IonStream.CurrentHitRange *= bonusMultiplier;
        }

        if (GameManager.IsSurvival && bonusPowers.DroneIonStreamBombRange > 0)
            IonStream.IsRangeBonus = true;
        else IonStream.IsRangeBonus = false;
        // Range        
        IonStream.DefaultHitNumber = upgradesManager.IonStreamUpgradesInfo.HitNumUpgrades[upgradesManager.CurrentUpgrades.IonStreamUpgrades.NumberHitsLevel - 1].NumberOfHits;
        IonStream.CurrentHitNumber = IonStream.DefaultHitNumber;
    }

    void UpdateShipStats()
    {
        ShipUpgrades upgrades = upgradesManager.CurrentUpgrades.ShipUpgrades;
        // Upgrades
        UpdadeUpgrades(Ship.HpUpgrades, upgrades.HPLevel, upgradesManager.ShipUpgradesInfo.HP_Upgrade.Length);
        UpdadeUpgrades(Ship.SpeedUpgrades, upgrades.SpeedLevel, upgradesManager.ShipUpgradesInfo.SpeedUpgrade.Length);
        UpdadeUpgrades(Ship.ManobrabilityUpgrades, upgrades.ManobrabilityLevel, upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade.Length);
        UpdadeUpgrades(Ship.TractorUpgrades, upgrades.TractorBeamLevel, upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade.Length, upgrades.TractorBeamEnabled);
        Ship.Upgrades = SumTotalUpgrades(Ship.HpUpgrades, Ship.SpeedUpgrades, Ship.ManobrabilityUpgrades, Ship.TractorUpgrades);
        // HP
        Ship.DefaultMaxHP = upgradesManager.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;
        Ship.CurrentMaxHP = Ship.DefaultMaxHP;
        // Speed
        Ship.DefaultMaxSpeed = upgradesManager.ShipUpgradesInfo.SpeedUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1].Speed;
        Ship.CurrentMaxSpeed = Ship.DefaultMaxSpeed;
        Ship.DefaultLinearInertia = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStop;
        Ship.CurrentLinearInertia = Ship.DefaultLinearInertia;
        if (GameManager.IsSurvival)
        {
            float bonusMult = 1 + BonusPowersDealer.Instance.Mobility/100f;
            Ship.CurrentMaxSpeed *= bonusMult;
            bonusMult = 1 - BonusPowersDealer.Instance.Mobility/200f;
            Ship.CurrentLinearInertia *= bonusMult;

            Ship.IsSpeedBonus = BonusPowersDealer.Instance.Mobility > 0;
        }
        // Manobrability
        Ship.DefaultMaxTurningSpeed = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TurningSpeed;
        Ship.CurrentMaxTurningSpeed = Ship.DefaultMaxTurningSpeed;
        Ship.DefaultAngularInertia = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStopRotating;
        Ship.CurrentAngularInertia = Ship.DefaultAngularInertia;
        if (GameManager.IsSurvival)
        {
            float bonusMult = 1 + BonusPowersDealer.Instance.Mobility/100f;
            Ship.CurrentMaxTurningSpeed *= bonusMult;
            bonusMult = 1 - BonusPowersDealer.Instance.Mobility/200f;
            Ship.CurrentAngularInertia *= bonusMult;

            Ship.IsTurningSpeedBonus = BonusPowersDealer.Instance.Mobility > 0;
        }
        // Tractor
        Ship.Tractor.Unlocked = upgrades.TractorBeamEnabled;
        Ship.Tractor.ForceDisable = upgrades.TractorBeamDisableOverwrite;
        Ship.Tractor.Enabled = Ship.Tractor.Unlocked && !Ship.Tractor.ForceDisable;

        Ship.Tractor.DefaultRange = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].RadiusMod;
        Ship.Tractor.CurrentRange = Ship.Tractor.DefaultRange;
        Ship.Tractor.DefaultForce = upgradesManager.ShipUpgradesInfo.TractorBeamUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].PullForce; ;
        Ship.Tractor.CurrentForce = Ship.Tractor.DefaultForce;

        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 + BonusPowersDealer.Instance.Tractor/100f;
            Ship.Tractor.CurrentRange *= bonusMultiplier;
            Ship.Tractor.CurrentForce *= bonusMultiplier;

            Ship.Tractor.IsRangeBonus = BonusPowersDealer.Instance.Tractor > 0;
            Ship.Tractor.IsForceBonus = BonusPowersDealer.Instance.Tractor > 0;
        }
    }

    void UpdadeUpgrades(TotalUpgrades upgrades, int currentUpgrades, int totalUpgrades, bool isUnlocked = true)
    {        
        upgrades.NumberOfUpgrades = totalUpgrades;
        upgrades.Upgrades = currentUpgrades;
        if (!isUnlocked) upgrades.Upgrades = 0;
        upgrades.UpgradesPerc = (int)Mathf.Ceil((float)upgrades.Upgrades/upgrades.NumberOfUpgrades*100);
    }

    TotalUpgrades SumTotalUpgrades (params TotalUpgrades[] upgrades)
    {
        TotalUpgrades total = new TotalUpgrades();
        for (int i = 0; i < upgrades.Length; i++)
        {
            total.NumberOfUpgrades += upgrades[i].NumberOfUpgrades;
            total.Upgrades += upgrades[i].Upgrades;
        }
        total.UpgradesPerc = (int)Mathf.Ceil((float)total.Upgrades/total.NumberOfUpgrades*100);
        return total;
    }

}
