using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum IonStreamUpgradeType
{
    Main, Power, FireRate, Range, HitNumber 
}

public class IonStreamButtomScripts : MonoBehaviour
{
    [SerializeField] InterfaceDataHolder interfaceData;
    [Header("")]
    [SerializeField] IonStreamUpgradeType upgradeType;
    [SerializeField] Image border;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] GameObject[] costs;

    IonStreamUpgradesInfo ionStreamUpgradeInfo;

    public static event Action UpgradedIonStream;

    void Awake()
    {
        ionStreamUpgradeInfo = PlayerUpgradesManager.Instance.IonStreamUpgradesInfo;
    }

    private void OnEnable()
    {
        UpgradeIonStreamButtons();
        UpgradedIonStream += UpgradeIonStreamButtons;
    }
    private void OnDisable()
    {
        UpgradedIonStream -= UpgradeIonStreamButtons;
    }

    void UpgradeIonStreamButtons()
    {
        IonStreamUpgrades ionStreamUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades;

        ResourceNumber[] costToSend;

        if (upgradeType == IonStreamUpgradeType.Main)
        {
            interfaceData.SetBoolButtonVisual(icon, border, upgradeLevelTxt, ionStreamUpgradeInfo.UnlockCost, costs, ionStreamUpgrades.Enabled,
                ionStreamUpgrades.DamageLevel == ionStreamUpgradeInfo.PowerUpgrades.Length && ionStreamUpgrades.CadencyLevel == ionStreamUpgradeInfo.CadencyUpgrades.Length
                && ionStreamUpgrades.RangeLevel == ionStreamUpgradeInfo.RangeUpgrades.Length && ionStreamUpgrades.NumberHitsLevel == ionStreamUpgradeInfo.HitNumUpgrades.Length);
        }
        else if (upgradeType == IonStreamUpgradeType.Power)
        {
            if (ionStreamUpgrades.DamageLevel == ionStreamUpgradeInfo.PowerUpgrades.Length)
                costToSend = ionStreamUpgradeInfo.PowerUpgrades[ionStreamUpgrades.DamageLevel - 1].Cost;
            else
                costToSend = ionStreamUpgradeInfo.PowerUpgrades[ionStreamUpgrades.DamageLevel].Cost;

            interfaceData.UpdateButtonVisual(ionStreamUpgrades.DamageLevel, ionStreamUpgradeInfo.PowerUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, ionStreamUpgrades.Enabled);
        }
        else if (upgradeType == IonStreamUpgradeType.FireRate)
        {
            if (ionStreamUpgrades.CadencyLevel == ionStreamUpgradeInfo.CadencyUpgrades.Length)
                costToSend = ionStreamUpgradeInfo.CadencyUpgrades[ionStreamUpgrades.CadencyLevel - 1].Cost;
            else
                costToSend = ionStreamUpgradeInfo.CadencyUpgrades[ionStreamUpgrades.CadencyLevel].Cost;

            interfaceData.UpdateButtonVisual(ionStreamUpgrades.CadencyLevel, ionStreamUpgradeInfo.CadencyUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, ionStreamUpgrades.Enabled);
        }
        else if (upgradeType == IonStreamUpgradeType.Range)
        {
            if (ionStreamUpgrades.RangeLevel == ionStreamUpgradeInfo.RangeUpgrades.Length)
                costToSend = ionStreamUpgradeInfo.RangeUpgrades[ionStreamUpgrades.RangeLevel - 1].Cost;
            else
                costToSend = ionStreamUpgradeInfo.RangeUpgrades[ionStreamUpgrades.RangeLevel].Cost;

            interfaceData.UpdateButtonVisual(ionStreamUpgrades.RangeLevel, ionStreamUpgradeInfo.RangeUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, ionStreamUpgrades.Enabled);
        }
        else if (upgradeType == IonStreamUpgradeType.HitNumber)
        {
            if (ionStreamUpgrades.NumberHitsLevel == ionStreamUpgradeInfo.HitNumUpgrades.Length)
                costToSend = ionStreamUpgradeInfo.HitNumUpgrades[ionStreamUpgrades.NumberHitsLevel - 1].Cost;
            else
                costToSend = ionStreamUpgradeInfo.HitNumUpgrades[ionStreamUpgrades.NumberHitsLevel].Cost;

            interfaceData.UpdateButtonVisual(ionStreamUpgrades.NumberHitsLevel, ionStreamUpgradeInfo.HitNumUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, ionStreamUpgrades.Enabled);
        }
    }
    public void UnlockIonStream()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(ionStreamUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.DamageLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.CadencyLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.RangeLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.NumberHitsLevel = 1;
            UpgradedIonStream.Invoke();
        }
    }
    public void BuyPower()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.DamageLevel == ionStreamUpgradeInfo.PowerUpgrades.Length) return;

        BuyUpgrade(ionStreamUpgradeInfo.PowerUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.DamageLevel].Cost, IonStreamUpgradeType.Power);
    }
    public void BuyFireRate()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.CadencyLevel == ionStreamUpgradeInfo.CadencyUpgrades.Length) return;

        BuyUpgrade(ionStreamUpgradeInfo.CadencyUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.CadencyLevel].Cost, IonStreamUpgradeType.FireRate);
    }
    public void BuyRange()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.RangeLevel == ionStreamUpgradeInfo.RangeUpgrades.Length) return;

        BuyUpgrade(ionStreamUpgradeInfo.RangeUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.RangeLevel].Cost, IonStreamUpgradeType.Range);
    }
    public void BuyHitNumber()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.NumberHitsLevel == ionStreamUpgradeInfo.HitNumUpgrades.Length) return;

        BuyUpgrade(ionStreamUpgradeInfo.HitNumUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.NumberHitsLevel].Cost, IonStreamUpgradeType.HitNumber);
    }

    void BuyUpgrade(ResourceNumber[] cost, IonStreamUpgradeType upgradeType)
    {
        if (PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == IonStreamUpgradeType.Power)
                PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.DamageLevel++;
            else if (upgradeType == IonStreamUpgradeType.FireRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.CadencyLevel++;
            else if (upgradeType == IonStreamUpgradeType.Range)
                PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.RangeLevel++;
            else if (upgradeType == IonStreamUpgradeType.HitNumber)
                PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.NumberHitsLevel++;

            UpgradedIonStream.Invoke();
        }        
    }
}