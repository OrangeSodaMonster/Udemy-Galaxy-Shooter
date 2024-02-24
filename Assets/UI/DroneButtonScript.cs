using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum DroneUpgradeType
{
    Main = 1, Power = 2, Range = 3, Heal = 4
}
enum DroneNumber
{
    One = 1, Two = 2, Three = 3,
}

public class DroneButtonScript : MonoBehaviour
{
    [SerializeField] InterfaceDataHolder interfaceData;
    [Header("")]
    [SerializeField] DroneNumber droneNumber;
    [SerializeField] DroneUpgradeType upgradeType;
    [SerializeField] Image border;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] GameObject[] costs;

    DroneUpgradeInfo droneUpgradeInfo;

    public static event Action UpgradedDrone;

    void Awake()
    {
        droneUpgradeInfo = PlayerUpgradesManager.Instance.DroneUpgradesInfo;
    }

    private void OnEnable()
    {
        UpgradeDroneButtons();
        UpgradedDrone += UpgradeDroneButtons;
        UpgradeDisableOverwrite.OnUpdateDrones += UpgradeDroneButtons;
    }
    private void OnDisable()
    {
        UpgradedDrone -= UpgradeDroneButtons;
        UpgradeDisableOverwrite.OnUpdateDrones -= UpgradeDroneButtons;
    }

    void UpgradeDroneButtons()
    {        
        DronesUpgrades droneUpgrades;
        if (droneNumber == DroneNumber.One)
            droneUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades;
        else if (droneNumber == DroneNumber.Two)
            droneUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades;
        else
            droneUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades;

        ResourceNumber[] costToSend;

        if (upgradeType == DroneUpgradeType.Main)
        {
            interfaceData.SetBoolButtonVisual(icon, border, upgradeLevelTxt, droneUpgradeInfo.UnlockCost, costs, droneUpgrades.Enabled,
                droneUpgrades.DamageLevel == droneUpgradeInfo.PowerUpgrades.Length && droneUpgrades.RangeLevel == droneUpgradeInfo.RangeUpgrades.Length
                && droneUpgrades.HealingLevel == droneUpgradeInfo.HealUpgrade.Length, isDisableOverwrite: droneUpgrades.DisableOverwrite);
        }
        else if (upgradeType == DroneUpgradeType.Power)
        {
            if (droneUpgrades.DamageLevel == droneUpgradeInfo.PowerUpgrades.Length)
                costToSend = droneUpgradeInfo.PowerUpgrades[droneUpgrades.DamageLevel - 1].Cost;
            else
                costToSend = droneUpgradeInfo.PowerUpgrades[droneUpgrades.DamageLevel].Cost;

            interfaceData.UpdateButtonVisual(droneUpgrades.DamageLevel, droneUpgradeInfo.PowerUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, droneUpgrades.Enabled);            
        }
        else if (upgradeType == DroneUpgradeType.Range)
        {
            if (droneUpgrades.RangeLevel == droneUpgradeInfo.RangeUpgrades.Length)
                costToSend = droneUpgradeInfo.RangeUpgrades[droneUpgrades.RangeLevel - 1].Cost;
            else
                costToSend = droneUpgradeInfo.RangeUpgrades[droneUpgrades.RangeLevel].Cost;

            interfaceData.UpdateButtonVisual(droneUpgrades.RangeLevel, droneUpgradeInfo.RangeUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, droneUpgrades.Enabled);
        }
        else if (upgradeType == DroneUpgradeType.Heal)
        {
            if (droneUpgrades.HealingLevel == droneUpgradeInfo.HealUpgrade.Length)
                costToSend = droneUpgradeInfo.HealUpgrade[droneUpgrades.HealingLevel - 1].Cost;
            else
                costToSend = droneUpgradeInfo.HealUpgrade[droneUpgrades.HealingLevel].Cost;

            interfaceData.UpdateButtonVisual(droneUpgrades.HealingLevel, droneUpgradeInfo.HealUpgrade.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, droneUpgrades.Enabled);
        }
    }
    public void UnlockDroneOne()
    {

        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(droneUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.RangeLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.HealingLevel = 1;
            UpgradedDrone.Invoke();
            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyOnePower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.DamageLevel == droneUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(droneUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.DamageLevel].Cost, DroneUpgradeType.Power);
    }

    public void BuyOneRange()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.RangeLevel == droneUpgradeInfo.RangeUpgrades.Length) return;

        BuyUpgrade(droneUpgradeInfo.RangeUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.RangeLevel].Cost, DroneUpgradeType.Range);
    }
    public void BuyOneHeal()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.HealingLevel == droneUpgradeInfo.HealUpgrade.Length) return;

        BuyUpgrade(droneUpgradeInfo.HealUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.HealingLevel].Cost, DroneUpgradeType.Heal);
    }
    public void UnlockDroneTwo()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(droneUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.RangeLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.HealingLevel = 1;
            UpgradedDrone.Invoke();
            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyTwoPower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.DamageLevel == droneUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(droneUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.DamageLevel].Cost, DroneUpgradeType.Power);
    }
    public void BuyTwoRange()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.RangeLevel == droneUpgradeInfo.RangeUpgrades.Length) return;

        BuyUpgrade(droneUpgradeInfo.RangeUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.RangeLevel].Cost, DroneUpgradeType.Range);
    }
    public void BuyTwoHeal()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.HealingLevel == droneUpgradeInfo.HealUpgrade.Length) return;

        BuyUpgrade(droneUpgradeInfo.HealUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.HealingLevel].Cost, DroneUpgradeType.Heal);
    }
    public void UnlockDroneThree()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(droneUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.RangeLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.HealingLevel = 1;
            UpgradedDrone.Invoke();
            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyThreePower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.DamageLevel == droneUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(droneUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.DamageLevel].Cost, DroneUpgradeType.Power);
    }
    public void BuyThreeRange()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.RangeLevel == droneUpgradeInfo.RangeUpgrades.Length) return;

        BuyUpgrade(droneUpgradeInfo.RangeUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.RangeLevel].Cost, DroneUpgradeType.Range);
    }
    public void BuyThreeHeal()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.HealingLevel == droneUpgradeInfo.HealUpgrade.Length) return;

        BuyUpgrade(droneUpgradeInfo.HealUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.HealingLevel].Cost, DroneUpgradeType.Heal);
    }   

    void BuyUpgrade(ResourceNumber[] cost, DroneUpgradeType upgradeType)
    {
        if (droneNumber == DroneNumber.One && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == DroneUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.DamageLevel++;
            else if (upgradeType == DroneUpgradeType.Range)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.RangeLevel++;
            else if (upgradeType == DroneUpgradeType.Heal)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.HealingLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedDrone.Invoke();
        }
        else if (droneNumber == DroneNumber.Two && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == DroneUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.DamageLevel++;
            else if (upgradeType == DroneUpgradeType.Range)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.RangeLevel++;
            else if (upgradeType == DroneUpgradeType.Heal)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.HealingLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedDrone.Invoke();
        }
        else if (droneNumber == DroneNumber.Three && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == DroneUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.DamageLevel++;
            else if (upgradeType == DroneUpgradeType.Range)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.RangeLevel++;
            else if (upgradeType == DroneUpgradeType.Heal)
                PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.HealingLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedDrone.Invoke();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();

    }
}