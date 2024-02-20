using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum LaserUpgradeType
{
    Main, Power, FireRate,
}
enum LaserPosition
{
    Front, Spread, Side, Back,
}

public class LaserButtonScript : MonoBehaviour
{
    [SerializeField] InterfaceDataHolder interfaceData;
    [Header("")]
    [SerializeField] LaserPosition position;
    [SerializeField] LaserUpgradeType upgradeType;
    [SerializeField] Image border;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] GameObject[] costs;

    LaserUpgradesInfo laserUpgradeInfo;
    Button button;

    public static event Action UpgradedLaser;

    void Awake()
    {
        laserUpgradeInfo = PlayerUpgradesManager.Instance.LaserUpgradesInfo;
        button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        UpgradeLaserButtons();
        UpgradedLaser += UpgradeLaserButtons;
        UpgradeDisableOverwrite.OnUpdateLasers += UpgradeLaserButtons;
    }
    private void OnDisable()
    {
        UpgradedLaser -= UpgradeLaserButtons;
        UpgradeDisableOverwrite.OnUpdateLasers -= UpgradeLaserButtons;
    }

    void UpgradeLaserButtons()
    {
        LaserUpgrades laserUpgrades;
        if (position == LaserPosition.Front)
            laserUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades;
        else if (position == LaserPosition.Spread)
            laserUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades;
        else if (position == LaserPosition.Side)
            laserUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades;
        else
            laserUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades;

        ResourceNumber[] costToSend;

        if (upgradeType == LaserUpgradeType.Main)
        {            
            interfaceData.SetBoolButtonVisual(icon, border, upgradeLevelTxt, laserUpgradeInfo.UnlockCost, costs, laserUpgrades.Enabled,
                laserUpgrades.DamageLevel == laserUpgradeInfo.PowerUpgrades.Length && laserUpgrades.CadencyLevel == laserUpgradeInfo.CadencyUpgrades.Length,
                isDisableOverwrite: laserUpgrades.DisableOverwrite);
        }
        else if (upgradeType == LaserUpgradeType.Power)
        {
            if (laserUpgrades.DamageLevel == laserUpgradeInfo.PowerUpgrades.Length)
                costToSend = laserUpgradeInfo.PowerUpgrades[laserUpgrades.DamageLevel - 1].Cost;
            else
                costToSend = laserUpgradeInfo.PowerUpgrades[laserUpgrades.DamageLevel].Cost;
            
            interfaceData.UpdateButtonVisual(laserUpgrades.DamageLevel, laserUpgradeInfo.PowerUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, laserUpgrades.Enabled);
        }
        else if (upgradeType == LaserUpgradeType.FireRate)
        {
            if (laserUpgrades.CadencyLevel == laserUpgradeInfo.CadencyUpgrades.Length)
                costToSend = laserUpgradeInfo.CadencyUpgrades[laserUpgrades.CadencyLevel - 1].Cost;
            else
                costToSend = laserUpgradeInfo.CadencyUpgrades[laserUpgrades.CadencyLevel].Cost;

            interfaceData.UpdateButtonVisual(laserUpgrades.CadencyLevel, laserUpgradeInfo.CadencyUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, laserUpgrades.Enabled);
        }
    }

    public void BuyFrontPower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.DamageLevel == laserUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.DamageLevel].Cost, LaserUpgradeType.Power);
    }
    public void BuyFrontCadency()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel == laserUpgradeInfo.CadencyUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.CadencyUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel].Cost, LaserUpgradeType.FireRate);
    }
    public void UnlockSpread()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.Enabled) return;
        
        if (PlayerCollectiblesCount.ExpendResources(laserUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }       
    }
    public void BuySpreadPower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.DamageLevel == laserUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.DamageLevel].Cost, LaserUpgradeType.Power);
    }
    public void BuySpreadCadency()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel == laserUpgradeInfo.CadencyUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.CadencyUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel].Cost, LaserUpgradeType.FireRate);
    }
    public void UnlockSide()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(laserUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.CadencyLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }
    }
    public void BuySidePower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.DamageLevel == laserUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.DamageLevel].Cost, LaserUpgradeType.Power);
    }
    public void BuySideCadency()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.CadencyLevel == laserUpgradeInfo.CadencyUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.CadencyUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.CadencyLevel].Cost, LaserUpgradeType.FireRate);
    }
    public void UnlockBack()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(laserUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.CadencyLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }
    }
    public void BuyBackPower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.DamageLevel == laserUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.DamageLevel].Cost, LaserUpgradeType.Power);
    }
    public void BuyBackCadency()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.CadencyLevel == laserUpgradeInfo.CadencyUpgrades.Length) return;

        BuyUpgrade(laserUpgradeInfo.CadencyUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.CadencyLevel].Cost, LaserUpgradeType.FireRate);
    }

    void BuyUpgrade(ResourceNumber[] cost, LaserUpgradeType upgradeType)
    {
        if (position == LaserPosition.Front && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == LaserUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.DamageLevel++;
            else if (upgradeType == LaserUpgradeType.FireRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.CadencyLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }
        else if (position == LaserPosition.Spread && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == LaserUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.DamageLevel++;
            else if (upgradeType == LaserUpgradeType.FireRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.CadencyLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }
        else if (position == LaserPosition.Side && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == LaserUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.DamageLevel++;
            else if (upgradeType == LaserUpgradeType.FireRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.CadencyLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }
        else if (position == LaserPosition.Back && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == LaserUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.DamageLevel++;
            else if (upgradeType == LaserUpgradeType.FireRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.CadencyLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedLaser.Invoke();
        }
    }
}