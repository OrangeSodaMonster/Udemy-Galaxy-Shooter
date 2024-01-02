using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLasers : MonoBehaviour
{   
    [SerializeField] InputSO Input;
    [SerializeField] GameObject basicLaser;
    [SerializeField] Transform[] frontLaserParents;
    [SerializeField] Transform[] spreadLaserParents;
    [SerializeField] Transform[] sideLaserParents;
    [SerializeField] Transform[] backLaserParents;

    float currentLaserCDMod = 1;

    PlayerUpgradesManager upgradesManager;

    private void Awake()
    {
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
    }

    void Update()
    {
        FrontLasersShoot();
        SpreadLaserShoot();
        SideLasersShoot();
        BackLasersShoot();
    }

    float timeSinceFrontLaserShoot = float.MaxValue;
    bool justFiredLeftFrontLaser = false;
    void FrontLasersShoot()
    {
        Material laserMaterial = upgradesManager.LaserUpgradesInfo.PowerUpgrades[upgradesManager.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel-1].Material;
        bool IsFrontShotActivated = upgradesManager.CurrentUpgrades.FrontLaserUpgrades.Enabled;
        float frontLaserCD = 
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;
        
        if (IsFrontShotActivated & !justFiredLeftFrontLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceFrontLaserShoot >= frontLaserCD)
        {
            InstantiateLaser(frontLaserParents[0], upgradesManager.CurrentUpgrades.FrontLaserUpgrades);
            timeSinceFrontLaserShoot = 0;
            justFiredLeftFrontLaser = true;
        }
        else if (IsFrontShotActivated & justFiredLeftFrontLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceFrontLaserShoot > frontLaserCD)
        {
            InstantiateLaser(frontLaserParents[1], upgradesManager.CurrentUpgrades.FrontLaserUpgrades);

            timeSinceFrontLaserShoot = 0;
            justFiredLeftFrontLaser = false;
        }
        timeSinceFrontLaserShoot += Time.deltaTime;
    }

    float timeSinceSpreadLaserShoot = float.MaxValue;
    bool justFiredRightSpreadLaser = false;
    void SpreadLaserShoot()
    {
        bool IsSpreadShotActivated = upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.Enabled;
        float spreadLaserCD =
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;

        if (IsSpreadShotActivated & !justFiredRightSpreadLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceSpreadLaserShoot >= spreadLaserCD)
        {
            InstantiateLaser(spreadLaserParents[1], upgradesManager.CurrentUpgrades.SpreadLaserUpgrades);

            timeSinceSpreadLaserShoot = 0;
            justFiredRightSpreadLaser = true;
        }
        else if (IsSpreadShotActivated & justFiredRightSpreadLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceSpreadLaserShoot >= spreadLaserCD)
        {

            InstantiateLaser(spreadLaserParents[0], upgradesManager.CurrentUpgrades.SpreadLaserUpgrades);

            timeSinceSpreadLaserShoot = 0;
            justFiredRightSpreadLaser = false;
        }
        timeSinceSpreadLaserShoot += Time.deltaTime;
    }

    float timeSinceSideLaserShoot = float.MaxValue;
    bool justFiredLeftSideLaser = false;
    void SideLasersShoot()
    {
        bool IsSideShotActivated = upgradesManager.CurrentUpgrades.SideLaserUpgrades.Enabled;
        float sideLaserCD =
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.SideLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;

        if (IsSideShotActivated & !justFiredLeftSideLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceSideLaserShoot >= sideLaserCD)
        {
            InstantiateLaser(sideLaserParents[0], upgradesManager.CurrentUpgrades.SideLaserUpgrades);

            timeSinceSideLaserShoot = 0;
            justFiredLeftSideLaser = true;
        }
        else if (IsSideShotActivated & justFiredLeftSideLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceSideLaserShoot > sideLaserCD)
        {
            InstantiateLaser(sideLaserParents[1], upgradesManager.CurrentUpgrades.SideLaserUpgrades);

            timeSinceSideLaserShoot = 0;
            justFiredLeftSideLaser = false;
        }
        timeSinceSideLaserShoot += Time.deltaTime;
    }

    float timeSinceBackLaserShoot = float.MaxValue;
    bool justFiredRightBackLaser = false;
    void BackLasersShoot()
    {
        bool IsBackShotActivated = upgradesManager.CurrentUpgrades.BackLaserUpgrades.Enabled;
        float backLaserCD =
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.BackLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;

        if (IsBackShotActivated & !justFiredRightBackLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceBackLaserShoot >= backLaserCD)
        {
            InstantiateLaser(backLaserParents[1], upgradesManager.CurrentUpgrades.BackLaserUpgrades);

            timeSinceBackLaserShoot = 0;
            justFiredRightBackLaser = true;
        }
        else if (IsBackShotActivated & justFiredRightBackLaser & (Input.IsAutoFire | Input.IsFiring) & timeSinceBackLaserShoot >= backLaserCD)
        {
            InstantiateLaser(backLaserParents[0], upgradesManager.CurrentUpgrades.BackLaserUpgrades);

            timeSinceBackLaserShoot = 0;
            justFiredRightBackLaser = false;
        }
        timeSinceBackLaserShoot += Time.deltaTime;
    }

    void InstantiateLaser(Transform laserParent, LaserUpgrades laserUpgrades)
    {
        GameObject laser = Instantiate(basicLaser, transform.position + transform.TransformDirection(laserParent.position),
            transform.rotation * laserParent.rotation, laserParent);

        laser.GetComponent<SpriteRenderer>().material = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Material;
        laser.GetComponent<PlayerLaserDamage>().Damage = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Damage;
    }

    public void PowerUpStart(float value)
    {
        currentLaserCDMod = value;
    }
    public void PowerUpEnd()
    {
        currentLaserCDMod = 1;
    }
}