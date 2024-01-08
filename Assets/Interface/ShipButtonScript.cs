using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[Serializable]
enum UpgradeType
{
    Speed, Manobrability, HP, TractorBeam,
}

public class ShipButtonScript : MonoBehaviour
{
    [SerializeField] InterfaceDataHolder interfaceData;
    [Header("")]
    [SerializeField] UpgradeType upgradeType;
    [SerializeField] Image border;
    [SerializeField] TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] Image costOne;
    [SerializeField] TextMeshProUGUI costOneTxt;
    [SerializeField] Image costTwo;
    [SerializeField] TextMeshProUGUI costTwoTxt;

    ShipUpgrades currentUpgrades;
    ShipUpgradesInfo shipUpgradeInfo;

    void Start()
    {
        currentUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades;
        shipUpgradeInfo = PlayerUpgradesManager.Instance.ShipUpgradesInfo;

        UpgradeShipButtons();
    }

    void Update()
    {
        
    }

    public void UpgradeShipButtons()
    {
        if(upgradeType == UpgradeType.Speed)
            UpdateNonBoolButton(currentUpgrades.SpeedLevel, shipUpgradeInfo.SpeedUpgrade.Length,
            shipUpgradeInfo.SpeedUpgrade[currentUpgrades.SpeedLevel].Cost);
        else if (upgradeType == UpgradeType.Manobrability)
            UpdateNonBoolButton(currentUpgrades.ManobrabilityLevel, shipUpgradeInfo.ManobrabilityUpgrade.Length,
            shipUpgradeInfo.ManobrabilityUpgrade[currentUpgrades.ManobrabilityLevel].Cost);
        else if (upgradeType == UpgradeType.HP)
            UpdateNonBoolButton(currentUpgrades.HPLevel, shipUpgradeInfo.HP_Upgrade.Length,
            shipUpgradeInfo.HP_Upgrade[currentUpgrades.HPLevel].Cost);
        else if (upgradeType == UpgradeType.TractorBeam)
            UpdateNonBoolButton(currentUpgrades.TractorBeamLevel, shipUpgradeInfo.TractorBeamUpgrade.Length,
            shipUpgradeInfo.TractorBeamUpgrade[currentUpgrades.TractorBeamLevel].Cost);
    }

    public void UpdateNonBoolButton(int upgradeLevel, int upgradeInfoLenght, ResourceNumber[] resourceNumber1)
    {
        upgradeLevelTxt.text = $"{upgradeLevel}/{upgradeInfoLenght}";

        if (upgradeLevel == upgradeInfoLenght)
        {
            border.color = interfaceData.maxedColor;
            upgradeLevelTxt.color = interfaceData.maxedColor;
            costOne.enabled = false;
            costOneTxt.enabled = false;
            costTwo.enabled = false;
            costTwoTxt.enabled = false;
        }
        else
        {
            border.color = interfaceData.boughtColor;
            upgradeLevelTxt.color = interfaceData.boughtColor;

            SetCost(resourceNumber1[0], costOne, costOneTxt);
            SetCost(resourceNumber1[1], costTwo, costTwoTxt);
        }        
    }

    private void SetCost(ResourceNumber resourceNumber, Image costImage, TextMeshProUGUI costText)
    {
        if (resourceNumber.ResourceType == ResourceType.Metal)
        {
            costImage.sprite = interfaceData.metalSprite;
            costText.text = resourceNumber.Amount.ToString();
            //costOneTxt.color = interfaceData.metalColor;
        }
        else if (resourceNumber.ResourceType == ResourceType.Alloy)
        {
            costImage.sprite = interfaceData.alloySprite;
            costText.text = resourceNumber.Amount.ToString();
            //costOneTxt.color = interfaceData.alloyColor;
        }
        else if (resourceNumber.ResourceType == ResourceType.EnergyCristal)
        {
            costImage.sprite = interfaceData.energyCristalSprite;
            costText.text = resourceNumber.Amount.ToString();
            //costOneTxt.color = interfaceData.energyCristalColor;
        }
        else if (resourceNumber.ResourceType == ResourceType.CondensedEnergyCristal)
        {
            costImage.sprite = interfaceData.condensedCristalSprite;
            costText.text = resourceNumber.Amount.ToString();
            //costOneTxt.color = interfaceData.condensedCristalColor;
        }
    }

}