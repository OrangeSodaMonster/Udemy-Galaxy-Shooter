using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum EnemyType
{
    Drone = 0,
    Ship = 1,
    SummonedDrone = 2,
    Sentinel = 3,
}

[Serializable]
public enum EnemyColor
{
    Green = 0,
    Yellow = 1,
    Orange = 2,
    Red = 3,
}

[CreateAssetMenu(fileName = "MasterEnemyInfo", menuName = "MySOs/MasterEnemyInfo")]
public class EnemyInfoMasterSO : BaseMasterEnemyInfo
{
    #region "Input"

    [BoxGroup("Percents of Variation")]
    [SerializeField, HorizontalGroup("Percents of Variation/1"), GUIColor("#ffb0b0")]
    float shootCdVariationPerc;
    public float ShootCdVariationPerc => shootCdVariationPerc;
    [SerializeField, HorizontalGroup("Percents of Variation/1"), GUIColor("#8559ff")]
    float speedVariationPerc;
    public float SpeedVariationPerc => speedVariationPerc;
    [SerializeField, HorizontalGroup("Percents of Variation/2")]
    float shipRotationTimeVariationPerc;
    public float ShipRotationTimeVariationPerc => shipRotationTimeVariationPerc;
    [SerializeField, HorizontalGroup("Percents of Variation/2")]
    float shipDroneSpawnVariationPerc;
    public float ShipDroneSpawnVariationPerc => shipDroneSpawnVariationPerc;

    [BoxGroup("DroneGreen")]
    [SerializeField, HorizontalGroup("DroneGreen/1"), Tooltip("HP"), GUIColor("#ff5959")] 
    int hPDroneGreen;
    [SerializeField, HorizontalGroup("DroneGreen/1"), Tooltip("ShootingCD"), GUIColor("#ffb0b0")] 
    float shootCdDroneGreen;
    [SerializeField, HorizontalGroup("DroneGreen/2"), Tooltip("Speed"), GUIColor("#8559ff")] 
    float speedDroneGreen;
    [SerializeField, HorizontalGroup("DroneGreen/2"), Tooltip("Collision Damage"), GUIColor("#efff85")] 
    int colDamageDroneGreen;
    [SerializeField, HorizontalGroup("DroneGreen/2"), Tooltip("Impact Velocity"), GUIColor("#efff85")]
    float impactVelDroneGreen;

    [BoxGroup("ShipGreen")]
    [SerializeField, HorizontalGroup("ShipGreen/1"), Tooltip("HP"), GUIColor("#ff5959")]
    int hPShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/1"), Tooltip("ShootingCD"), GUIColor("#ffb0b0")]
    float shootCdShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/2"), Tooltip("Speed"), GUIColor("#8559ff")]
    float speedShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/2"), Tooltip("Collision Damage"), GUIColor("#efff85")]
    int colDamageShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/2"), Tooltip("Impact Velocity"), GUIColor("#efff85")]
    float impactVelShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/3"), Tooltip("Time Between Rotation Change")]
    float droneSpawnTimeShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/3"), Tooltip("Lateral Speed")]
    float xSpeedShipGreen;
    [SerializeField, HorizontalGroup("ShipGreen/3"), Tooltip("Time Between Rotation Change")]
    float rotChangeTimeShipGreen;

    [BoxGroup("SumDroneGreen")]
    [SerializeField, HorizontalGroup("SumDroneGreen/1"), Tooltip("HP"), GUIColor("#ff5959")]
    int hPSumDroneGreen;    
    [SerializeField, HorizontalGroup("SumDroneGreen/2"), Tooltip("Speed"), GUIColor("#8559ff")]
    float speedSumDroneGreen;
    [SerializeField, HorizontalGroup("SumDroneGreen/2"), Tooltip("Collision Damage"), GUIColor("#efff85")]
    int colDamageSumDroneGreen;
    [SerializeField, HorizontalGroup("SumDroneGreen/2"), Tooltip("Impact Velocity"), GUIColor("#efff85")]
    float impactSumDroneGreen;

    [BoxGroup("SentinelGreen")]
    [SerializeField, HorizontalGroup("SentinelGreen/1"), Tooltip("HP"), GUIColor("#ff5959")]
    int hPSentinelGreen;
    [SerializeField, HorizontalGroup("SentinelGreen/1"), Tooltip("ShootingCD"), GUIColor("#ffb0b0")]
    float shootCdSentinelGreen;
    [SerializeField, HorizontalGroup("SentinelGreen/2"), Tooltip("Collision Damage"), GUIColor("#efff85")]
    int colDamageSentinelGreen;
    public int ColDamageSentinelGreen => colDamageSentinelGreen;
    [SerializeField, HorizontalGroup("SentinelGreen/2"), Tooltip("Impact Velocity"), GUIColor("#efff85")]
    float impactSentinelGreen;
    public float ImpactSentinelGreen => impactSentinelGreen;

    [BoxGroup("HP ColorMultipliers")]
    [SerializeField, HorizontalGroup("HP ColorMultipliers/1"), GUIColor("Yellow")] float yellowHpMult;
    [SerializeField, HorizontalGroup("HP ColorMultipliers/1"), GUIColor("Orange")] float orangeHpMult;
    [SerializeField, HorizontalGroup("HP ColorMultipliers/1"), GUIColor("Red")] float redHpMult;

    [BoxGroup("ShootCD ColorMultipliers")]
    [SerializeField, HorizontalGroup("ShootCD ColorMultipliers/1"), GUIColor("Yellow")] float yellowShootCdMult;
    [SerializeField, HorizontalGroup("ShootCD ColorMultipliers/1"), GUIColor("Orange")] float orangeShootCdMult;
    [SerializeField, HorizontalGroup("ShootCD ColorMultipliers/1"), GUIColor("Red")] float redShootCdMult;

    [BoxGroup("Speed ColorMultipliers"), Tooltip("Affects Impact Velocity Too")]
    [SerializeField, HorizontalGroup("Speed ColorMultipliers/1"), GUIColor("Yellow")] float yellowSpeedMult;
    [SerializeField, HorizontalGroup("Speed ColorMultipliers/1"), GUIColor("Orange")] float orangeSpeedMult;
    [SerializeField, HorizontalGroup("Speed ColorMultipliers/1"), GUIColor("Red")] float redSpeedMult;

    [BoxGroup("ColDamage ColorMultipliers")]
    [SerializeField, HorizontalGroup("ColDamage ColorMultipliers/1"), GUIColor("Yellow")] float yellowColDamageMult;
    [SerializeField, HorizontalGroup("ColDamage ColorMultipliers/1"), GUIColor("Orange")] float orangeColDamageMult;
    [SerializeField, HorizontalGroup("ColDamage ColorMultipliers/1"), GUIColor("Red")] float redColDamageMult;
        
    [BoxGroup("PowerUPs"), SerializeField, GUIColor("Green")] List<PowerUpDrops> greenPuDrops;
    [BoxGroup("PowerUPs"), SerializeField, GUIColor("Yellow")] List<PowerUpDrops> yellowPuDrops;
    [BoxGroup("PowerUPs"), SerializeField, GUIColor("Orange")] List<PowerUpDrops> orangePuDrops;
    [BoxGroup("PowerUPs"), SerializeField, GUIColor("Red")] List<PowerUpDrops> redPuDrops;

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

    public int CalculateHP(EnemyType type, EnemyColor color)
    {
        int hp;
        switch (type)
        {
            case EnemyType.Drone:
                hp = hPDroneGreen;
                break;
            case EnemyType.Ship:
                hp = hPShipGreen;
                break;
            case EnemyType.SummonedDrone:
                hp = hPSumDroneGreen;
                break;
            case EnemyType.Sentinel:
                hp = hPSentinelGreen;
                break;
            default:
                hp = 5;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. HP = 5</color>");
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowHpMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeHpMult;
                break;
            case EnemyColor.Red:
                multiplier = redHpMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        hp = (int)(hp * multiplier);

        if(hp%5 != 0)
        {
            hp -= hp%5;
        }

        return hp;
    }

    public float CalculateSpeed(EnemyType type, EnemyColor color)
    {
        float speed;
        switch (type)
        {
            case EnemyType.Drone:
                speed = speedDroneGreen;
                break;
            case EnemyType.Ship:
                speed = speedShipGreen;
                break;
            case EnemyType.SummonedDrone:
                speed = speedSumDroneGreen;
                break;
            case EnemyType.Sentinel:
                speed = 0;
                break;
            default:
                speed = 0;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Speed = 0</color>");
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowSpeedMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeSpeedMult;
                break;
            case EnemyColor.Red:
                multiplier = redSpeedMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        return speed * multiplier;        
    }

    public float CalculateXSpeed(EnemyType type, EnemyColor color)
    {
        float xSpeed;
        switch (type)
        {            
            case EnemyType.Ship:
                xSpeed = xSpeedShipGreen;
                break;            
            default:
                xSpeed = 0;
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowSpeedMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeSpeedMult;
                break;
            case EnemyColor.Red:
                multiplier = redSpeedMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }
        return xSpeed * multiplier;
    }

    public float CalculateRotationChangeTime(EnemyType type, EnemyColor color)
    {
        float time;
        switch (type)
        {
            case EnemyType.Ship:
                time = rotChangeTimeShipGreen;
                break;
            default:
                time = 0;
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowSpeedMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeSpeedMult;
                break;
            case EnemyColor.Red:
                multiplier = redSpeedMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }
        return time - time * (multiplier - 1);
    }

    public float CalculateDroneSpawnTime(EnemyType type, EnemyColor color)
    {
        float time;
        switch (type)
        {
            case EnemyType.Ship:
                time = droneSpawnTimeShipGreen;
                break;
            default:
                time = 0;
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowShootCdMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeShootCdMult;
                break;
            case EnemyColor.Red:
                multiplier = redShootCdMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }
        return time * multiplier;
    }

    public int CalculateColDamage(EnemyType type, EnemyColor color)
    {
        int damage;
        switch (type)
        {
            case EnemyType.Drone:
                damage = colDamageDroneGreen;
                break;
            case EnemyType.Ship:
                damage = colDamageShipGreen;
                break;
            case EnemyType.SummonedDrone:
                damage = colDamageSumDroneGreen;
                break;
            case EnemyType.Sentinel:
                damage = colDamageSentinelGreen;
                break;
            default:
                damage = 0;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Damage = 0</color>");
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowColDamageMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeColDamageMult;
                break;
            case EnemyColor.Red:
                multiplier = redColDamageMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        damage = (int)(damage * multiplier);

        if (damage%5 != 0)
        {
            damage -= damage%5;
        }

        return damage;
    }

    public float CalculateImpactVelocity(EnemyType type, EnemyColor color)
    {
        float impact;
        switch (type)
        {
            case EnemyType.Drone:
                impact = impactVelDroneGreen;
                break;
            case EnemyType.Ship:
                impact = impactVelShipGreen;
                break;
            case EnemyType.SummonedDrone:
                impact = impactSumDroneGreen;
                break;
            case EnemyType.Sentinel:
                impact = impactSentinelGreen;
                break;
            default:
                impact = 0;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. Impact = 0</color>");
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowSpeedMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeSpeedMult;
                break;
            case EnemyColor.Red:
                multiplier = redSpeedMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        return impact * multiplier;
    }

    public float CalculateShootCD(EnemyType type, EnemyColor color)
    {
        float cd;
        switch (type)
        {
            case EnemyType.Drone:
                cd = shootCdDroneGreen;
                break;
            case EnemyType.Ship:
                cd = shootCdShipGreen;
                break;
            case EnemyType.SummonedDrone:
                cd = 0;
                break;
            case EnemyType.Sentinel:
                cd = shootCdSentinelGreen;
                break;
            default:
                cd = 5;
                Debug.Log("<color=red>EnemyType wrong, fell to Default. CD = 5</color>");
                break;
        }

        float multiplier;
        switch (color)
        {
            case EnemyColor.Green:
                multiplier = 1;
                break;
            case EnemyColor.Yellow:
                multiplier = yellowShootCdMult;
                break;
            case EnemyColor.Orange:
                multiplier = orangeShootCdMult;
                break;
            case EnemyColor.Red:
                multiplier = redShootCdMult;
                break;
            default:
                multiplier = 1;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default. Multiplier = 1</color>");
                break;
        }

        return cd * multiplier;
    }

    public List<PowerUpDrops> GetPowerUpList(EnemyColor color)
    {
        List<PowerUpDrops> powerUps = new List<PowerUpDrops>();
        switch (color)
        {
            case EnemyColor.Green:
                powerUps = greenPuDrops;
                break;
            case EnemyColor.Yellow:
                powerUps = yellowPuDrops;
                break;
            case EnemyColor.Orange:
                powerUps = orangePuDrops;
                break;
            case EnemyColor.Red:
                powerUps = redPuDrops;
                break;
            default:
                powerUps = greenPuDrops;
                Debug.Log("<color=red>EnemyColor wrong, fell to Default GreenPuDrops </color>");
                break;
        }

        return powerUps;
    }
}
