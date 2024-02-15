using MoreMountains.Tools;
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
    [SerializeField] RectTransform configPage;

    //public bool IsPaused;
    bool hasReleasedPause;
    bool isOnPage;

    //scripts to disable if is pausing
    PlayerMove playerMove;
    PlayerLasers playerLasers;
    BombScript bombScript;

    public static UIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

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
        if (hasReleasedPause && input.IsPausing && !GameStatus.IsPaused && !isOnPage && MySceneManager.IsFeedbackEnabled)
        {
            StartPause();
        }
        else if (hasReleasedPause && input.IsPausing && GameStatus.IsPaused && !isOnPage)
        {
            LeavePause();
        }
        else if (hasReleasedPause && input.IsPausing && isOnPage)
        {
            //DisableAllCanvas();
            hasReleasedPause = false;
        }

            if (!input.IsPausing)
            hasReleasedPause = true;
    }

    public void StartPause()
    {
        pauseCanvas.gameObject.SetActive(true);
        GameStatus.IsPaused = true;
        Time.timeScale = 0;

        if(playerMove != null) playerMove.enabled = false;
        if (playerLasers != null) playerLasers.enabled = false;
        if (bombScript != null) bombScript.enabled = false;

        hasReleasedPause = false;

        AudioTrackConfig.Instance.MuteVFX();
    }

    public void LeavePause()
    {
        pauseCanvas.gameObject.SetActive(false);
        GameStatus.IsPaused = false;
        Time.timeScale = 1;

        playerMove.enabled = true;
        playerLasers.enabled = true;
        bombScript.enabled = true;

        hasReleasedPause = false;

        AudioTrackConfig.Instance.UnmuteVFX();
    }

    public void EnableShipUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(shipUpgradePage));
        isOnPage = true;
    }
    public void EnableLaserUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(laserUpgradePage));
        isOnPage = true;
    }
    public void EnableShieldUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(shieldUpgradePage));
        isOnPage = true;
    }
    public void EnableIonStreamUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(ionStreamUpgradePage));
        isOnPage = true;
    }
    public void EnableDronesUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(dronesUpgradePage));
        isOnPage = true;
    }
    public void EnableConfigPage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(configPage));
        isOnPage = true;
    }
    public void DisableAllCanvas()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false);
        ionStreamUpgradePage.gameObject.SetActive(false);
        dronesUpgradePage.gameObject.SetActive(false);
        configPage.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);         
    }

    IEnumerator DisableEnableUpgradeDelay(RectTransform canvasToEnable)
    {
        yield return null;

        DisableAllCanvas();

        canvasToEnable.gameObject.SetActive(true);
        isOnPage = true;
    }

    IEnumerator DisableEnablePauseDelay()
    {
        yield return null;

        DisableAllCanvas();

        pauseCanvas.gameObject.SetActive(true);
        isOnPage = false;
    }

    public void ReturnToPauseCanvas()
    {
        StartCoroutine(DisableEnablePauseDelay());
    }

    public static void EnableGameoverCanvas()
    {
        s_gameoverCanvas.gameObject.SetActive(true);
    }
    public void RestartScene()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
        GameStatus.IsPaused = false;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
    }

}