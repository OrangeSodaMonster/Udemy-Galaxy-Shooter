using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    [SerializeField] DroneAttackScript drone1;
    [SerializeField] DroneAttackScript drone2;
    [SerializeField] DroneAttackScript drone3;
    [SerializeField] float rotationSpeed;

    Transform player;
    PlayerUpgradesManager upgradesManager;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
    }

    void Update()
    {        
        transform.position = player.position;
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);

        UpdateDroneValue(drone1, upgradesManager.CurrentUpgrades.Drone_1_Upgrades, DroneNumber.One);
        UpdateDroneValue(drone2, upgradesManager.CurrentUpgrades.Drone_2_Upgrades, DroneNumber.Two);
        UpdateDroneValue(drone3, upgradesManager.CurrentUpgrades.Drone_3_Upgrades, DroneNumber.Three);

        drone1.gameObject.SetActive(PlayerStats.Instance.Drones.Drone1.Enabled && !GameStatus.IsPortal);
        drone2.gameObject.SetActive(PlayerStats.Instance.Drones.Drone2.Enabled && !GameStatus.IsPortal);
        drone3.gameObject.SetActive(PlayerStats.Instance.Drones.Drone3.Enabled && !GameStatus.IsPortal);
    }

    void UpdateDroneValue(DroneAttackScript drone, DronesUpgrades droneUpgrades, DroneNumber droneNumber)
    {
        GetValues(droneNumber, out int damage, out float range);
        drone.DamagePerSecond = damage;
        drone.Range = range;
        drone.LineColor = upgradesManager.DroneUpgradesInfo.PowerUpgrades[droneUpgrades.DamageLevel - 1].Color;
        drone.VFXScaleMultiplier = 0.95f + .05f * droneUpgrades.DamageLevel;
    }

    void GetValues(DroneNumber droneNumber, out int damagePerSecond, out float range)
    {
        PlayerStats.DronesStats dronesStats = PlayerStats.Instance.Drones;
        damagePerSecond = 0;
        range = 0;

        switch (droneNumber)
        {
            case DroneNumber.One:
                damagePerSecond = dronesStats.Drone1.CurrentPower;
                range = dronesStats.Drone1.CurrentRange;
                break;
            case DroneNumber.Two:
                damagePerSecond = dronesStats.Drone2.CurrentPower;
                range = dronesStats.Drone2.CurrentRange;
                break;
            case DroneNumber.Three:
                damagePerSecond = dronesStats.Drone3.CurrentPower;
                range = dronesStats.Drone3.CurrentRange;
                break;
        }
    }
}