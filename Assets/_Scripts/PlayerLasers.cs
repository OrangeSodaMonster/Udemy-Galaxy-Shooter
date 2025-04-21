using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLasers : MonoBehaviour
{   
    //[SerializeField] InputSO Input;
    [SerializeField] GameObject basicLaser;
    [SerializeField] Transform[] frontLaserParents;
    [SerializeField] Transform[] spreadLaserParents;
    [SerializeField] Transform[] sideLaserParents;
    [SerializeField] Transform[] backLaserParents;
    [SerializeField] MMSimpleObjectPooler objPool;

    float currentLaserCDMod = 1;

    PlayerUpgradesManager upgradesManager;
    PlayerStats stats;

    private void Awake()
    {
        upgradesManager = PlayerUpgradesManager.Instance;
    }

    private void Start()
    {
        stats = PlayerStats.Instance;
    }

    void Update()
    {
        if (GameStatus.IsPaused || GameStatus.IsPortal) return;

        FrontLasersShoot();
        SpreadLaserShoot();
        SideLasersShoot();
        BackLasersShoot();
    }

    float timeSinceFrontLaserShoot = float.MaxValue;
    bool justFiredLeftFrontLaser = false;
    void FrontLasersShoot()
    {
        PlayerStats.LaserStats laserStats = PlayerStats.Instance.Lasers.FrontLaser;
        bool IsFrontShotActivated = laserStats.Enabled;

        float frontLaserCD = laserStats.CurrentInterval/2/currentLaserCDMod;
            //upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;
        
            if (IsFrontShotActivated && !justFiredLeftFrontLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceFrontLaserShoot >= frontLaserCD)
        {
            InstantiateLaser(frontLaserParents[0], upgradesManager.CurrentUpgrades.FrontLaserUpgrades, LaserType.Frontal);
            timeSinceFrontLaserShoot = 0;
            justFiredLeftFrontLaser = true;
        }
        else if (IsFrontShotActivated && justFiredLeftFrontLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceFrontLaserShoot > frontLaserCD)
        {
            InstantiateLaser(frontLaserParents[1], upgradesManager.CurrentUpgrades.FrontLaserUpgrades, LaserType.Frontal);

            timeSinceFrontLaserShoot = 0;
            justFiredLeftFrontLaser = false;
        }
        timeSinceFrontLaserShoot += Time.deltaTime;
    }
    float timeSinceSpreadLaserShoot = float.MaxValue;
    bool justFiredRightSpreadLaser = false;

    void SpreadLaserShoot()
    {
        PlayerStats.LaserStats laserStats = PlayerStats.Instance.Lasers.SpreadLaser;
        bool IsSpreadShotActivated = laserStats.Enabled;
        float spreadLaserCD = laserStats.CurrentInterval/2/currentLaserCDMod;

        if (IsSpreadShotActivated && !justFiredRightSpreadLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceSpreadLaserShoot >= spreadLaserCD)
        {
            InstantiateLaser(spreadLaserParents[1], upgradesManager.CurrentUpgrades.SpreadLaserUpgrades, LaserType.Spread);

            timeSinceSpreadLaserShoot = 0;
            justFiredRightSpreadLaser = true;
        }
        else if (IsSpreadShotActivated & justFiredRightSpreadLaser & (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) & timeSinceSpreadLaserShoot >= spreadLaserCD)
        {

            InstantiateLaser(spreadLaserParents[0], upgradesManager.CurrentUpgrades.SpreadLaserUpgrades, LaserType.Spread);

            timeSinceSpreadLaserShoot = 0;
            justFiredRightSpreadLaser = false;
        }
        timeSinceSpreadLaserShoot += Time.deltaTime;
    }

    float timeSinceSideLaserShoot = float.MaxValue;
    bool justFiredLeftSideLaser = false;
    void SideLasersShoot()
    {
        PlayerStats.LaserStats laserStats = PlayerStats.Instance.Lasers.SideLaser;
        bool IsSideShotActivated = laserStats.Enabled;
        float sideLaserCD = laserStats.CurrentInterval/2/currentLaserCDMod;

        if (IsSideShotActivated && !justFiredLeftSideLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceSideLaserShoot >= sideLaserCD)
        {
            InstantiateLaser(sideLaserParents[0], upgradesManager.CurrentUpgrades.SideLaserUpgrades, LaserType.Lateral);

            timeSinceSideLaserShoot = 0;
            justFiredLeftSideLaser = true;
        }
        else if (IsSideShotActivated && justFiredLeftSideLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceSideLaserShoot > sideLaserCD)
        {
            InstantiateLaser(sideLaserParents[1], upgradesManager.CurrentUpgrades.SideLaserUpgrades, LaserType.Lateral);

            timeSinceSideLaserShoot = 0;
            justFiredLeftSideLaser = false;
        }
        timeSinceSideLaserShoot += Time.deltaTime;
    }

    float timeSinceBackLaserShoot = float.MaxValue;
    bool justFiredRightBackLaser = false;
    void BackLasersShoot()
    {
        PlayerStats.LaserStats laserStats = PlayerStats.Instance.Lasers.BackLaser;
        bool IsBackShotActivated = laserStats.Enabled;
        float backLaserCD = laserStats.CurrentInterval/2/currentLaserCDMod;

        if (IsBackShotActivated && !justFiredRightBackLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceBackLaserShoot >= backLaserCD)
        {
            InstantiateLaser(backLaserParents[1], upgradesManager.CurrentUpgrades.BackLaserUpgrades, LaserType.Back);

            timeSinceBackLaserShoot = 0;
            justFiredRightBackLaser = true;
        }
        else if (IsBackShotActivated && justFiredRightBackLaser && (GameManager.IsAutoFire || InputHolder.Instance.IsFiring) && timeSinceBackLaserShoot >= backLaserCD)
        {
            InstantiateLaser(backLaserParents[0], upgradesManager.CurrentUpgrades.BackLaserUpgrades, LaserType.Back);

            timeSinceBackLaserShoot = 0;
            justFiredRightBackLaser = false;
        }
        timeSinceBackLaserShoot += Time.deltaTime;
    }

    void InstantiateLaser(Transform laserParent, LaserUpgrades laserUpgrades, LaserType type)
    {
        GameObject laser = objPool.GetPooledGameObject();
        laser.transform.SetPositionAndRotation(transform.position + transform.TransformDirection(laserParent.position), transform.rotation * laserParent.rotation);
        //laser.GetComponent<SpriteRenderer>().material = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Material;
        laser.GetComponent<SpriteRenderer>().sprite = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Sprite;
        laser.GetComponent<LaserMove>().VFXGradient = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].VFXGradient;
        laser.GetComponent<PlayerLaserDamage>().LaserType = type;
        //laser.GetComponent<PlayerLaserDamage>().Damage = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Damage;
        laser.GetComponent<PlayerLaserDamage>().Damage = GetLaserDamage(type);
        
        laser.SetActive(true);

        AudioManager.Instance.PlayLaserSound();
    }
    int GetLaserDamage(LaserType type)
    {
        switch (type)
        {
            case LaserType.Frontal:
                return stats.Lasers.FrontLaser.CurrentPower;
            case LaserType.Spread:
                return stats.Lasers.SpreadLaser.CurrentPower;
            case LaserType.Lateral:
                return stats.Lasers.SideLaser.CurrentPower;
            case LaserType.Back:
                return stats.Lasers.BackLaser.CurrentPower;
            default:
                return 0;
        }
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