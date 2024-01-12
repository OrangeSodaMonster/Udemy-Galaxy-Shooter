using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    [SerializeField] InputSO input;

    [Header("")]
    [SerializeField] RectTransform pauseCanvas;
    [Header("")]
    [SerializeField] RectTransform gameoverCanvas;
    static RectTransform s_gameoverCanvas;
    [Header("Upgrade Canvas")]
    [SerializeField] RectTransform shipUpgradePage;
    [SerializeField] RectTransform laserUpgradePage;
    [SerializeField] RectTransform shieldUpgradePage;
    [SerializeField] RectTransform ionStreamUpgradePage;
    [SerializeField] RectTransform dronesUpgradePage;

    public static bool isPaused;
    bool hasReleasedPause;
    bool isOnUpgrade;

    private void Awake()
    {
        s_gameoverCanvas = gameoverCanvas;
    }

    void Update()
    {
        if (hasReleasedPause && input.IsPausing && !isPaused && !isOnUpgrade)
        {
            StartPause();
        }
        else if (hasReleasedPause && input.IsPausing && isPaused && !isOnUpgrade)
        {
            LeavePause();
        }
        else if (hasReleasedPause && input.IsPausing && isOnUpgrade)
        {
            DisableUpgradePage();
            hasReleasedPause = false;
        }

            if (!input.IsPausing)
            hasReleasedPause = true;
    }

    public void StartPause()
    {
        pauseCanvas.gameObject.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
        hasReleasedPause = false;
    }

    public void LeavePause()
    {
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        hasReleasedPause = false;
    }

    public void EnableShipUpgradePage()
    {
        if (!isPaused) return;
        pauseCanvas.gameObject.SetActive(false);

        DisableUpgradePage();
        shipUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableLaserUpgradePage()
    {
        if (!isPaused) return;
        pauseCanvas.gameObject.SetActive(false);

        DisableUpgradePage();
        laserUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableShieldUpgradePage()
    {
        if (!isPaused) return;
        pauseCanvas.gameObject.SetActive(false);

        DisableUpgradePage();
        shieldUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableIonStreamUpgradePage()
    {
        if (!isPaused) return;
        pauseCanvas.gameObject.SetActive(false);

        DisableUpgradePage();
        ionStreamUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableDronesUpgradePage()
    {
        if (!isPaused) return;
        pauseCanvas.gameObject.SetActive(false);

        DisableUpgradePage();
        dronesUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void DisableUpgradePage()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false) ;
        ionStreamUpgradePage.gameObject.SetActive(false);
        dronesUpgradePage.gameObject.SetActive(false);

        pauseCanvas.gameObject.SetActive(true);

        isOnUpgrade = false;
    }
    public static void EnableGameoverCanvas()
    {
        s_gameoverCanvas.gameObject.SetActive(true);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}