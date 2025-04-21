using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum ShieldUpgradeType
{
    Main, Strenght, RecoveryRate,
}
public enum ShieldPosition
{
    Front, Right, Left, Back,
}

public class ShieldButtonScript : MonoBehaviour
{

    [SerializeField] InterfaceDataHolder interfaceData;
    [Header("")]
    [SerializeField] ShieldPosition position;
    [SerializeField] ShieldUpgradeType upgradeType;
    [SerializeField] Image border;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] GameObject[] costs;

    ShieldsUpgradesInfo shieldUpgradeInfo;

    public static event Action UpgradedShield;

    void Awake()
    {
        shieldUpgradeInfo = PlayerUpgradesManager.Instance.ShieldUpgradesInfo;
    }

    private void OnEnable()
    {
        UpgradeShieldButtons();
        UpgradedShield += UpgradeShieldButtons;
        UpgradeDisableOverwrite.OnUpdateShield += UpgradeShieldButtons;
    }
    private void OnDisable()
    {
        UpgradedShield -= UpgradeShieldButtons;
        UpgradeDisableOverwrite.OnUpdateShield -= UpgradeShieldButtons;
    }

    void UpgradeShieldButtons()
    {
        ShieldUpgrades shieldUpgrades;
        if (position == ShieldPosition.Front)
            shieldUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades;
        else if (position == ShieldPosition.Right)
            shieldUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades;
        else if (position == ShieldPosition.Left)
            shieldUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades;
        else
            shieldUpgrades = PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades;

        ResourceNumber[] costToSend;

        if (upgradeType == ShieldUpgradeType.Main)
        {
            interfaceData.SetBoolButtonVisual(icon, border, upgradeLevelTxt, shieldUpgradeInfo.UnlockCost, costs, shieldUpgrades.Enabled,
                shieldUpgrades.ResistenceLevel == shieldUpgradeInfo.StrenghtUpgrades.Length && shieldUpgrades.RecoveryLevel == shieldUpgradeInfo.RecoveryUpgrades.Length,
                isDisableOverwrite: shieldUpgrades.DisableOverwrite);
        }
        else if (upgradeType == ShieldUpgradeType.Strenght)
        {
            if (shieldUpgrades.ResistenceLevel == shieldUpgradeInfo.StrenghtUpgrades.Length)
                costToSend = shieldUpgradeInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].Cost;
            else
                costToSend = shieldUpgradeInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel].Cost;

            interfaceData.UpdateButtonVisual(shieldUpgrades.ResistenceLevel, shieldUpgradeInfo.StrenghtUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, shieldUpgrades.Enabled);
        }
        else if (upgradeType == ShieldUpgradeType.RecoveryRate)
        {
            if (shieldUpgrades.RecoveryLevel == shieldUpgradeInfo.RecoveryUpgrades.Length)
                costToSend = shieldUpgradeInfo.RecoveryUpgrades[shieldUpgrades.RecoveryLevel - 1].Cost;
            else
                costToSend = shieldUpgradeInfo.RecoveryUpgrades[shieldUpgrades.RecoveryLevel].Cost;

            interfaceData.UpdateButtonVisual(shieldUpgrades.RecoveryLevel, shieldUpgradeInfo.RecoveryUpgrades.Length,
                icon, border, upgradeLevelTxt, costs, costToSend, shieldUpgrades.Enabled);
        }
    }
    public void UnlockFront()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(shieldUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.ResistenceLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.RecoveryLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyFrontStrenght()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.ResistenceLevel == shieldUpgradeInfo.StrenghtUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.StrenghtUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.ResistenceLevel].Cost, ShieldUpgradeType.Strenght);
    }
    public void BuyFrontRecovery()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.RecoveryLevel == shieldUpgradeInfo.RecoveryUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.RecoveryUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.RecoveryLevel].Cost, ShieldUpgradeType.RecoveryRate);
    }
    public void UnlockRight()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(shieldUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.ResistenceLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.RecoveryLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyRightStrenght()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.ResistenceLevel == shieldUpgradeInfo.StrenghtUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.StrenghtUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.ResistenceLevel].Cost, ShieldUpgradeType.Strenght);
    }
    public void BuyRightRecovery()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.RecoveryLevel == shieldUpgradeInfo.RecoveryUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.RecoveryUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.RecoveryLevel].Cost, ShieldUpgradeType.RecoveryRate);
    }
    public void UnlockLeft()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(shieldUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.ResistenceLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.RecoveryLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyLeftStrenght()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.ResistenceLevel == shieldUpgradeInfo.StrenghtUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.StrenghtUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.ResistenceLevel].Cost, ShieldUpgradeType.Strenght);
    }
    public void BuyLeftRecovery()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.RecoveryLevel == shieldUpgradeInfo.RecoveryUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.RecoveryUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.RecoveryLevel].Cost, ShieldUpgradeType.RecoveryRate);
    }
    public void UnlockBack()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.Enabled) return;

        if (PlayerCollectiblesCount.ExpendResources(shieldUpgradeInfo.UnlockCost))
        {
            PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.Enabled = true;
            PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.ResistenceLevel = 1;
            PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.RecoveryLevel = 1;

            AudioManager.Instance.UnlockUpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }
    public void BuyBackStrenght()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.ResistenceLevel == shieldUpgradeInfo.StrenghtUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.StrenghtUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.ResistenceLevel].Cost, ShieldUpgradeType.Strenght);
    }
    public void BuyBackRecovery()
    {
        if (PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.RecoveryLevel == shieldUpgradeInfo.RecoveryUpgrades.Length) return;

        BuyUpgrade(shieldUpgradeInfo.RecoveryUpgrades[PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.RecoveryLevel].Cost, ShieldUpgradeType.RecoveryRate);
    }

    void BuyUpgrade(ResourceNumber[] cost, ShieldUpgradeType upgradeType)
    {
        if (position == ShieldPosition.Front && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == ShieldUpgradeType.Strenght)
                PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.ResistenceLevel++;
            else if (upgradeType == ShieldUpgradeType.RecoveryRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.RecoveryLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else if (position == ShieldPosition.Right && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == ShieldUpgradeType.Strenght)
                PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.ResistenceLevel++;
            else if (upgradeType == ShieldUpgradeType.RecoveryRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.RecoveryLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else if (position == ShieldPosition.Left && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == ShieldUpgradeType.Strenght)
                PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.ResistenceLevel++;
            else if (upgradeType == ShieldUpgradeType.RecoveryRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.RecoveryLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else if (position == ShieldPosition.Back && PlayerCollectiblesCount.ExpendResources(cost))
        {
            if (upgradeType == ShieldUpgradeType.Strenght)
                PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.ResistenceLevel++;
            else if (upgradeType == ShieldUpgradeType.RecoveryRate)
                PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.RecoveryLevel++;

            AudioManager.Instance.UpgradeSound.PlayFeedbacks();
            UpgradedShield.Invoke();
        }
        else AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();
    }

}