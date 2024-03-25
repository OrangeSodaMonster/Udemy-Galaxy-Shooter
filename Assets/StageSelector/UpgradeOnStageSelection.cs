using DG.Tweening;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UpgradeOnStageSelection : MonoBehaviour
{
    [SerializeField] RectTransform upgradePanel;
    [SerializeField] RectTransform upgradeButton;
    [SerializeField] RectTransform returnToMainMenuButton;
    [SerializeField] MenuNavigationScript baseCanvasMenuNavigation;
    [Space] 
    [SerializeField] RectTransform shipUpgradePage;
    [SerializeField] RectTransform laserUpgradePage;
    [SerializeField] RectTransform shieldUpgradePage;
    [SerializeField] RectTransform ionStreamUpgradePage;
    [SerializeField] RectTransform dronesUpgradePage;

    public void OpenUpgradesPanel()
    {
        StartCoroutine(DisableEnableUpgradeDelay(upgradePanel));
        upgradeButton.gameObject.SetActive(false);
        returnToMainMenuButton.gameObject.SetActive(false);
        baseCanvasMenuNavigation.enabled = false;
    }
    public void CloseUpgradesPanel()
    {
        DisableAllCanvas();
        upgradeButton.gameObject.SetActive(true);
        returnToMainMenuButton.gameObject.SetActive(true);
        baseCanvasMenuNavigation.enabled = true;
    }

    public void EnableShipUpgradePage()
    {
        StartCoroutine(DisableEnableUpgradeDelay(shipUpgradePage));
    }
    public void EnableLaserUpgradePage()
    {
        StartCoroutine(DisableEnableUpgradeDelay(laserUpgradePage));
    }
    public void EnableShieldUpgradePage()
    {
        StartCoroutine(DisableEnableUpgradeDelay(shieldUpgradePage));
    }
    public void EnableIonStreamUpgradePage()
    {
        StartCoroutine(DisableEnableUpgradeDelay(ionStreamUpgradePage));
    }
    public void EnableDronesUpgradePage()
    {
        StartCoroutine(DisableEnableUpgradeDelay(dronesUpgradePage));
    }
    
    public void DisableAllCanvas()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false);
        ionStreamUpgradePage.gameObject.SetActive(false);
        dronesUpgradePage.gameObject.SetActive(false);
        upgradePanel.gameObject.SetActive(false);
    }

    IEnumerator DisableEnableUpgradeDelay(RectTransform canvasToEnable)
    {
        yield return null;

        DisableAllCanvas();

        canvasToEnable.gameObject.SetActive(true);
    }

    IEnumerator DisableEnablePanelDelay()
    {
        yield return null;

        DisableAllCanvas();

        upgradePanel.gameObject.SetActive(true);
    }

    public void ReturnToPanel()
    {
        StartCoroutine(DisableEnablePanelDelay());
    }
}