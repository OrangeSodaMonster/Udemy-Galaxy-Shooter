using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPowersDealer : MonoBehaviour
{
    #region PowerUpgrades
    [FoldoutGroup("PowerUpgrades")]
    [HorizontalGroup("PowerUpgrades/LaserPower", 0.45f, LabelWidth = 130)]
    [SerializeField] int laserPowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/LaserPower")]
    [SerializeField] int[] laserPowerExtraDamage = new int[] { 2, 4, 6 };
    
    [HorizontalGroup("PowerUpgrades/DronePower", 0.45f, LabelWidth = 130)]
    [SerializeField] int dronePowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/DronePower")]
    [SerializeField] int[] dronePowerExtraDamage = new int[] { 5, 10, 15 };
    
    [HorizontalGroup("PowerUpgrades/IonStreamPower", 0.45f, LabelWidth = 130)]
    [SerializeField] int ionStreamPowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/IonStreamPower")]
    [SerializeField] int[] ionStreamPowerExtraDamage = new int[] { 5, 10, 15 };
    
    [HorizontalGroup("PowerUpgrades/BombPower", 0.45f, LabelWidth = 130)]
    [SerializeField] int bombPowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/BombPower")]
    [SerializeField] int[] bombPowerExtraDamage = new int[] { 50, 100, 150 };
    
    [HorizontalGroup("PowerUpgrades/LaserIonStreamCadency", 0.45f, LabelWidth = 130)]
    [SerializeField] int laserIonStreamCadencyLevel = 0;
    [HorizontalGroup("PowerUpgrades/LaserIonStreamCadency")]
    [SerializeField] int[] laserIonStreamCadencyPerc = new int[] { 5, 10, 15 };
    
    [HorizontalGroup("PowerUpgrades/BombRegeneration", 0.45f, LabelWidth = 130)]
    [SerializeField] int bombRegenerationLevel = 0;
    [HorizontalGroup("PowerUpgrades/BombRegeneration")]
    [SerializeField] int[] bombRegenerationTimer = new int[] { 45, 35, 25 };
    
    [HorizontalGroup("PowerUpgrades/ExtraEnemyDamage", 0.45f, LabelWidth = 130)]
    [SerializeField] int extraDamageToEnemiesLevel = 0;
    [HorizontalGroup("PowerUpgrades/ExtraEnemyDamage")]
    [SerializeField] int[] extraDamageToEnemiesPerc = new int[] { 5, 10, 15 }; // Reduce Enemy HP
    
    [HorizontalGroup("PowerUpgrades/ExtraObjectiveDamage", 0.45f, LabelWidth = 130)]
    [SerializeField] int extraDamageToObjectivesLevel = 0;
    [HorizontalGroup("PowerUpgrades/ExtraObjectiveDamage")]
    [SerializeField] int[] extraDamageToObjectivesPerc = new int[] { 10, 15, 20 }; // Reduce Enemy HP
    #endregion

    #region UtilityUpgrades
    [FoldoutGroup("UtilityUpgrades")]
    [HorizontalGroup("UtilityUpgrades/Tractor", 0.45f, LabelWidth = 130)]
    [SerializeField] int tractorLevel = 0;
    [HorizontalGroup("UtilityUpgrades/Tractor")]
    [SerializeField] int[] tractorExtraPerc = new int[] { 10, 20, 30 }; // Range, pull force
    
    [HorizontalGroup("UtilityUpgrades/DroneIonRange", 0.45f, LabelWidth = 130)]
    [SerializeField] int droneIonStreamRangeLevel = 0;
    [HorizontalGroup("UtilityUpgrades/DroneIonRange")]
    [SerializeField] int[] droneIonStreamRangeExtraPerc = new int[] { 10, 15, 20 };
    
    [HorizontalGroup("UtilityUpgrades/Mobility", 0.45f, LabelWidth = 130)]
    [SerializeField] int mobilityLevel = 0;
    [HorizontalGroup("UtilityUpgrades/Mobility")]
    [SerializeField] int[] mobilityExtraPerc = new int[] { 5, 10, 15 }; // Speed, turn speed
    
    [HorizontalGroup("UtilityUpgrades/PowerUpDrop", 0.45f, LabelWidth = 130)]
    [SerializeField] int powerUpDropLevel = 0;
    [HorizontalGroup("UtilityUpgrades/PowerUpDrop")]
    [SerializeField] int[] powerUpDropExtraPerc = new int[] { 10, 20, 30 }; // Speed, turn speed
    
    [HorizontalGroup("UtilityUpgrades/EnergyCristalDrop", 0.45f, LabelWidth = 130)]
    [SerializeField] int energyCristalsDropLevel = 0;
    [HorizontalGroup("UtilityUpgrades/EnergyCristalDrop")]
    [SerializeField] int[] energyCristalsExtraPerc = new int[] { 10, 20, 30 }; // Chance de dropar um cristal de energia a mais, tipo baseado na chance base blue 5, pink 1 por exemplo
    
    [HorizontalGroup("UtilityUpgrades/AutoConvertion", 0.45f, LabelWidth = 130)]
    [SerializeField] int autoConvertionLevel = 0;
    [HorizontalGroup("UtilityUpgrades/AutoConvertion")]
    [SerializeField] int[] autoConvertionTimer = new int[] { 20, 15, 10 };

    [HorizontalGroup("UtilityUpgrades/hpRecovery", 0.45f, LabelWidth = 130)]
    [SerializeField] int hpRecoveryLevel = 0;
    [HorizontalGroup("UtilityUpgrades/hpRecovery")]
    [SerializeField] int[] hpRecoveryTimerExtraPerc = new int[] { 10, 15, 20 };

    [HorizontalGroup("UtilityUpgrades/shieldStr", 0.45f, LabelWidth = 130)]
    [SerializeField] int shieldStrLevel = 0;
    [HorizontalGroup("UtilityUpgrades/shieldStr")]
    [SerializeField] int[] shieldExtraStr = new int[] { 5, 10, 15 };

    [HorizontalGroup("UtilityUpgrades/ShieldRecovery", 0.45f, LabelWidth = 130)]
    [SerializeField] int shieldRecoveryLevel = 0;
    [HorizontalGroup("UtilityUpgrades/ShieldRecovery")]
    [SerializeField] int[] shieldRecoveryExtraPerc = new int[] { 10, 15, 20 };
    #endregion

    #region Supers
    [FoldoutGroup("Supers")]
    [SerializeField] bool fourthDrone = false;
    [FoldoutGroup("Supers")]
    [SerializeField] bool superBomb = false;
    [FoldoutGroup("Supers")]
    [SerializeField] bool moreLaserCadency = false;
    [FoldoutGroup("Supers")]
    [SerializeField] bool secondIonStream = false;
    #endregion

    #region CurrentPower
    public int LaserPower 
    { 
        get => GetBonusValue(laserPowerLevel, laserPowerExtraDamage);
        set => laserPowerLevel = SetBonusValue(value);
    }
    public int DronePower 
    { 
        get => GetBonusValue(dronePowerLevel, dronePowerExtraDamage);
        set => dronePowerLevel = SetBonusValue(value);
    }
    public int IonStreamPower 
    {
        get => GetBonusValue(ionStreamPowerLevel, ionStreamPowerExtraDamage);
        set => ionStreamPowerLevel = SetBonusValue(value);
    }
    public int BombPower 
    {
        get => GetBonusValue(bombPowerLevel, bombPowerExtraDamage); 
        set => bombPowerLevel = SetBonusValue(value);
    }
    public int LaserIonStreamCadency
    {
        get => GetBonusValue(laserIonStreamCadencyLevel, laserIonStreamCadencyPerc); 
        set => laserIonStreamCadencyLevel = SetBonusValue(value);
    }
    public int BombRegeneration 
    {
        get => GetBonusValue(bombRegenerationLevel, bombRegenerationTimer); 
        set => bombRegenerationLevel = SetBonusValue(value);
    }
    public int ExtraDamageToEnemies
    {
        get => GetBonusValue(extraDamageToEnemiesLevel, extraDamageToEnemiesPerc); 
        set => extraDamageToEnemiesLevel = SetBonusValue(value);
    }
    public int ExtraDamageToObjectives
    { 
        get => GetBonusValue(extraDamageToObjectivesLevel, extraDamageToObjectivesPerc);
        set => extraDamageToObjectivesLevel = SetBonusValue(value);
    }
    #endregion

    #region CurrentUtility
    public int Tractor
    {
        get => GetBonusValue(tractorLevel, tractorExtraPerc);
        set => tractorLevel = SetBonusValue(value);
    }
    public int DroneIonStreamRange
    {
        get => GetBonusValue(droneIonStreamRangeLevel, droneIonStreamRangeExtraPerc); 
        set => droneIonStreamRangeLevel = SetBonusValue(value);
    }
    public int Mobility
    {
        get => GetBonusValue(mobilityLevel, mobilityExtraPerc); 
        set => mobilityLevel = SetBonusValue(value);
    }
    public int PowerUpDrop
    {
        get => GetBonusValue(powerUpDropLevel, powerUpDropExtraPerc); 
        set => powerUpDropLevel = SetBonusValue(value);
    }
    public int EnergyCristalsDrop
    {
        get => GetBonusValue(energyCristalsDropLevel, energyCristalsExtraPerc); 
        set => energyCristalsDropLevel = SetBonusValue(value);
    }
    public int AutoConvertion
    {
        get => GetBonusValue(autoConvertionLevel, autoConvertionTimer);
        set => autoConvertionLevel = SetBonusValue(value);
    }
    public int HP_Recovery
    {
        get => GetBonusValue(hpRecoveryLevel, hpRecoveryTimerExtraPerc);
        set => hpRecoveryLevel = SetBonusValue(value);
    }
    public int ShieldStrenght
    {
        get => GetBonusValue(shieldStrLevel, shieldExtraStr);
        set => shieldStrLevel = SetBonusValue(value);
    }
    public int ShieldRecovery
    {
        get => GetBonusValue(shieldRecoveryLevel, shieldRecoveryExtraPerc);
        set => shieldRecoveryLevel = SetBonusValue(value);
    }
    #endregion

    #region CurrentSupers
    public bool FourthDrone 
    {
        get => fourthDrone;
        set => fourthDrone = value;
    }
    public bool SuperBomb 
    {
        get => superBomb;
        set => superBomb = value;
    }
    public bool MoreLaserCadency 
    {
        get => moreLaserCadency;
        set => moreLaserCadency = value;
    }
    public bool SecondIonStream 
    {
        get => secondIonStream;
        set => secondIonStream = value;
    }
    #endregion

    static public BonusPowersDealer Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    int GetBonusValue(int level, int[] array)
    {
        if (level == 0) return 0;
        else
            return array[level - 1];
    }
    int SetBonusValue (int value)
    {
        if(value < 0) return 0;
        else if (value > 3) return 3;
        else return value;        
    }
}