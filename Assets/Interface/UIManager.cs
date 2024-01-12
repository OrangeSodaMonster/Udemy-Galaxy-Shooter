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

    //scripts to disable if is pausing
    PlayerMove playerMove;
    PlayerLasers playerLasers;
    BombScript bombScript;

    private void Awake()
    {
        s_gameoverCanvas = gameoverCanvas;
    }

    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        playerLasers = FindObjectOfType<PlayerLasers>();
        bombScript = FindObjectOfType<BombScript>();
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

        playerMove.enabled = false;
        playerLasers.enabled = false;
        bombScript.enabled = false;

        hasReleasedPause = false;
    }

    public void LeavePause()
    {
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;

        playerMove.enabled = true;
        playerLasers.enabled = true;
        bombScript.enabled = true;

        hasReleasedPause = false;
    }

    public void EnableShipUpgradePage()
    {
        if (!isPaused) return;

        DisableUpgradePage();
        pauseCanvas.gameObject.SetActive(false);
        shipUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableLaserUpgradePage()
    {
        if (!isPaused) return;

        DisableUpgradePage();
        pauseCanvas.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableShieldUpgradePage()
    {
        if (!isPaused) return;

        DisableUpgradePage();
        pauseCanvas.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableIonStreamUpgradePage()
    {
        if (!isPaused) return;

        DisableUpgradePage();
        pauseCanvas.gameObject.SetActive(false);
        ionStreamUpgradePage.gameObject.SetActive(true);
        isOnUpgrade = true;
    }
    public void EnableDronesUpgradePage()
    {
        if (!isPaused) return;

        DisableUpgradePage();
        pauseCanvas.gameObject.SetActive(false);
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
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}