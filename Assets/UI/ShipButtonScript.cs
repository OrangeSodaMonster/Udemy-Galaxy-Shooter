using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum ShipUpgradeType
{
    Speed, Manobrability, HP, TractorBeam,
}

public class ShipButtonScript : MonoBehaviour
{
    [SerializeField] InterfaceDataHolder interfaceData;
    [Header("")]
    [SerializeField] ShipUpgradeType upgradeType;
    [SerializeField] Image border;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] GameObject[] costs;
    [SerializeField] float clickInterval = 0.2f;

    ShipUpgradesInfo shipUpgradeInfo;

    public static event Action UpgradedShip;

    void Awake()
    {
        shipUpgradeInfo = PlayerUpgradesManager.Instance.ShipUpgradesInfo;        
    }

    private void OnEnable()
    {
        UpgradeShipButtons();
        UpgradedShip += UpgradeShipButtons;
    }
    private void OnDisable()
    {
        UpgradedShip -= UpgradeShipButtons;
    }

    void UpgradeShipButtons()
    {
        ResourceNumber[] costToSend;

        if (upgradeType == ShipUpgradeType.Speed)
        {
            // Previne que se passe do limite do array no ultimo upgrade
            if(PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel == shipUpgradeInfo.SpeedUpgrade.Length)
                costToSend = shipUpgradeInfo.SpeedUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1].Cost;
            else
                costToSend = shipUpgradeInfo.SpeedUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel].Cost;

            interfaceData.UpdateButtonVisual(PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel, shipUpgradeInfo.SpeedUpgrade.Length, icon, border, upgradeLevelTxt, costs,
            costToSend);
        }
        else if (upgradeType == ShipUpgradeType.Manobrability)
        {
            if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel == shipUpgradeInfo.ManobrabilityUpgrade.Length)
                costToSend = shipUpgradeInfo.ManobrabilityUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].Cost;
            else
                costToSend = shipUpgradeInfo.ManobrabilityUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel].Cost;

            interfaceData.UpdateButtonVisual(PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel, shipUpgradeInfo.ManobrabilityUpgrade.Length, icon, border, upgradeLevelTxt, costs,
            costToSend);
        }
        else if (upgradeType == ShipUpgradeType.HP)
        {
            if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel == shipUpgradeInfo.HP_Upgrade.Length)
                costToSend = shipUpgradeInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].Cost;
            else
                costToSend = shipUpgradeInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel].Cost;

            interfaceData.UpdateButtonVisual(PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel, shipUpgradeInfo.HP_Upgrade.Length, icon, border, upgradeLevelTxt, costs,
            costToSend);
        }
        else if (upgradeType == ShipUpgradeType.TractorBeam)
        {
            // O custo est� no index 0 nesse caso, n�o no 1
            if (!PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled)
                interfaceData.SetUnlockedCostVisual(shipUpgradeInfo.TractorBeamUpgrade[0].Cost, costs);

            if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel == shipUpgradeInfo.TractorBeamUpgrade.Length)
                costToSend = shipUpgradeInfo.TractorBeamUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel - 1].Cost;
            else
                costToSend = shipUpgradeInfo.TractorBeamUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel].Cost;
                        
            interfaceData.UpdateButtonVisual(PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel, shipUpgradeInfo.TractorBeamUpgrade.Length, icon, border, upgradeLevelTxt, costs,
            costToSend, PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled, false);            
        }
    }

    public void BuySpeed()
    {
        if (!canClick) return;
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel == shipUpgradeInfo.SpeedUpgrade.Length) return;

        BuyUpgrade(shipUpgradeInfo.SpeedUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel].Cost, ShipUpgradeType.Speed);
        StartCoroutine(ClickIntervalRoutine());
    }
    public void BuyManobrability()
    {
        if(!canClick) return;
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel == shipUpgradeInfo.ManobrabilityUpgrade.Length) return;

        BuyUpgrade(shipUpgradeInfo.ManobrabilityUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel].Cost, ShipUpgradeType.Manobrability);
        StartCoroutine(ClickIntervalRoutine());
    }
    public void BuyHP()
    {
        if(!canClick) return;
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel == shipUpgradeInfo.HP_Upgrade.Length) return;

        BuyUpgrade(shipUpgradeInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel].Cost, ShipUpgradeType.HP);

        PlayerHP.ApplyHPUpgrade();
        StartCoroutine(ClickIntervalRoutine());
    }
    public void BuyTractor()
    {
        if(!canClick) return;
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel == shipUpgradeInfo.TractorBeamUpgrade.Length) return;

        if (!PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled)
        {
            if (PlayerCollectiblesCount.ExpendResources(shipUpgradeInfo.TractorBeamUpgrade[0].Cost))
            {
                PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled = true;
                PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel = 1;

                AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
                UpgradedShip.Invoke();
            }
        }            
        else
            BuyUpgrade(shipUpgradeInfo.TractorBeamUpgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel].Cost, ShipUpgradeType.TractorBeam);
        StartCoroutine(ClickIntervalRoutine());
    }

    bool canClick = true;
    IEnumerator ClickIntervalRoutine()
    {
        canClick = false;
        yield return new WaitForSecondsRealtime(clickInterval);

        canClick = true;
    }

    void BuyUpgrade(ResourceNumber[] cost, ShipUpgradeType upgradeType)
    {
        if (PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == ShipUpgradeType.Speed)
                PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel++;
            else if (upgradeType == ShipUpgradeType.Manobrability)
                PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel++;
            else if (upgradeType == ShipUpgradeType.HP)
                PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel++;
            else if (upgradeType == ShipUpgradeType.TractorBeam)
                PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedShip.Invoke();
        }
    }  



}