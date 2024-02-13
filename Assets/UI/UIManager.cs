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

    //public bool IsPaused;
    bool hasReleasedPause;
    bool isOnUpgrade;

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
        if (hasReleasedPause && input.IsPausing && !GameStatus.IsPaused && !isOnUpgrade && MySceneManager.IsFeedbackEnabled)
        {
            StartPause();
        }
        else if (hasReleasedPause && input.IsPausing && GameStatus.IsPaused && !isOnUpgrade)
        {
            LeavePause();
        }
        else if (hasReleasedPause && input.IsPausing && isOnUpgrade)
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

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
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

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
    }

    public void EnableShipUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(shipUpgradePage));
        isOnUpgrade = true;
    }
    public void EnableLaserUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(laserUpgradePage));
        isOnUpgrade = true;
    }
    public void EnableShieldUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(shieldUpgradePage));
        isOnUpgrade = true;
    }
    public void EnableIonStreamUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(ionStreamUpgradePage));
        isOnUpgrade = true;
    }
    public void EnableDronesUpgradePage()
    {
        if (!GameStatus.IsPaused) return;

        StartCoroutine(DisableEnableUpgradeDelay(dronesUpgradePage));
        isOnUpgrade = true;
    }
    public void DisableAllCanvas()
    {
        shipUpgradePage.gameObject.SetActive(false);
        laserUpgradePage.gameObject.SetActive(false);
        shieldUpgradePage.gameObject.SetActive(false);
        ionStreamUpgradePage.gameObject.SetActive(false);
        dronesUpgradePage.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);         
    }

    IEnumerator DisableEnableUpgradeDelay(RectTransform canvasToEnable)
    {
        yield return null;

        DisableAllCanvas();

        canvasToEnable.gameObject.SetActive(true);
        isOnUpgrade = true;
    }

    IEnumerator DisableEnablePauseDelay()
    {
        yield return null;

        DisableAllCanvas();

        pauseCanvas.gameObject.SetActive(true);
        isOnUpgrade = false;
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