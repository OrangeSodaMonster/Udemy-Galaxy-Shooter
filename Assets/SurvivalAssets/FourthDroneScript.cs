using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthDroneScript : MonoBehaviour
{
    [BoxGroup("Drones")]
    [HorizontalGroup("Drones/1")]
    [SerializeField] DroneAttackScript drone1;
    [HorizontalGroup("Drones/1")]
    [SerializeField] Vector2 drone1Pos;
    [HorizontalGroup("Drones/2")]
    [SerializeField] DroneAttackScript drone2;
    [HorizontalGroup("Drones/2")]
    [SerializeField] Vector2 drone2Pos;
    [HorizontalGroup("Drones/3")]
    [SerializeField] DroneAttackScript drone3;
    [HorizontalGroup("Drones/3")]
    [SerializeField] Vector2 drone3Pos;
    [HorizontalGroup("Drones/4")]
    [SerializeField] DroneAttackScript drone4;
    [HorizontalGroup("Drones/4")]
    [SerializeField] Vector2 drone4Pos;

    PlayerUpgradesManager upgradesManager;

    void Start()
    {
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
    }

    void Update()
    {

        bool notAllDronesEnabled = !PlayerStats.Instance.Drones.Drone1.Enabled ||
            !PlayerStats.Instance.Drones.Drone2.Enabled || !PlayerStats.Instance.Drones.Drone3.Enabled;        

        if (BonusPowersDealer.Instance.IsFourthDrone)
        {
            UpdateDrone4Value();

            drone4.gameObject.SetActive(!notAllDronesEnabled && !GameStatus.IsPortal);

            drone1.transform.localPosition = drone1Pos;
            drone2.transform.localPosition = drone2Pos;
            drone3.transform.localPosition = drone3Pos;
            drone4.transform.localPosition = drone4Pos;
        }
    }

    void UpdateDrone4Value()
    {
        //Use Lowest Values
        int lowestDamageLevel = 100;
        if (PlayerStats.Instance.Drones.Drone1.PowerUpgrades.Upgrades < lowestDamageLevel)
            lowestDamageLevel = PlayerStats.Instance.Drones.Drone1.PowerUpgrades.Upgrades;
        if (PlayerStats.Instance.Drones.Drone2.PowerUpgrades.Upgrades < lowestDamageLevel)
            lowestDamageLevel = PlayerStats.Instance.Drones.Drone2.PowerUpgrades.Upgrades;
        if (PlayerStats.Instance.Drones.Drone3.PowerUpgrades.Upgrades < lowestDamageLevel)
            lowestDamageLevel = PlayerStats.Instance.Drones.Drone3.PowerUpgrades.Upgrades;

        int lowestRangeLevel = 100;
        if (PlayerStats.Instance.Drones.Drone1.RangeUpgrades.Upgrades < lowestRangeLevel)
            lowestRangeLevel = PlayerStats.Instance.Drones.Drone1.RangeUpgrades.Upgrades;
        if (PlayerStats.Instance.Drones.Drone2.RangeUpgrades.Upgrades < lowestRangeLevel)
            lowestRangeLevel = PlayerStats.Instance.Drones.Drone2.RangeUpgrades.Upgrades;
        if (PlayerStats.Instance.Drones.Drone3.RangeUpgrades.Upgrades < lowestRangeLevel)
            lowestRangeLevel = PlayerStats.Instance.Drones.Drone3.RangeUpgrades.Upgrades;

        drone4.DamagePerSecond = upgradesManager.DroneUpgradesInfo.PowerUpgrades[lowestDamageLevel - 1].DamagePerSecond;
        drone4.LineColor = upgradesManager.DroneUpgradesInfo.PowerUpgrades[lowestDamageLevel - 1].Color;
        drone4.Range = upgradesManager.DroneUpgradesInfo.RangeUpgrades[lowestRangeLevel - 1].Range;
        drone4.VFXScaleMultiplier = 0.95f + .05f * lowestDamageLevel;
    }
}
