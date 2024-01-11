using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasScaler scaler;
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] InputSO input;

    [Header("Upgrade Canvas")]
    [SerializeField] RectTransform shipUpgradePage;
    [SerializeField] RectTransform laserUpgradePage;
    [SerializeField] RectTransform shieldUpgradePage;
    [SerializeField] RectTransform ionStreamUpgradePage;
    [SerializeField] RectTransform dronesUpgradePage;

    public static bool isPaused;
    bool hasReleasedPause;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        scaler = GetComponent<CanvasScaler>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if (hasReleasedPause && input.IsPausing && !isPaused)
        {
            StartPause();
        }
        else if (hasReleasedPause && input.IsPausing && isPaused)
        {
            LeavePause();
        }

        if (!input.IsPausing)
            hasReleasedPause = true;
    }

    public void StartPause()
    {
        canvas.enabled = true;
        scaler.enabled = true;
        raycaster.enabled = true;
        isPaused = true;
        Time.timeScale = 0;
        hasReleasedPause = false;
    }

    public void LeavePause()
    {
        canvas.enabled = false;
        scaler.enabled = false;
        raycaster.enabled = false;
        isPaused = false;
        Time.timeScale = 1;
        hasReleasedPause = false;
    }

    public void EnableShipUpgradePage()
    {
        DisableUpgradePage();
        shipUpgradePage.gameObject.SetActive(true);
    }
    public void EnableLaserUpgradePage()
    {
        DisableUpgradePage();
        laserUpgradePage.gameObject.SetActive(true);
    }
    public void EnableShieldUpgradePage()
    {
        DisableUpgradePage();
        shieldUpgradePage.gameObject.SetActive(true);
    }  
    public void EnableIonStreamUpgradePage()
    {
        DisableUpgradePage();
        ionStreamUpgradePage.gameObject.SetActive(true);
    }
    public void EnableDronesUpgradePage()
    {
        DisableUpgradePage();
        dronesUpgradePage.gameObject.SetActive(true);
    }
    public void DisableUpgradePage()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false) ;
        ionStreamUpgradePage.gameObject.SetActive(false);
        dronesUpgradePage.gameObject.SetActive(false);
    }
    
}