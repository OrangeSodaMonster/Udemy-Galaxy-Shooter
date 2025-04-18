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

    private void Awake()
    {
        upgradesManager = PlayerUpgradesManager.Instance;
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
        bool IsFrontShotActivated = upgradesManager.CurrentUpgrades.FrontLaserUpgrades.Enabled && !upgradesManager.CurrentUpgrades.FrontLaserUpgrades.DisableOverwrite;
        float frontLaserCD = 
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;
        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 - BonusPowersDealer.Instance.LaserIonStreamCadency/100;
            frontLaserCD *= bonusMultiplier;
        }

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
        bool IsSpreadShotActivated = upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.Enabled && !upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.DisableOverwrite;
        float spreadLaserCD =
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;
        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 - BonusPowersDealer.Instance.LaserIonStreamCadency/100;
            spreadLaserCD *= bonusMultiplier;
        }

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
        bool IsSideShotActivated = upgradesManager.CurrentUpgrades.SideLaserUpgrades.Enabled  && !upgradesManager.CurrentUpgrades.SideLaserUpgrades.DisableOverwrite;
        float sideLaserCD =
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.SideLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;
        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 - BonusPowersDealer.Instance.LaserIonStreamCadency/100;
            sideLaserCD *= bonusMultiplier;
        }

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
        bool IsBackShotActivated = upgradesManager.CurrentUpgrades.BackLaserUpgrades.Enabled && !upgradesManager.CurrentUpgrades.BackLaserUpgrades.DisableOverwrite;
        float backLaserCD =
            upgradesManager.LaserUpgradesInfo.CadencyUpgrades[upgradesManager.CurrentUpgrades.BackLaserUpgrades.CadencyLevel-1].TimeBetween / 2 / currentLaserCDMod;
        if (GameManager.IsSurvival)
        {
            float bonusMultiplier = 1 - BonusPowersDealer.Instance.LaserIonStreamCadency/100;
            backLaserCD *= bonusMultiplier;
        }

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
        laser.GetComponent<PlayerLaserDamage>().Damage = upgradesManager.LaserUpgradesInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Damage;
        if (GameManager.IsSurvival)
            laser.GetComponent<PlayerLaserDamage>().Damage += BonusPowersDealer.Instance.LaserPower;
        laser.SetActive(true);

        AudioManager.Instance.PlayLaserSound();
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