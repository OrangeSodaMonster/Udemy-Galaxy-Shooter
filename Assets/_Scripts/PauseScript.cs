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
        shipUpgradePage.gameObject.SetActive(true);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false);

    }
    public void EnableLaserUpgradePage()
    {
        laserUpgradePage.gameObject.SetActive(true);
        shipUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false);

    }
    public void EnableShieldUpgradePage()
    {
        shieldUpgradePage.gameObject.SetActive(true);
        laserUpgradePage.gameObject.SetActive(false);
        shipUpgradePage.gameObject.SetActive(false);

    }
    public void DisableUpgradePage()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false) ;
    }
    
}