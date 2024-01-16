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

        UpdateDroneValue(drone1, upgradesManager.CurrentUpgrades.Drone_1_Upgrades);
        UpdateDroneValue(drone2, upgradesManager.CurrentUpgrades.Drone_2_Upgrades);
        UpdateDroneValue(drone3, upgradesManager.CurrentUpgrades.Drone_3_Upgrades);

        drone1.gameObject.SetActive(upgradesManager.CurrentUpgrades.Drone_1_Upgrades.Enabled);
        drone2.gameObject.SetActive(upgradesManager.CurrentUpgrades.Drone_2_Upgrades.Enabled);
        drone3.gameObject.SetActive(upgradesManager.CurrentUpgrades.Drone_3_Upgrades.Enabled);
    }

    void UpdateDroneValue(DroneAttackScript drone, DronesUpgrades droneUpgrades)
    {
        drone.DamagePerSecond = upgradesManager.DroneUpgradesInfo.PowerUpgrades[droneUpgrades.DamageLevel - 1].DamagePerSecond;
        drone.LineColor = upgradesManager.DroneUpgradesInfo.PowerUpgrades[droneUpgrades.DamageLevel - 1].Color;
        drone.Range = upgradesManager.DroneUpgradesInfo.RangeUpgrades[droneUpgrades.RangeLevel - 1].Range;
        drone.VFXScaleMultiplier = 0.95f + .05f * droneUpgrades.DamageLevel;
    }
}