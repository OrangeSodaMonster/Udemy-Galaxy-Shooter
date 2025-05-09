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
    public int[] LaserPowerExtraDamage { get { return laserPowerExtraDamage; } }
    
    [HorizontalGroup("PowerUpgrades/DronePower", 0.45f, LabelWidth = 130)]
    [SerializeField] int dronePowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/DronePower")]
    [SerializeField] int[] dronePowerExtraDamage = new int[] { 5, 10, 15 };
    public int[] DronePowerExtraDamage { get { return dronePowerExtraDamage; } }
    
    [HorizontalGroup("PowerUpgrades/IonStreamPower", 0.45f, LabelWidth = 130)]
    [SerializeField] int ionStreamPowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/IonStreamPower")]
    [SerializeField] int[] ionStreamPowerExtraDamage = new int[] { 5, 10, 15 };
    public int[] IonStreamPowerExtraDamage { get { return ionStreamPowerExtraDamage; } }

    [HorizontalGroup("PowerUpgrades/BombPower", 0.45f, LabelWidth = 130)]
    [SerializeField] int bombPowerLevel = 0;
    [HorizontalGroup("PowerUpgrades/BombPower")]
    [SerializeField] int[] bombPowerExtraDamage = new int[] { 50, 100, 150 };
    public int[] BombPowerExtraDamage { get { return bombPowerExtraDamage; } }

    [HorizontalGroup("PowerUpgrades/LaserIonStreamCadency", 0.45f, LabelWidth = 130)]
    [SerializeField] int laserIonStreamCadencyLevel = 0;
    [HorizontalGroup("PowerUpgrades/LaserIonStreamCadency")]
    [SerializeField] int[] laserIonStreamCadencyPerc = new int[] { 5, 10, 15 };
    public int[] LaserIonStreamCadencyPerc { get { return laserIonStreamCadencyPerc; } }

    [HorizontalGroup("PowerUpgrades/BombRegeneration", 0.45f, LabelWidth = 130)]
    [SerializeField] int bombRegenerationLevel = 0;
    [HorizontalGroup("PowerUpgrades/BombRegeneration")]
    [SerializeField] int[] bombRegenerationTimer = new int[] { 45, 35, 25 };
    public int[] BombRegenerationTimer { get { return bombRegenerationTimer; } }

    [HorizontalGroup("PowerUpgrades/ExtraEnemyDamage", 0.45f, LabelWidth = 130)]
    [SerializeField] int extraDamageToEnemiesLevel = 0;
    [HorizontalGroup("PowerUpgrades/ExtraEnemyDamage")]
    [SerializeField] int[] extraDamageToEnemiesPerc = new int[] { 5, 10, 15 }; // Reduce Enemy HP
    public int[] ExtraDamageToEnemiesPerc { get { return extraDamageToEnemiesPerc; } }

    [HorizontalGroup("PowerUpgrades/ExtraObjectiveDamage", 0.45f, LabelWidth = 130)]
    [SerializeField] int extraDamageToObjectivesLevel = 0;
    [HorizontalGroup("PowerUpgrades/ExtraObjectiveDamage")]
    [SerializeField] int[] extraDamageToObjectivesPerc = new int[] { 10, 15, 20 }; // Reduce Enemy HP
    public int[] ExtraDamageToObjectivesPerc { get { return extraDamageToObjectivesPerc; } }
    #endregion

    #region UtilityUpgrades
    [FoldoutGroup("UtilityUpgrades")]
    [HorizontalGroup("UtilityUpgrades/Tractor", 0.45f, LabelWidth = 130)]
    [SerializeField] int tractorLevel = 0;
    [HorizontalGroup("UtilityUpgrades/Tractor")]
    [SerializeField] int[] tractorExtraPerc = new int[] { 10, 20, 30 }; // Range, pull force
    public int[] TractorExtraPerc { get { return tractorExtraPerc; } }

    [HorizontalGroup("UtilityUpgrades/DroneIonRange", 0.45f, LabelWidth = 130)]
    [SerializeField] int droneIonStreamRangeLevel = 0;
    [HorizontalGroup("UtilityUpgrades/DroneIonRange")]
    [SerializeField] int[] droneIonStreamRangeExtraPerc = new int[] { 10, 15, 20 };
    public int[] DroneIonStreamRangeExtraPerc { get { return droneIonStreamRangeExtraPerc; } }
    
    [HorizontalGroup("UtilityUpgrades/Mobility", 0.45f, LabelWidth = 130)]
    [SerializeField] int mobilityLevel = 0;
    [HorizontalGroup("UtilityUpgrades/Mobility")]
    [SerializeField] int[] mobilityExtraPerc = new int[] { 5, 10, 15 }; // Speed, turn speed
    public int[] MobilityExtraPerc { get { return mobilityExtraPerc; } }
    
    [HorizontalGroup("UtilityUpgrades/PowerUpDrop", 0.45f, LabelWidth = 130)]
    [SerializeField] int powerUpDropLevel = 0;
    [HorizontalGroup("UtilityUpgrades/PowerUpDrop")]
    [SerializeField] int[] powerUpDropExtraPerc = new int[] { 10, 20, 30 }; // Speed, turn speed
    public int[] PowerUpDropExtraPerc { get { return powerUpDropExtraPerc; } }
    
    [HorizontalGroup("UtilityUpgrades/EnergyCristalDrop", 0.45f, LabelWidth = 130)]
    [SerializeField] int energyCristalsDropLevel = 0;
    [HorizontalGroup("UtilityUpgrades/EnergyCristalDrop")]
    [SerializeField] int[] energyCristalsExtraPerc = new int[] { 10, 20, 30 }; // Chance de dropar um cristal de energia a mais, tipo baseado na chance base blue 5, pink 1 por exemplo
    public int[] EnergyCristalsExtraPerc { get { return energyCristalsExtraPerc; } }
    
    [HorizontalGroup("UtilityUpgrades/AutoConvertion", 0.45f, LabelWidth = 130)]
    [SerializeField] int autoConvertionLevel = 0;
    [HorizontalGroup("UtilityUpgrades/AutoConvertion")]
    [SerializeField] int[] autoConvertionTimer = new int[] { 20, 15, 10 };
    public int[] AutoConvertionTimer { get { return autoConvertionTimer; } }

    [HorizontalGroup("UtilityUpgrades/hpRecovery", 0.45f, LabelWidth = 130)]
    [SerializeField] int hpRecoveryLevel = 0;
    [HorizontalGroup("UtilityUpgrades/hpRecovery")]
    [SerializeField] int[] hpRecoveryTimerExtraPerc = new int[] { 10, 15, 20 };
    public int[] HpRecoveryTimerExtraPerc { get { return hpRecoveryTimerExtraPerc; } }

    [HorizontalGroup("UtilityUpgrades/shieldStr", 0.45f, LabelWidth = 130)]
    [SerializeField] int shieldStrLevel = 0;
    [HorizontalGroup("UtilityUpgrades/shieldStr")]
    [SerializeField] int[] shieldExtraStr = new int[] { 5, 10, 15 };
    public int[] ShieldExtraStr { get { return shieldExtraStr; } }

    [HorizontalGroup("UtilityUpgrades/ShieldRecovery", 0.45f, LabelWidth = 130)]
    [SerializeField] int shieldRecoveryLevel = 0;
    [HorizontalGroup("UtilityUpgrades/ShieldRecovery")]
    [SerializeField] int[] shieldRecoveryExtraPerc = new int[] { 10, 15, 20 };
    public int[] ShieldRecoveryExtraPerc { get { return shieldRecoveryExtraPerc; } }
    #endregion

    #region Supers
    [FoldoutGroup("Supers")]
    [SerializeField] bool fourthDrone = false;
    [HorizontalGroup("Supers/BombSuper")]
    [SerializeField] bool superBomb = false;
    [HorizontalGroup("Supers/BombSuper")]
    [SerializeField] int superBombExtraDamage = 200;
    [HorizontalGroup("Supers/BombSuper")]
    [SerializeField] float superBombExtraRangePerc = 15;
    [HorizontalGroup("Supers/LaserSuper", 0.45f, LabelWidth = 150)]
    [SerializeField] bool moreLaserCadency = false;
    [HorizontalGroup("Supers/LaserSuper")]
    [SerializeField] float moreLaserCadencyPerc = 15;
    [FoldoutGroup("Supers")]
    [SerializeField] bool secondIonStream = false;
    #endregion

    #region AutoConvertion
    [FoldoutGroup("AutoConversion")]
    [HorizontalGroup("AutoConversion/Metal to RareMetal")]
    [SerializeField] int minMetalToRareMetal = 150;
    [HorizontalGroup("AutoConversion/Metal to RareMetal")]
    [SerializeField] int metalToRareMetalPrice = 15;
    [HorizontalGroup("AutoConversion/RareMetal to EnergyCristal")]
    [SerializeField] int minRareMetalToEnergyCristal = 100;
    [HorizontalGroup("AutoConversion/RareMetal to EnergyCristal")]
    [SerializeField] int rareMetalToEnergyCristalPrice = 10;
    [HorizontalGroup("AutoConversion/EnergyCristal to CondensedEnergyCristal")]
    [SerializeField] int minEnergyCristalToCondensedEnergyCristal = 100;
    [HorizontalGroup("AutoConversion/EnergyCristal to CondensedEnergyCristal")]
    [SerializeField] int energyCristalToCondensedEnergyCristalPrice = 25;

    float convertionTimer = 0;
    void AutoConvertionDealer()
    {
        if(convertionTimer >= AutoConvertion)
        {
            if(PlayerCollectiblesCount.MetalAmount >= minMetalToRareMetal)
            {
                PlayerCollectiblesCount.MetalAmount -= metalToRareMetalPrice;
                PlayerCollectiblesCount.RareMetalAmount += 1;
            }
            if (PlayerCollectiblesCount.RareMetalAmount >= minRareMetalToEnergyCristal)
            {
                PlayerCollectiblesCount.RareMetalAmount -= rareMetalToEnergyCristalPrice;
                PlayerCollectiblesCount.EnergyCristalAmount += 1;
            }
            if (PlayerCollectiblesCount.EnergyCristalAmount >= minEnergyCristalToCondensedEnergyCristal)
            {
                PlayerCollectiblesCount.EnergyCristalAmount -= energyCristalToCondensedEnergyCristalPrice;
                PlayerCollectiblesCount.CondensedEnergyCristalAmount += 1;
            }
            PlayerCollectiblesCount.OnChangedCollectibleAmount?.Invoke();
            convertionTimer = 0;
        }

        convertionTimer += Time.deltaTime;
    }
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
    public int BombGeneration 
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
    public int DroneIonStreamBombRange
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
    public bool IsFourthDrone 
    {
        get => fourthDrone;
        set => fourthDrone = value;
    }
    public bool IsSuperBomb 
    {
        get => superBomb;
        set => superBomb = value;
    }
    public int SuperBombExtraDamage
    {
        get => superBombExtraDamage;
    }
    public float SuperBombExtraRange
    {
        get => superBombExtraRangePerc;
    }
    public bool IsMoreLaserCadency 
    {
        get => moreLaserCadency;
        set => moreLaserCadency = value;
    }
    public float MoreLaserCadencyPerc
    {
        get => moreLaserCadencyPerc;
        //set => moreLaserCadencyPerc = value;
    }
    public bool IsSecondIonStream 
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

    private void Update()
    {
        if(autoConvertionLevel >= 1)
        {
            AutoConvertionDealer();
        }
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

    public Dictionary<BonusSelection.BonusType, int> GetBonusLevels()
    {
        Dictionary<BonusSelection.BonusType, int> dict = new();

        for (int i = 0; i < BonusSelection.Instance.ListOfChances.Count; i++)
        {
            dict.Add(BonusSelection.Instance.ListOfChances[i].Type, 0);
            switch (BonusSelection.Instance.ListOfChances[i].Type)
            {
                case BonusSelection.BonusType.LaserPower:                   
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = laserPowerLevel;
                    break;
                case BonusSelection.BonusType.DronePower:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = dronePowerLevel;
                    break;
                case BonusSelection.BonusType.IonStreamPower:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] =  ionStreamPowerLevel;
                    break;
                case BonusSelection.BonusType.BombPower:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] =  bombPowerLevel;
                    break;
                case BonusSelection.BonusType.LaserIonCadency:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = laserIonStreamCadencyLevel;
                    break;
                case BonusSelection.BonusType.BombGeneration:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = bombRegenerationLevel;
                    break;
                case BonusSelection.BonusType.EnemyExtraDamage:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = extraDamageToEnemiesLevel;
                    break;
                case BonusSelection.BonusType.ObjectiveExtraDamage:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = extraDamageToObjectivesLevel;
                    break;
                case BonusSelection.BonusType.Tractor:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = tractorLevel;
                    break;
                case BonusSelection.BonusType.DroneIonBombRange:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = droneIonStreamRangeLevel;
                    break;
                case BonusSelection.BonusType.Mobility:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = mobilityLevel;
                    break;
                case BonusSelection.BonusType.PowerUpDrop:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = powerUpDropLevel;
                    break;
                case BonusSelection.BonusType.CristalDrop:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = energyCristalsDropLevel;
                    break;
                case BonusSelection.BonusType.AutoConvertion:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = autoConvertionLevel;
                    break;
                case BonusSelection.BonusType.HpRecovery:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = hpRecoveryLevel;
                    break;
                case BonusSelection.BonusType.ShieldStrenght:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = shieldStrLevel;
                    break;
                case BonusSelection.BonusType.ShieldRecovery:
                    dict[BonusSelection.Instance.ListOfChances[i].Type] = shieldRecoveryLevel;
                    break;
            }
        }
            return dict;
    }

    public void AddBonusLevel(BonusSelection.BonusType type)
    {
        switch (type)
        {
            case BonusSelection.BonusType.LaserPower:
                laserPowerLevel++;
                break;
            case BonusSelection.BonusType.DronePower:
                dronePowerLevel++;
                break;
            case BonusSelection.BonusType.IonStreamPower:
                ionStreamPowerLevel++;
                break;
            case BonusSelection.BonusType.BombPower:
                bombPowerLevel++;
                break;
            case BonusSelection.BonusType.LaserIonCadency:
                laserIonStreamCadencyLevel++;
                break;
            case BonusSelection.BonusType.BombGeneration:
                bombRegenerationLevel++;
                break;
            case BonusSelection.BonusType.EnemyExtraDamage:
                extraDamageToEnemiesLevel++;
                break;
            case BonusSelection.BonusType.ObjectiveExtraDamage:
                extraDamageToObjectivesLevel++;
                break;
            case BonusSelection.BonusType.Tractor:
                tractorLevel++;
                break;
            case BonusSelection.BonusType.DroneIonBombRange:
                droneIonStreamRangeLevel++;
                break;
            case BonusSelection.BonusType.Mobility:
                mobilityLevel++;
                break;
            case BonusSelection.BonusType.PowerUpDrop:
                powerUpDropLevel++;
                break;
            case BonusSelection.BonusType.CristalDrop:
                energyCristalsDropLevel++;
                break;
            case BonusSelection.BonusType.AutoConvertion:
                autoConvertionLevel++;
                break;
            case BonusSelection.BonusType.HpRecovery:
                hpRecoveryLevel++;
                break;
            case BonusSelection.BonusType.ShieldStrenght:
                shieldStrLevel++;
                break;
            case BonusSelection.BonusType.ShieldRecovery:
                shieldRecoveryLevel++;
                break;
            case BonusSelection.BonusType.SuperBomb:
                IsSuperBomb = true;
                break;
            case BonusSelection.BonusType.SuperFourthDrone:
                IsFourthDrone = true;
                break;
            case BonusSelection.BonusType.SuperLaserCadency:
                IsMoreLaserCadency = true;
                break;
            case BonusSelection.BonusType.SuperSecondIonStream:
                IsSecondIonStream = true;
                break;
        }
    }
}